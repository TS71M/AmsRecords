using static AmsRecords.Irrigation.IrrigationAdvisorDesignDtos;
using static AmsRecords.Irrigation.IrrigationVisualSimulatorDtos;

namespace AmsRecords.Irrigation;

public sealed partial class IrrigationNozzleOptimizer
{
    const int DesignCandidatesPerHead = 36;
    // A small soft preference: improved coverage/uniformity can still justify another supported pressure.
    const double NonPreferredPressurePenalty = .03;
    static double PressurePenalty(IrrigationDesignCandidate candidate) => candidate.PreferredPressureBar is decimal preferred &&
        candidate.Setting.PressureBar != preferred ? NonPreferredPressurePenalty : 0;

    /// <summary>Design from fixed positions without requiring an installed numerical baseline.</summary>
    public DesignResult Design(Guid surfacePubId, string surfaceName, string mode,
        IrrigationAreaPolygon polygon, IReadOnlyList<IrrigationDesignPosition> positions, CancellationToken ct = default, double targetDepthMm = 5)
    {
        if (!double.IsFinite(targetDepthMm) || targetDepthMm is < .1 or > 50) throw new ArgumentException("Target depth must be between 0.1 and 50 mm.");
        if (positions.Count is < 1 or > MaximumHeadCount || positions.Select(p => p.PubId).Distinct().Count() != positions.Count)
            throw new ArgumentException($"Select a surface with 1–{MaximumHeadCount} distinct mapped sprinkler positions.");
        foreach (var position in positions)
        {
            if (position.Candidates.Count is < 1 or > 5000)
                throw new ArgumentException($"{position.Name}: no supported nozzle choices, or too many choices for this search.");
            var first = position.Candidates[0].SimulationHead.Head;
            if (position.Candidates.Any(c => c.Setting.HeadPubId != position.PubId ||
                c.SimulationHead.Head.MapX != first.MapX || c.SimulationHead.Head.MapY != first.MapY ||
                c.SimulationHead.Head.ArcDegrees != c.Setting.ArcDegrees ||
                c.SimulationHead.Performance.RequestedPressureBar != c.Setting.PressureBar ||
                c.SimulationHead.Head.PubId != position.PubId || !c.SimulationHead.Performance.Supported ||
                c.SimulationHead.Performance.FlowM3H is not > 0 || c.SimulationHead.Performance.RadiusM is not > 0 ||
                c.SimulationHead.Head.MapX is not double x || !double.IsFinite(x) ||
                c.SimulationHead.Head.MapY is not double y || !double.IsFinite(y) ||
                c.Setting.ArcDegrees is <= 0 or > 360))
                throw new ArgumentException($"{position.Name}: invalid position or pressure-resolved nozzle performance.");
        }
        var ordered = positions.OrderBy(p => p.Name, StringComparer.Ordinal).ThenBy(p => p.PubId).ToArray();
        var shortlist = ShortlistDesign(polygon, ordered, ct);
        var inputs = shortlist.SelectMany(p => p).Select(AsOptimizationCandidate).ToArray();
        var resolution = ResolveGridResolution(polygon, inputs, 1d, out _);
        var grid = CreateGrid(polygon, inputs, resolution);
        while ((long)grid.Width * grid.Height > 20000 && resolution < 2)
        {
            resolution = Math.Min(2, resolution + .25);
            grid = CreateGrid(polygon, inputs, resolution);
        }
        if ((long)grid.Width * grid.Height > 20000)
            throw new ArgumentException("This design covers too much ground for one search. Select a smaller surface.");
        var mask = polygon.CreateGridMask(grid);
        if (mask.TargetCellCount == 0) throw new ArgumentException("The surface is too small for the coverage grid.");
        var prepared = shortlist.Select(options => options.Select(c =>
        {
            ct.ThrowIfCancellationRequested();
            return new DesignPrepared(c, PrepareCandidate(AsOptimizationCandidate(c), grid).DepthPerMinuteMm);
        }).ToArray()).ToArray();
        var tested = 0;
        var solutions = new List<(int[] Assignment, double[] Runtimes, double[] Depth, double Score)>();
        // Different starts reduce sensitivity to a locally attractive individual sprinkler setting.
        for (var start = 0; start < 5; start++)
        {
            var assignment = prepared.Select(p => start == 4
                ? Enumerable.Range(0, p.Length).MinBy(i => PressurePenalty(p[i].Candidate))
                : Math.Min(start * 3, p.Length - 1)).ToArray();
            var (runtimes, depth) = IrrigationRuntimeBalancer.Fit(assignment.Select((a, h) => prepared[h][a].Depth).ToArray(), mask.Cells, 1, ct);
            var penalty = assignment.Select((choice, h) => PressurePenalty(prepared[h][choice].Candidate)).Sum() / prepared.Length;
            var score = DesignScore(depth, mask) + penalty;
            for (var pass = 0; pass < 6; pass++)
            {
                var changed = false;
                for (var h = 0; h < prepared.Length; h++)
                {
                    ct.ThrowIfCancellationRequested();
                    var old = assignment[h];
                    var best = old;
                    var oldRuntime = runtimes[h]; var bestRuntime = oldRuntime;
                    AddDepth(depth, prepared[h][old].Depth, -oldRuntime);
                    // Explicitly test omission as well as a positive runtime; never force every recorded head to operate.
                    var omittedScore = DesignScore(depth, mask) + penalty;
                    tested++;
                    if (omittedScore + 1e-10 < score) { score = omittedScore; bestRuntime = 0; }
                    foreach (var next in Enumerable.Range(0, prepared[h].Length))
                    {
                        ct.ThrowIfCancellationRequested();
                        var runtime = IrrigationRuntimeBalancer.BestRuntime(prepared[h][next].Depth, depth, mask.Cells);
                        AddDepth(depth, prepared[h][next].Depth, runtime);
                        var trialPenalty = penalty + (PressurePenalty(prepared[h][next].Candidate) - PressurePenalty(prepared[h][old].Candidate)) / prepared.Length;
                        var trial = DesignScore(depth, mask) + trialPenalty;
                        tested++;
                        if (trial + 1e-10 < score) { score = trial; best = next; bestRuntime = runtime; }
                        AddDepth(depth, prepared[h][next].Depth, -runtime);
                    }
                    penalty += (PressurePenalty(prepared[h][best].Candidate) - PressurePenalty(prepared[h][old].Candidate)) / prepared.Length;
                    assignment[h] = best;
                    runtimes[h] = bestRuntime;
                    AddDepth(depth, prepared[h][best].Depth, bestRuntime);
                    changed |= best != old || Math.Abs(bestRuntime - oldRuntime) > 1e-8;
                }
                if (!changed) break;
            }
            IrrigationRuntimeBalancer.Normalize(runtimes, depth, mask.Cells, targetDepthMm);
            solutions.Add((assignment, runtimes, depth, score));
        }
        var options = solutions.OrderBy(s => s.Score).DistinctBy(s => string.Join(',', s.Assignment)).Take(3)
            .Select((s, index) =>
            {
                var selected = s.Assignment.Select((choice, h) => prepared[h][choice].Candidate).ToArray();
                var simulation = _precipitationEngine.Simulate(new(grid,
                    selected.Select((c, h) => c.SimulationHead with { RuntimeSeconds = s.Runtimes[h] * 60 }).ToArray()));
                var metrics = IrrigationDistributionAnalytics.Analyze(simulation, mask, new(targetDepthMm, 0.1));
                var mean = Enumerable.Range(0, mask.CellCount).Where(i => mask.Cells[i]).Average(i => s.Depth[i]);
                var coverage = mean <= Epsilon ? 0 : 100d * Enumerable.Range(0, mask.CellCount).Where(i => mask.Cells[i]).Count(i => s.Depth[i] >= mean * 0.1) / mask.TargetCellCount;
                return new DesignOption(index + 1, s.Score, coverage, metrics.ChristiansenUniformityCoefficient * 100,
                    metrics.DistributionUniformityLowQuarter * 100, 100 - (metrics.TargetApplicationEfficiencyPercent ?? 0),
                    selected.Select((c, h) => c.Setting with { RuntimeMinutes = s.Runtimes[h], Enabled = s.Runtimes[h] > 0,
                        LayoutAction = s.Runtimes[h] > 0 ? c.Setting.LayoutAction : "Omit",
                        Latitude = s.Runtimes[h] > 0 ? c.Setting.Latitude : c.Setting.OriginalLatitude ?? c.Setting.Latitude,
                        Longitude = s.Runtimes[h] > 0 ? c.Setting.Longitude : c.Setting.OriginalLongitude ?? c.Setting.Longitude }).ToArray(), new(grid.GridOriginX, grid.GridOriginY, grid.CellSizeM,
                        grid.Width, grid.Height, s.Depth.Select(d => Math.Max(0, d)).ToArray(), mask.Cells), targetDepthMm);
            }).ToArray();
        return new(surfacePubId, surfaceName, mode, options, ordered.Sum(p => p.Candidates.Count), tested, resolution,
            ["The best tested combinations balance even watering, coverage and water falling outside the surface. Heads may be left unused; proposed positions are shown on the map. The bounded search does not guarantee a global optimum.",
             "Pressure changes use supported catalogue flow and throw data. Radial distribution is an estimate wherever no measured profile is available.",
             "Toro designs prefer 65 PSI when coverage is comparable. A different supported setting may be selected when it improves the combined watering result or 65 PSI is unavailable.",
             "Individual runtimes are balanced while selecting nozzles and arcs, then scaled to the requested mean depth in mm. Runtime estimates are illustrative; no controller schedule is changed or exported.",
             "Coverage means surface cells receiving at least 10% of the surface-average depth. An average target does not mean every point receives that depth.",
             "Recommended pressures are operating pressures at each sprinkler. Confirm that the supply and head regulators can provide them."]);
    }

    static IrrigationOptimizationNozzleCandidate AsOptimizationCandidate(IrrigationDesignCandidate c)
        => new(c.Setting.NozzlePubId, c.Setting.NozzleName, c.Setting.ModelName, c.Setting.PressureBar, 1, c.SimulationHead);

    public static DesignOption BalanceCurrent(IrrigationAreaPolygon polygon, IrrigationSimulatorGridDto template,
        IReadOnlyList<double[]> rates, IReadOnlyList<Setting> settings, double target, CancellationToken ct)
    {
        if (rates.Count != settings.Count) throw new ArgumentException("Every current sprinkler needs its own contribution grid.");
        var grid = new IrrigationSimulationGrid(template.OriginX, template.OriginY, template.Width, template.Height, template.CellSizeM);
        var mask = polygon.CreateGridMask(grid);
        var (runtimes, depth) = IrrigationRuntimeBalancer.Fit(rates, mask.Cells, target, ct);
        var cells = new double[grid.Height, grid.Width];
        for (var i = 0; i < depth.Length; i++) cells[i / grid.Width, i % grid.Width] = depth[i];
        var simulation = new IrrigationSimulationResult(grid.GridOriginX, grid.GridOriginY, grid.CellSizeM, grid.Width, grid.Height,
            target, depth.Min(), depth.Max(), depth.Sum() * grid.CellSizeM * grid.CellSizeM / 1000, cells, [], [], IrrigationSimulationConfidence.ManufacturerDerived);
        var metrics = IrrigationDistributionAnalytics.Analyze(simulation, mask, new(target, .1));
        var coverage = 100d * Enumerable.Range(0, depth.Length).Count(i => mask.Cells[i] && depth[i] >= .1 * target) / mask.TargetCellCount;
        return new(0, DesignScore(depth, mask), coverage, metrics.ChristiansenUniformityCoefficient * 100,
            metrics.DistributionUniformityLowQuarter * 100, 100 - (metrics.TargetApplicationEfficiencyPercent ?? 0),
            settings.Select((s, i) => s with { RuntimeMinutes = runtimes[i] }).ToArray(),
            template with { ApplicationDepthMm = depth, TargetMask = mask.Cells }, target);
    }

    static IReadOnlyList<IrrigationDesignCandidate[]> ShortlistDesign(IrrigationAreaPolygon polygon,
        IReadOnlyList<IrrigationDesignPosition> positions, CancellationToken ct)
    {
        var bounds = polygon.Metrics.BoundingBox;
        var cell = Math.Max(1, Math.Sqrt((bounds.MaxX - bounds.MinX) * (bounds.MaxY - bounds.MinY) / 1000));
        var grid = IrrigationSimulationGrid.FromBounds(bounds.MinX, bounds.MinY, bounds.MaxX, bounds.MaxY, cell);
        var mask = polygon.CreateGridMask(grid);
        var samples = Enumerable.Range(0, mask.CellCount).Where(i => mask.Cells[i]).Select(i => new IrrigationPlanarPoint(
            grid.GridOriginX + (i % grid.Width + 0.5) * cell, grid.GridOriginY + (i / grid.Width + 0.5) * cell)).ToArray();
        double Distance(IrrigationPlanarPoint p, IrrigationDesignPosition h)
            => Math.Pow(p.X - h.Candidates[0].SimulationHead.Head.MapX!.Value, 2) + Math.Pow(p.Y - h.Candidates[0].SimulationHead.Head.MapY!.Value, 2);
        var owners = samples.Select(p => positions.MinBy(h => Distance(p, h))!.PubId).ToArray();
        return positions.Select(position =>
        {
            var owned = owners.Count(id => id == position.PubId);
            var ranked = position.Candidates.Select(c =>
            {
                ct.ThrowIfCancellationRequested();
                var inside = 0; var covered = 0;
                for (var i = 0; i < samples.Length; i++)
                {
                    var dx = samples[i].X - c.SimulationHead.Head.MapX!.Value;
                    var dy = samples[i].Y - c.SimulationHead.Head.MapY!.Value;
                    if (dx * dx + dy * dy > c.Setting.RadiusM * c.Setting.RadiusM) continue;
                    var angle = Math.Atan2(dy, dx) * 180 / Math.PI;
                    var delta = Math.Abs(((angle - (double)c.SimulationHead.Head.OrientationDegrees!.Value + 540) % 360) - 180);
                    if (delta > (double)c.Setting.ArcDegrees / 2) continue;
                    inside++; if (owners[i] == position.PubId) covered++;
                }
                var footprint = Math.PI * c.Setting.RadiusM * c.Setting.RadiusM * (double)c.Setting.ArcDegrees / 360;
                var outside = Math.Clamp(1 - inside * cell * cell / footprint, 0, 1);
                return (Candidate: c, Score: (owned == 0 ? 0 : 3d * (1d - (double)covered / owned)) + outside);
            }).OrderBy(x => x.Score).ThenBy(x => PressurePenalty(x.Candidate)).ThenBy(x => x.Candidate.Setting.PressureBar).ToArray();
            // Retain arc diversity as well as strong individual coverage; the combined search chooses among them.
            return ranked.Take(24)
                .Concat(ranked.Where(x => x.Candidate.PreferredPressureBar == x.Candidate.Setting.PressureBar)
                    .GroupBy(x => x.Candidate.Setting.ArcDegrees).Select(g => g.First()))
                .Concat(ranked.GroupBy(x => x.Candidate.Setting.ArcDegrees).Select(g => g.First()))
                .DistinctBy(x => x.Candidate).Take(DesignCandidatesPerHead).Select(x => x.Candidate).ToArray();
        }).ToArray();
    }

    static void AddDepth(double[] target, double[] source, double factor)
    {
        for (var i = 0; i < target.Length; i++) target[i] += source[i] * factor;
    }

    static double DesignScore(double[] depth, IrrigationAreaGridMask mask)
    {
        double sum = 0, squares = 0, total = 0;
        for (var i = 0; i < depth.Length; i++)
        {
            var value = Math.Max(0, depth[i]); total += value;
            if (mask.Cells[i]) { sum += value; squares += value * value; }
        }
        if (sum <= Epsilon) return 1e9;
        var mean = sum / mask.TargetCellCount;
        var cv = Math.Sqrt(Math.Max(0, squares / mask.TargetCellCount - mean * mean)) / mean;
        var dry = (double)Enumerable.Range(0, mask.CellCount).Where(i => mask.Cells[i]).Count(i => depth[i] < mean * 0.1) / mask.TargetCellCount;
        return 4 * dry + 2 * cv + (total <= Epsilon ? 1 : (total - sum) / total);
    }

    sealed record DesignPrepared(IrrigationDesignCandidate Candidate, double[] Depth);
}

