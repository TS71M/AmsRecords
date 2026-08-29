using static AmsRecords.Irrigation.IrrigationNozzleOptimizerDtos;
using static AmsRecords.Irrigation.IrrigationVisualSimulatorDtos;

namespace AmsRecords.Irrigation;

/// <summary>One pressure-resolved, model-compatible nozzle choice for one fixed irrigation head.</summary>
public sealed record IrrigationOptimizationNozzleCandidate(
    Guid NozzlePubId,
    string NozzleName,
    string NozzleFamily,
    decimal PressureBar,
    double PressureSuitability,
    IrrigationSimulationHead SimulationHead);

/// <summary>
/// One head in an optimization problem. Pressure, arc, orientation, and position remain fixed in the MVP;
/// the optimizer may only select a candidate nozzle and runtime.
/// </summary>
public sealed record IrrigationOptimizationHead(
    Guid HeadPubId,
    string HeadName,
    Guid? InstalledNozzlePubId,
    string InstalledNozzleName,
    double CurrentRuntimeMinutes,
    bool KeepExistingNozzle,
    IReadOnlyList<IrrigationOptimizationNozzleCandidate> Candidates);

/// <summary>A fully resolved, persistence-independent optimization problem for one canonical irrigation area.</summary>
public sealed record IrrigationNozzleOptimizationProblem(
    Guid AreaPubId,
    string AreaName,
    IrrigationAreaPolygon TargetArea,
    IReadOnlyList<IrrigationOptimizationHead> Heads);

/// <summary>
/// Deterministic bounded nozzle/runtime optimizer. It uses diverse candidate filtering, a bounded beam search,
/// and deterministic coordinate descent. It never mutates installed infrastructure or persistent scenarios.
/// </summary>
public sealed class IrrigationNozzleOptimizer
{
    const int MaximumHeadCount = 24;
    const int MaximumNozzleCandidatesPerHead = 6;
    const int MaximumVisitedAssignments = 1_500;
    const int BeamWidth = 48;
    const int MaximumRuntimeOptimizedAssignments = 64;
    const int MaximumCoordinatePasses = 4;
    const int MaximumOptimizationCellCount = 100_000;
    const double MaximumOptimizationGridResolutionM = 2d;
    const double RuntimeIncrementMinutes = 0.25d;
    const double Epsilon = 1e-9;

    readonly IrrigationPrecipitationEngine _precipitationEngine = new();

    public IrrigationNozzleOptimizationResultDto Optimize(
        IrrigationNozzleOptimizationProblem problem,
        IrrigationNozzleOptimizationRequestDto request,
        CancellationToken ct = default)
    {
        Validate(problem, request);
        var objective = IrrigationOptimizationObjectiveCodes.Normalize(request.Objective);
        var warnings = new List<string>();
        var resultReasonCodes = new List<string>();
        var relevantCandidatesByHead = SelectRelevantCandidateInputs(problem, request);
        var relevantCandidateInputs = relevantCandidatesByHead.Values.SelectMany(x => x).ToList();
        var effectiveGridResolution = ResolveGridResolution(
            problem.TargetArea,
            relevantCandidateInputs,
            request.GridResolutionM,
            out var coarsened);
        if (coarsened)
        {
            resultReasonCodes.Add(IrrigationOptimizationReasonCodes.GridResolutionCoarsened);
            warnings.Add($"The optimization grid was deterministically coarsened to {effectiveGridResolution:0.##} m to keep the bounded search within {MaximumOptimizationCellCount:N0} cells.");
        }

        var grid = CreateGrid(problem.TargetArea, relevantCandidateInputs, effectiveGridResolution);
        var mask = problem.TargetArea.CreateGridMask(grid);
        if (mask.TargetCellCount == 0)
            throw new ArgumentException("The selected optimization grid does not represent any target-area cell centres.", nameof(request));

        var preparedHeads = PrepareHeads(problem, request, relevantCandidatesByHead, grid, warnings, ct);
        var availableFamilies = problem.Heads
            .SelectMany(x => x.Candidates)
            .Select(x => x.NozzleFamily.Trim())
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var baselineAssignment = preparedHeads.Select(x => x.InstalledCandidate).ToArray();
        var baselineRuntimes = preparedHeads.Select(x => ClampRuntime(x.Source.CurrentRuntimeMinutes, request.MaximumRuntimeMinutes)).ToArray();
        var evaluationCount = 0;
        var baseline = Evaluate(
            baselineAssignment,
            baselineRuntimes,
            preparedHeads,
            mask,
            request,
            objective,
            ref evaluationCount);

        if (preparedHeads.Any(x => x.EligibleCandidates.Count == 0))
        {
            resultReasonCodes.Add(IrrigationOptimizationReasonCodes.NoFeasibleNozzleCandidates);
            return EmptyResult(
                problem,
                request,
                objective,
                preparedHeads,
                baseline,
                availableFamilies,
                evaluationCount,
                resultReasonCodes,
                warnings);
        }

        var rootAssignment = preparedHeads
            .Select(x => x.EligibleCandidates.FirstOrDefault(candidate =>
                             candidate.Source.NozzlePubId == x.Source.InstalledNozzlePubId)
                         ?? x.EligibleCandidates[0])
            .Cast<PreparedCandidate?>()
            .ToArray();
        if (CountNozzleChanges(rootAssignment, preparedHeads) > request.MaximumNozzleChanges)
        {
            resultReasonCodes.Add(IrrigationOptimizationReasonCodes.NoFeasibleNozzleCandidates);
            warnings.Add("The allowed nozzle-family, pressure-suitability, and locked-head constraints require more nozzle changes than the configured maximum.");
            return EmptyResult(
                problem,
                request,
                objective,
                preparedHeads,
                baseline,
                availableFamilies,
                evaluationCount,
                resultReasonCodes,
                warnings);
        }

        var visited = new Dictionary<string, ScenarioEvaluation>(StringComparer.Ordinal);
        var root = EvaluateProxy(rootAssignment, preparedHeads, mask, request, objective, ref evaluationCount);
        visited.Add(AssignmentSignature(rootAssignment), root);
        var frontier = new List<ScenarioEvaluation> { root };
        var searchLimitReached = false;
        var iterationLimit = Math.Min(6, request.MaximumNozzleChanges + 2);

        for (var iteration = 0; iteration < iterationLimit && frontier.Count > 0; iteration++)
        {
            ct.ThrowIfCancellationRequested();
            var expanded = new List<ScenarioEvaluation>();
            foreach (var state in frontier)
            {
                for (var headIndex = 0; headIndex < preparedHeads.Count; headIndex++)
                {
                    foreach (var candidate in preparedHeads[headIndex].EligibleCandidates)
                    {
                        if (ReferenceEquals(candidate, state.Assignment[headIndex]))
                            continue;

                        var assignment = (PreparedCandidate?[])state.Assignment.Clone();
                        assignment[headIndex] = candidate;
                        if (CountNozzleChanges(assignment, preparedHeads) > request.MaximumNozzleChanges)
                            continue;

                        var signature = AssignmentSignature(assignment);
                        if (visited.ContainsKey(signature))
                            continue;
                        if (visited.Count >= MaximumVisitedAssignments)
                        {
                            searchLimitReached = true;
                            break;
                        }

                        var evaluation = EvaluateProxy(
                            assignment,
                            preparedHeads,
                            mask,
                            request,
                            objective,
                            ref evaluationCount);
                        visited.Add(signature, evaluation);
                        expanded.Add(evaluation);
                    }

                    if (searchLimitReached)
                        break;
                }

                if (searchLimitReached)
                    break;
            }

            frontier = expanded
                .OrderBy(x => x, ScenarioEvaluationComparer.Instance)
                .Take(BeamWidth)
                .ToList();
        }

        if (searchLimitReached)
            resultReasonCodes.Add(IrrigationOptimizationReasonCodes.SearchLimitReached);

        var optimized = new List<ScenarioEvaluation>();
        var anyFeasibleOptimized = false;
        foreach (var assignment in visited.Values
                     .OrderBy(x => x, ScenarioEvaluationComparer.Instance)
                     .Take(MaximumRuntimeOptimizedAssignments))
        {
            ct.ThrowIfCancellationRequested();
            var result = OptimizeRuntimes(
                assignment.Assignment,
                preparedHeads,
                mask,
                request,
                objective,
                ref evaluationCount,
                ct);
            anyFeasibleOptimized |= result.Feasible;
            if (result.Feasible && IsBetter(result, baseline))
                optimized.Add(result);
        }

        var ranked = optimized
            .GroupBy(ScenarioSignature)
            .Select(x => x.OrderBy(y => y, ScenarioEvaluationComparer.Instance).First())
            .OrderBy(x => x, ScenarioEvaluationComparer.Instance)
            .Take(request.RequestedOptionCount)
            .ToList();

        if (ranked.Count == 0)
        {
            resultReasonCodes.Add(anyFeasibleOptimized
                ? IrrigationOptimizationReasonCodes.NoBetterScenarioFound
                : IrrigationOptimizationReasonCodes.TargetDepthUnreachable);
        }

        var baselineDto = BuildBaseline(preparedHeads, baseline);
        var options = ranked
            .Select((x, index) => BuildOption(
                problem,
                request,
                effectiveGridResolution,
                preparedHeads,
                mask,
                baseline,
                x,
                index + 1))
            .ToList();

        return new IrrigationNozzleOptimizationResultDto(
            problem.AreaPubId,
            problem.AreaName,
            objective,
            baselineDto,
            options,
            availableFamilies,
            evaluationCount,
            searchLimitReached,
            resultReasonCodes.Distinct(StringComparer.Ordinal).ToList(),
            warnings.Distinct(StringComparer.Ordinal).ToList());
    }

    List<PreparedHead> PrepareHeads(
        IrrigationNozzleOptimizationProblem problem,
        IrrigationNozzleOptimizationRequestDto request,
        IReadOnlyDictionary<Guid, IReadOnlyList<IrrigationOptimizationNozzleCandidate>> relevantCandidatesByHead,
        IrrigationSimulationGrid grid,
        ICollection<string> warnings,
        CancellationToken ct)
    {
        var allowedFamilies = (request.AllowedNozzleFamilies ?? [])
            .Select(x => x?.Trim() ?? "")
            .Where(x => x.Length > 0)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var result = new List<PreparedHead>(problem.Heads.Count);

        foreach (var head in problem.Heads.OrderBy(x => x.HeadName, StringComparer.Ordinal).ThenBy(x => x.HeadPubId))
        {
            ct.ThrowIfCancellationRequested();
            var all = relevantCandidatesByHead[head.HeadPubId]
                .OrderBy(x => x.NozzlePubId)
                .Select(candidate => PrepareCandidate(candidate, grid))
                .ToList();
            var installed = all.FirstOrDefault(x => x.Source.NozzlePubId == head.InstalledNozzlePubId);
            var eligible = all.Where(x =>
                    x.Source.PressureSuitability + Epsilon >= request.MinimumPressureSuitability &&
                    (allowedFamilies.Count == 0 || allowedFamilies.Contains(x.Source.NozzleFamily)) &&
                    (!head.KeepExistingNozzle || x.Source.NozzlePubId == head.InstalledNozzlePubId))
                .ToList();
            eligible = LimitCandidates(eligible, head.InstalledNozzlePubId);

            if (installed is null)
                warnings.Add($"{head.HeadName}: the installed nozzle lacks usable performance at the fixed operating pressure and is omitted from the numerical baseline.");
            if (eligible.Count == 0)
                warnings.Add($"{head.HeadName}: no nozzle satisfies the family, pressure-suitability, and keep-existing constraints.");

            result.Add(new PreparedHead(head, installed, eligible));
        }

        return result;
    }

    static IReadOnlyDictionary<Guid, IReadOnlyList<IrrigationOptimizationNozzleCandidate>> SelectRelevantCandidateInputs(
        IrrigationNozzleOptimizationProblem problem,
        IrrigationNozzleOptimizationRequestDto request)
    {
        var allowedFamilies = (request.AllowedNozzleFamilies ?? [])
            .Select(x => x?.Trim() ?? "")
            .Where(x => x.Length > 0)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var result = new Dictionary<Guid, IReadOnlyList<IrrigationOptimizationNozzleCandidate>>();
        foreach (var head in problem.Heads)
        {
            var eligible = head.Candidates.Where(x =>
                    x.PressureSuitability + Epsilon >= request.MinimumPressureSuitability &&
                    (allowedFamilies.Count == 0 || allowedFamilies.Contains(x.NozzleFamily)) &&
                    (!head.KeepExistingNozzle || x.NozzlePubId == head.InstalledNozzlePubId))
                .ToList();
            eligible = LimitCandidateInputs(eligible, head.InstalledNozzlePubId);
            var installed = head.Candidates.FirstOrDefault(x => x.NozzlePubId == head.InstalledNozzlePubId);
            if (installed is not null && eligible.All(x => x.NozzlePubId != installed.NozzlePubId))
                eligible.Add(installed);
            result.Add(head.HeadPubId, eligible);
        }
        return result;
    }

    static List<IrrigationOptimizationNozzleCandidate> LimitCandidateInputs(
        IReadOnlyList<IrrigationOptimizationNozzleCandidate> source,
        Guid? installedNozzlePubId)
    {
        var ordered = source
            .OrderBy(x => x.SimulationHead.Performance.FlowM3H)
            .ThenBy(x => x.NozzleName, StringComparer.Ordinal)
            .ThenBy(x => x.NozzlePubId)
            .ToList();
        if (ordered.Count <= MaximumNozzleCandidatesPerHead)
            return ordered;

        var selected = new Dictionary<Guid, IrrigationOptimizationNozzleCandidate>();
        void Add(IrrigationOptimizationNozzleCandidate candidate) => selected.TryAdd(candidate.NozzlePubId, candidate);
        var installed = ordered.FirstOrDefault(x => x.NozzlePubId == installedNozzlePubId);
        if (installed is not null) Add(installed);
        Add(ordered[0]);
        Add(ordered[^1]);
        for (var slot = 1; selected.Count < MaximumNozzleCandidatesPerHead && slot < ordered.Count - 1; slot++)
        {
            var index = (int)Math.Round(
                slot * (ordered.Count - 1d) / (MaximumNozzleCandidatesPerHead - 1d),
                MidpointRounding.AwayFromZero);
            Add(ordered[index]);
        }
        foreach (var candidate in ordered)
        {
            if (selected.Count >= MaximumNozzleCandidatesPerHead) break;
            Add(candidate);
        }
        return selected.Values
            .OrderBy(x => x.SimulationHead.Performance.FlowM3H)
            .ThenBy(x => x.NozzleName, StringComparer.Ordinal)
            .ThenBy(x => x.NozzlePubId)
            .ToList();
    }

    PreparedCandidate PrepareCandidate(
        IrrigationOptimizationNozzleCandidate candidate,
        IrrigationSimulationGrid grid)
    {
        var unitHead = candidate.SimulationHead with { RuntimeSeconds = 60d, IsOperating = true };
        var simulation = _precipitationEngine.Simulate(new IrrigationSimulationRequest(grid, [unitHead]));
        var depths = new double[checked(grid.Width * grid.Height)];
        for (var row = 0; row < grid.Height; row++)
        for (var column = 0; column < grid.Width; column++)
            depths[row * grid.Width + column] = simulation.Cells[row, column];
        return new PreparedCandidate(candidate, depths, (double)candidate.SimulationHead.Performance.FlowM3H!.Value);
    }

    static List<PreparedCandidate> LimitCandidates(
        IReadOnlyList<PreparedCandidate> source,
        Guid? installedNozzlePubId)
    {
        var ordered = source
            .OrderBy(x => x.FlowM3H)
            .ThenBy(x => x.Source.NozzleName, StringComparer.Ordinal)
            .ThenBy(x => x.Source.NozzlePubId)
            .ToList();
        if (ordered.Count <= MaximumNozzleCandidatesPerHead)
            return ordered;

        var selected = new Dictionary<Guid, PreparedCandidate>();
        void Add(PreparedCandidate candidate) => selected.TryAdd(candidate.Source.NozzlePubId, candidate);
        if (installedNozzlePubId.HasValue)
        {
            var installed = ordered.FirstOrDefault(x => x.Source.NozzlePubId == installedNozzlePubId.Value);
            if (installed is not null)
                Add(installed);
        }
        Add(ordered[0]);
        Add(ordered[^1]);
        for (var slot = 1; selected.Count < MaximumNozzleCandidatesPerHead && slot < ordered.Count - 1; slot++)
        {
            var index = (int)Math.Round(
                slot * (ordered.Count - 1d) / (MaximumNozzleCandidatesPerHead - 1d),
                MidpointRounding.AwayFromZero);
            Add(ordered[index]);
        }
        foreach (var candidate in ordered)
        {
            if (selected.Count >= MaximumNozzleCandidatesPerHead)
                break;
            Add(candidate);
        }
        return selected.Values
            .OrderBy(x => x.FlowM3H)
            .ThenBy(x => x.Source.NozzleName, StringComparer.Ordinal)
            .ThenBy(x => x.Source.NozzlePubId)
            .ToList();
    }

    static ScenarioEvaluation EvaluateProxy(
        PreparedCandidate?[] assignment,
        IReadOnlyList<PreparedHead> heads,
        IrrigationAreaGridMask mask,
        IrrigationNozzleOptimizationRequestDto request,
        string objective,
        ref int evaluationCount)
    {
        var runtimes = heads.Select(x => ClampRuntime(x.Source.CurrentRuntimeMinutes, request.MaximumRuntimeMinutes)).ToArray();
        runtimes = ScaleRuntimesToTarget(assignment, runtimes, heads, mask, request);
        return Evaluate(assignment, runtimes, heads, mask, request, objective, ref evaluationCount);
    }

    static ScenarioEvaluation OptimizeRuntimes(
        PreparedCandidate?[] assignment,
        IReadOnlyList<PreparedHead> heads,
        IrrigationAreaGridMask mask,
        IrrigationNozzleOptimizationRequestDto request,
        string objective,
        ref int evaluationCount,
        CancellationToken ct)
    {
        var current = heads.Select(x => ClampRuntime(x.Source.CurrentRuntimeMinutes, request.MaximumRuntimeMinutes)).ToArray();
        var seeds = new List<double[]>
        {
            current,
            ScaleRuntimesToTarget(assignment, current, heads, mask, request),
            ScaleRuntimesToTarget(
                assignment,
                Enumerable.Repeat(Math.Min(1d, request.MaximumRuntimeMinutes), heads.Count).ToArray(),
                heads,
                mask,
                request)
        };
        ScenarioEvaluation? best = null;

        foreach (var seed in seeds)
        {
            var candidate = Evaluate(assignment, seed, heads, mask, request, objective, ref evaluationCount);
            for (var pass = 0; pass < MaximumCoordinatePasses; pass++)
            {
                ct.ThrowIfCancellationRequested();
                var improved = false;
                for (var headIndex = 0; headIndex < heads.Count; headIndex++)
                {
                    var contributionMean = MeanTargetContributionPerMinute(assignment[headIndex], mask);
                    var values = new HashSet<double>
                    {
                        candidate.Runtimes[headIndex],
                        0d,
                        request.MaximumRuntimeMinutes,
                        RoundRuntime(candidate.Runtimes[headIndex] - RuntimeIncrementMinutes, request.MaximumRuntimeMinutes),
                        RoundRuntime(candidate.Runtimes[headIndex] + RuntimeIncrementMinutes, request.MaximumRuntimeMinutes)
                    };
                    if (contributionMean > Epsilon)
                    {
                        values.Add(RoundRuntime(
                            candidate.Runtimes[headIndex] +
                            (request.TargetDepthMm - candidate.Fast.MeanDepthMm) / contributionMean,
                            request.MaximumRuntimeMinutes));
                    }

                    foreach (var value in values.OrderBy(x => x))
                    {
                        if (Math.Abs(value - candidate.Runtimes[headIndex]) <= Epsilon)
                            continue;
                        var runtimes = (double[])candidate.Runtimes.Clone();
                        runtimes[headIndex] = value;
                        var alternative = Evaluate(
                            assignment,
                            runtimes,
                            heads,
                            mask,
                            request,
                            objective,
                            ref evaluationCount);
                        if (ScenarioEvaluationComparer.Instance.Compare(alternative, candidate) < 0)
                        {
                            candidate = alternative;
                            improved = true;
                        }
                    }
                }

                var scaledRuntimes = ScaleRuntimesToTarget(
                    assignment,
                    candidate.Runtimes,
                    heads,
                    mask,
                    request);
                var scaled = Evaluate(
                    assignment,
                    scaledRuntimes,
                    heads,
                    mask,
                    request,
                    objective,
                    ref evaluationCount);
                if (ScenarioEvaluationComparer.Instance.Compare(scaled, candidate) < 0)
                {
                    candidate = scaled;
                    improved = true;
                }
                if (!improved)
                    break;
            }

            if (best is null || ScenarioEvaluationComparer.Instance.Compare(candidate, best) < 0)
                best = candidate;
        }

        return best!;
    }

    static double[] ScaleRuntimesToTarget(
        PreparedCandidate?[] assignment,
        IReadOnlyList<double> source,
        IReadOnlyList<PreparedHead> heads,
        IrrigationAreaGridMask mask,
        IrrigationNozzleOptimizationRequestDto request)
    {
        var runtimes = source.Select(x => ClampRuntime(x, request.MaximumRuntimeMinutes)).ToArray();
        var mean = CalculateMeanDepth(assignment, runtimes, mask);
        if (mean <= Epsilon)
        {
            for (var index = 0; index < runtimes.Length; index++)
                runtimes[index] = assignment[index] is null ? 0d : Math.Min(1d, request.MaximumRuntimeMinutes);
            mean = CalculateMeanDepth(assignment, runtimes, mask);
        }
        if (mean <= Epsilon)
            return runtimes;

        var scale = request.TargetDepthMm / mean;
        for (var index = 0; index < runtimes.Length; index++)
            runtimes[index] = RoundRuntime(runtimes[index] * scale, request.MaximumRuntimeMinutes);
        return runtimes;
    }

    static ScenarioEvaluation Evaluate(
        PreparedCandidate?[] assignment,
        IReadOnlyList<double> runtimes,
        IReadOnlyList<PreparedHead> heads,
        IrrigationAreaGridMask mask,
        IrrigationNozzleOptimizationRequestDto request,
        string objective,
        ref int evaluationCount)
    {
        evaluationCount++;
        var depth = CombineDepths(assignment, runtimes, mask.CellCount);
        var metrics = CalculateFastMetrics(depth, assignment, runtimes, heads, mask, request);
        var feasible = metrics.MeanDepthMm + Epsilon >= request.TargetDepthMm * (1d - request.TargetToleranceFraction) &&
                       metrics.MeanDepthMm - Epsilon <= request.TargetDepthMm * (1d + request.TargetToleranceFraction) &&
                       (!request.MaximumSimultaneousFlowM3H.HasValue ||
                        metrics.FlowM3H <= request.MaximumSimultaneousFlowM3H.Value + Epsilon);
        var score = ObjectiveScore(metrics, objective, request, mask);
        var feasibilityPenalty = FeasibilityPenalty(metrics, request);
        return new ScenarioEvaluation(
            (PreparedCandidate?[])assignment.Clone(),
            runtimes.ToArray(),
            depth,
            metrics,
            score,
            feasibilityPenalty,
            feasible);
    }

    static FastMetrics CalculateFastMetrics(
        IReadOnlyList<double> depth,
        PreparedCandidate?[] assignment,
        IReadOnlyList<double> runtimes,
        IReadOnlyList<PreparedHead> heads,
        IrrigationAreaGridMask mask,
        IrrigationNozzleOptimizationRequestDto request)
    {
        var targetValues = new double[mask.TargetCellCount];
        var targetIndex = 0;
        var outsideAreaVolumeM3 = 0d;
        var cellAreaM2 = mask.Grid.CellAreaM2;
        for (var index = 0; index < depth.Count; index++)
        {
            if (mask.Cells[index])
                targetValues[targetIndex++] = depth[index];
            else
                outsideAreaVolumeM3 += depth[index] / 1_000d * cellAreaM2;
        }

        var mean = targetValues.Average();
        Array.Sort(targetValues);
        var lowQuarterCount = Math.Max(1, (int)Math.Ceiling(targetValues.Length * 0.25d));
        var lowQuarterMean = targetValues.Take(lowQuarterCount).Average();
        var dUlq = mean > Epsilon ? lowQuarterMean / mean : (double?)null;
        var cu = mean > Epsilon
            ? 1d - targetValues.Sum(x => Math.Abs(x - mean)) / (targetValues.Length * mean)
            : (double?)null;
        var lower = request.TargetDepthMm * (1d - request.TargetToleranceFraction);
        var upper = request.TargetDepthMm * (1d + request.TargetToleranceFraction);
        var outsideCount = targetValues.Count(x => x < lower || x > upper);
        var flow = 0d;
        var totalVolume = 0d;
        for (var index = 0; index < assignment.Length; index++)
        {
            if (assignment[index] is null || runtimes[index] <= Epsilon)
                continue;
            flow += assignment[index]!.FlowM3H;
            totalVolume += assignment[index]!.FlowM3H * runtimes[index] / 60d;
        }

        return new FastMetrics(
            flow,
            dUlq,
            cu,
            mean,
            Math.Abs(mean - request.TargetDepthMm),
            outsideCount * 100d / targetValues.Length,
            outsideAreaVolumeM3,
            totalVolume,
            CountNozzleChanges(assignment, heads));
    }

    static double ObjectiveScore(
        FastMetrics metrics,
        string objective,
        IrrigationNozzleOptimizationRequestDto request,
        IrrigationAreaGridMask mask)
    {
        var idealVolumeM3 = Math.Max(Epsilon, mask.ApproximateTargetAreaM2 * request.TargetDepthMm / 1_000d);
        return objective switch
        {
            IrrigationOptimizationObjectiveCodes.MaximumDistributionUniformityLowQuarter =>
                1d - (metrics.DUlq ?? -1d),
            IrrigationOptimizationObjectiveCodes.MaximumChristiansenUniformity =>
                1d - (metrics.CU ?? -1d),
            IrrigationOptimizationObjectiveCodes.MinimumTargetDeviation =>
                metrics.TargetDeviationMm / request.TargetDepthMm,
            IrrigationOptimizationObjectiveCodes.MinimumOutsideTargetApplication =>
                metrics.OutsideTargetPercent / 100d,
            IrrigationOptimizationObjectiveCodes.MinimumTotalWaterVolume =>
                metrics.TotalWaterVolumeM3 / idealVolumeM3,
            IrrigationOptimizationObjectiveCodes.MinimumNozzleChanges =>
                metrics.NozzleChangeCount,
            IrrigationOptimizationObjectiveCodes.CompositeWeighted =>
                CompositeScore(metrics, request, idealVolumeM3),
            _ => throw new InvalidOperationException($"Unsupported optimization objective '{objective}'.")
        };
    }

    static double CompositeScore(
        FastMetrics metrics,
        IrrigationNozzleOptimizationRequestDto request,
        double idealVolumeM3)
    {
        var weights = request.CompositeWeights ?? new IrrigationOptimizationCompositeWeightsDto();
        var weighted =
            weights.DUlq * (1d - (metrics.DUlq ?? 0d)) +
            weights.CU * (1d - (metrics.CU ?? 0d)) +
            weights.TargetDeviation * metrics.TargetDeviationMm / request.TargetDepthMm +
            weights.OutsideTargetApplication * metrics.OutsideTargetPercent / 100d +
            weights.TotalWaterVolume * metrics.TotalWaterVolumeM3 / idealVolumeM3 +
            weights.NozzleChanges * metrics.NozzleChangeCount / Math.Max(1d, request.MaximumNozzleChanges);
        var totalWeight = weights.DUlq + weights.CU + weights.TargetDeviation +
                          weights.OutsideTargetApplication + weights.TotalWaterVolume + weights.NozzleChanges;
        return weighted / totalWeight;
    }

    static double FeasibilityPenalty(FastMetrics metrics, IrrigationNozzleOptimizationRequestDto request)
    {
        var lower = request.TargetDepthMm * (1d - request.TargetToleranceFraction);
        var upper = request.TargetDepthMm * (1d + request.TargetToleranceFraction);
        var depthMiss = metrics.MeanDepthMm < lower
            ? lower - metrics.MeanDepthMm
            : metrics.MeanDepthMm > upper
                ? metrics.MeanDepthMm - upper
                : 0d;
        var flowMiss = request.MaximumSimultaneousFlowM3H.HasValue &&
                       metrics.FlowM3H > request.MaximumSimultaneousFlowM3H.Value
            ? (metrics.FlowM3H - request.MaximumSimultaneousFlowM3H.Value) /
              Math.Max(Epsilon, request.MaximumSimultaneousFlowM3H.Value)
            : 0d;
        return depthMiss / request.TargetDepthMm * 1_000d + flowMiss * 1_000d;
    }

    static bool IsBetter(ScenarioEvaluation candidate, ScenarioEvaluation baseline)
    {
        if (candidate.Feasible != baseline.Feasible)
            return candidate.Feasible;
        return ScenarioEvaluationComparer.Instance.Compare(candidate, baseline) < 0 &&
               !string.Equals(ScenarioSignature(candidate), ScenarioSignature(baseline), StringComparison.Ordinal);
    }

    static IrrigationOptimizationBaselineDto BuildBaseline(
        IReadOnlyList<PreparedHead> heads,
        ScenarioEvaluation baseline)
        => new(ToMetricsDto(baseline.Fast), BuildHeadSettings(heads, baseline));

    static IrrigationOptimizationOptionDto BuildOption(
        IrrigationNozzleOptimizationProblem problem,
        IrrigationNozzleOptimizationRequestDto request,
        double effectiveGridResolution,
        IReadOnlyList<PreparedHead> heads,
        IrrigationAreaGridMask mask,
        ScenarioEvaluation baseline,
        ScenarioEvaluation option,
        int rank)
    {
        var canonical = CanonicalMetrics(option, mask, request);
        var metrics = new IrrigationOptimizationMetricsDto(
            option.Fast.FlowM3H,
            canonical.DistributionUniformityLowQuarter,
            canonical.ChristiansenUniformityCoefficient,
            canonical.MeanApplicationDepthMm,
            Math.Abs(canonical.MeanTargetDeviationMm),
            canonical.OutsideTargetPercent,
            canonical.OutsideTargetAreaApplicationVolumeM3 ?? 0d,
            option.Fast.TotalWaterVolumeM3,
            option.Fast.NozzleChangeCount);
        var settings = BuildHeadSettings(heads, option);
        var changedHeads = new List<IrrigationOptimizationChangedHeadDto>();
        for (var index = 0; index < heads.Count; index++)
        {
            var selected = option.Assignment[index];
            if (selected is null)
                continue;
            var nozzleChanged = selected.Source.NozzlePubId != heads[index].Source.InstalledNozzlePubId;
            var runtimeChanged = Math.Abs(option.Runtimes[index] - heads[index].Source.CurrentRuntimeMinutes) > Epsilon;
            if (!nozzleChanged && !runtimeChanged)
                continue;
            changedHeads.Add(new IrrigationOptimizationChangedHeadDto(
                heads[index].Source.HeadPubId,
                heads[index].Source.HeadName,
                heads[index].Source.InstalledNozzlePubId,
                heads[index].Source.InstalledNozzleName,
                selected.Source.NozzlePubId,
                selected.Source.NozzleName,
                heads[index].Source.CurrentRuntimeMinutes,
                option.Runtimes[index]));
        }

        var reasons = BuildReasonCodes(baseline.Fast, option.Fast, option.Runtimes, request);
        var simulation = new IrrigationSimulatorRequestDto(
            problem.AreaPubId,
            request.DefaultCurrentRuntimeMinutes,
            request.TargetDepthMm,
            effectiveGridResolution,
            true,
            settings);
        return new IrrigationOptimizationOptionDto(
            rank,
            $"Option {(char)('A' + rank - 1)}",
            Math.Round(option.Score, 9, MidpointRounding.AwayFromZero),
            metrics,
            changedHeads,
            settings,
            simulation,
            reasons);
    }

    static IrrigationDistributionMetrics CanonicalMetrics(
        ScenarioEvaluation evaluation,
        IrrigationAreaGridMask mask,
        IrrigationNozzleOptimizationRequestDto request)
    {
        var cells = new double[mask.Grid.RowCount, mask.Grid.ColumnCount];
        for (var row = 0; row < mask.Grid.RowCount; row++)
        for (var column = 0; column < mask.Grid.ColumnCount; column++)
            cells[row, column] = evaluation.Depth[row * mask.Grid.ColumnCount + column];
        var simulation = new IrrigationSimulationResult(
            mask.Grid.OriginX,
            mask.Grid.OriginY,
            mask.Grid.CellWidthM,
            mask.Grid.ColumnCount,
            mask.Grid.RowCount,
            evaluation.Fast.MeanDepthMm,
            evaluation.Depth.Min(),
            evaluation.Depth.Max(),
            evaluation.Depth.Sum() / 1_000d * mask.Grid.CellAreaM2,
            cells,
            [],
            [],
            IrrigationSimulationConfidence.ManufacturerDerived);
        return IrrigationDistributionAnalytics.Analyze(
            simulation,
            mask,
            new IrrigationDistributionTarget(request.TargetDepthMm, request.TargetToleranceFraction));
    }

    static IReadOnlyList<IrrigationSimulatorHeadOverrideDto> BuildHeadSettings(
        IReadOnlyList<PreparedHead> heads,
        ScenarioEvaluation evaluation)
        => heads.Select((head, index) => new IrrigationSimulatorHeadOverrideDto(
                head.Source.HeadPubId,
                evaluation.Assignment[index] is not null && evaluation.Runtimes[index] > Epsilon,
                evaluation.Runtimes[index],
                evaluation.Assignment[index]?.Source.NozzlePubId,
                evaluation.Assignment[index]?.Source.PressureBar,
                null))
            .ToList();

    static IReadOnlyList<string> BuildReasonCodes(
        FastMetrics baseline,
        FastMetrics option,
        IReadOnlyList<double> runtimes,
        IrrigationNozzleOptimizationRequestDto request)
    {
        var result = new List<string>();
        if (Greater(option.DUlq, baseline.DUlq)) result.Add(IrrigationOptimizationReasonCodes.ImprovedLowQuarter);
        if (Greater(option.CU, baseline.CU)) result.Add(IrrigationOptimizationReasonCodes.ImprovedCu);
        if (option.TargetDeviationMm + Epsilon < baseline.TargetDeviationMm) result.Add(IrrigationOptimizationReasonCodes.ReducedTargetDeviation);
        if (option.OutsideTargetPercent + Epsilon < baseline.OutsideTargetPercent) result.Add(IrrigationOptimizationReasonCodes.ReducedOutsideTargetApplication);
        if (option.OutsideTargetAreaVolumeM3 + Epsilon < baseline.OutsideTargetAreaVolumeM3) result.Add(IrrigationOptimizationReasonCodes.ReducedOverspray);
        if (option.TotalWaterVolumeM3 + Epsilon < baseline.TotalWaterVolumeM3) result.Add(IrrigationOptimizationReasonCodes.ReducedWaterVolume);
        if (request.MaximumSimultaneousFlowM3H.HasValue &&
            option.FlowM3H >= request.MaximumSimultaneousFlowM3H.Value * 0.995d)
            result.Add(IrrigationOptimizationReasonCodes.FlowLimitReached);
        if (request.MaximumNozzleChanges > 0 && option.NozzleChangeCount == request.MaximumNozzleChanges)
            result.Add(IrrigationOptimizationReasonCodes.NozzleChangeLimitReached);
        if (runtimes.Any(x => x >= request.MaximumRuntimeMinutes - Epsilon))
            result.Add(IrrigationOptimizationReasonCodes.RuntimeLimitReached);
        if (option.MeanDepthMm + Epsilon >= request.TargetDepthMm * (1d - request.TargetToleranceFraction) &&
            option.MeanDepthMm - Epsilon <= request.TargetDepthMm * (1d + request.TargetToleranceFraction))
            result.Add(IrrigationOptimizationReasonCodes.TargetDepthSatisfied);
        return result;
    }

    static IrrigationNozzleOptimizationResultDto EmptyResult(
        IrrigationNozzleOptimizationProblem problem,
        IrrigationNozzleOptimizationRequestDto request,
        string objective,
        IReadOnlyList<PreparedHead> heads,
        ScenarioEvaluation baseline,
        IReadOnlyList<string> availableFamilies,
        int evaluationCount,
        IReadOnlyList<string> reasonCodes,
        IReadOnlyList<string> warnings)
        => new(
            problem.AreaPubId,
            problem.AreaName,
            objective,
            BuildBaseline(heads, baseline),
            [],
            availableFamilies,
            evaluationCount,
            false,
            reasonCodes,
            warnings);

    static IrrigationOptimizationMetricsDto ToMetricsDto(FastMetrics value)
        => new(
            value.FlowM3H,
            value.DUlq,
            value.CU,
            value.MeanDepthMm,
            value.TargetDeviationMm,
            value.OutsideTargetPercent,
            value.OutsideTargetAreaVolumeM3,
            value.TotalWaterVolumeM3,
            value.NozzleChangeCount);

    static double[] CombineDepths(
        IReadOnlyList<PreparedCandidate?> assignment,
        IReadOnlyList<double> runtimes,
        int cellCount)
    {
        var result = new double[cellCount];
        for (var headIndex = 0; headIndex < assignment.Count; headIndex++)
        {
            var candidate = assignment[headIndex];
            var runtime = runtimes[headIndex];
            if (candidate is null || runtime <= Epsilon)
                continue;
            for (var cellIndex = 0; cellIndex < cellCount; cellIndex++)
                result[cellIndex] += candidate.DepthPerMinuteMm[cellIndex] * runtime;
        }
        return result;
    }

    static double CalculateMeanDepth(
        IReadOnlyList<PreparedCandidate?> assignment,
        IReadOnlyList<double> runtimes,
        IrrigationAreaGridMask mask)
    {
        var sum = 0d;
        for (var headIndex = 0; headIndex < assignment.Count; headIndex++)
        {
            var candidate = assignment[headIndex];
            if (candidate is null || runtimes[headIndex] <= Epsilon)
                continue;
            for (var index = 0; index < mask.CellCount; index++)
            {
                if (mask.Cells[index])
                    sum += candidate.DepthPerMinuteMm[index] * runtimes[headIndex];
            }
        }
        return sum / mask.TargetCellCount;
    }

    static double MeanTargetContributionPerMinute(
        PreparedCandidate? candidate,
        IrrigationAreaGridMask mask)
    {
        if (candidate is null)
            return 0d;
        var sum = 0d;
        for (var index = 0; index < mask.CellCount; index++)
        {
            if (mask.Cells[index])
                sum += candidate.DepthPerMinuteMm[index];
        }
        return sum / mask.TargetCellCount;
    }

    static int CountNozzleChanges(
        IReadOnlyList<PreparedCandidate?> assignment,
        IReadOnlyList<PreparedHead> heads)
        => assignment.Select((candidate, index) =>
                candidate is not null &&
                candidate.Source.NozzlePubId != heads[index].Source.InstalledNozzlePubId)
            .Count(x => x);

    static string AssignmentSignature(IReadOnlyList<PreparedCandidate?> assignment)
        => string.Join('|', assignment.Select(x => x?.Source.NozzlePubId.ToString("N") ?? "none"));

    static string ScenarioSignature(ScenarioEvaluation evaluation)
        => $"{AssignmentSignature(evaluation.Assignment)}:{string.Join(',', evaluation.Runtimes.Select(x => x.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)))}";

    static bool Greater(double? left, double? right)
        => left.HasValue && (!right.HasValue || left.Value > right.Value + Epsilon);

    static double ClampRuntime(double value, double maximum)
        => !double.IsFinite(value) ? 0d : Math.Clamp(value, 0d, maximum);

    static double RoundRuntime(double value, double maximum)
        => Math.Clamp(
            Math.Round(value / RuntimeIncrementMinutes, MidpointRounding.AwayFromZero) * RuntimeIncrementMinutes,
            0d,
            maximum);

    static double ResolveGridResolution(
        IrrigationAreaPolygon targetArea,
        IReadOnlyList<IrrigationOptimizationNozzleCandidate> candidates,
        double requested,
        out bool coarsened)
    {
        var bounds = ExpandedBounds(targetArea, candidates);
        var resolution = requested;
        while (CellCount(bounds, resolution) > MaximumOptimizationCellCount &&
               resolution < MaximumOptimizationGridResolutionM)
        {
            var factor = Math.Sqrt(CellCount(bounds, resolution) / (double)MaximumOptimizationCellCount);
            resolution = Math.Min(
                MaximumOptimizationGridResolutionM,
                Math.Ceiling(resolution * factor * 20d) / 20d);
        }
        if (CellCount(bounds, resolution) > MaximumOptimizationCellCount)
            throw new ArgumentException(
                $"The influencing-head footprint exceeds {MaximumOptimizationCellCount:N0} optimization cells even at {MaximumOptimizationGridResolutionM:0.#} m resolution.",
                nameof(targetArea));
        coarsened = resolution > requested + Epsilon;
        return resolution;
    }

    static IrrigationSimulationGrid CreateGrid(
        IrrigationAreaPolygon polygon,
        IReadOnlyList<IrrigationOptimizationNozzleCandidate> candidates,
        double cellSizeM)
    {
        var bounds = polygon.Metrics.BoundingBox;
        var minimumX = bounds.MinX;
        var minimumY = bounds.MinY;
        var maximumX = bounds.MaxX;
        var maximumY = bounds.MaxY;
        foreach (var candidate in candidates)
        {
            var head = candidate.SimulationHead.Head;
            var radius = (double)candidate.SimulationHead.Performance.RadiusM!.Value;
            minimumX = Math.Min(minimumX, head.MapX!.Value - radius);
            minimumY = Math.Min(minimumY, head.MapY!.Value - radius);
            maximumX = Math.Max(maximumX, head.MapX!.Value + radius);
            maximumY = Math.Max(maximumY, head.MapY!.Value + radius);
        }
        return IrrigationSimulationGrid.FromBounds(
            minimumX - cellSizeM,
            minimumY - cellSizeM,
            maximumX + cellSizeM,
            maximumY + cellSizeM,
            cellSizeM);
    }

    static IrrigationBoundingBox ExpandedBounds(
        IrrigationAreaPolygon targetArea,
        IReadOnlyList<IrrigationOptimizationNozzleCandidate> candidates)
    {
        var bounds = targetArea.Metrics.BoundingBox;
        var minX = bounds.MinX;
        var minY = bounds.MinY;
        var maxX = bounds.MaxX;
        var maxY = bounds.MaxY;
        foreach (var candidate in candidates)
        {
            var radius = (double)(candidate.SimulationHead.Performance.RadiusM ?? 0m);
            var x = candidate.SimulationHead.Head.MapX ?? 0d;
            var y = candidate.SimulationHead.Head.MapY ?? 0d;
            minX = Math.Min(minX, x - radius);
            minY = Math.Min(minY, y - radius);
            maxX = Math.Max(maxX, x + radius);
            maxY = Math.Max(maxY, y + radius);
        }
        return new IrrigationBoundingBox(minX, minY, maxX, maxY);
    }

    static long CellCount(IrrigationBoundingBox bounds, double cellSizeM)
        => checked((long)Math.Ceiling((bounds.MaxX - bounds.MinX + 2d * cellSizeM) / cellSizeM) *
                   (long)Math.Ceiling((bounds.MaxY - bounds.MinY + 2d * cellSizeM) / cellSizeM));

    static void Validate(
        IrrigationNozzleOptimizationProblem problem,
        IrrigationNozzleOptimizationRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(problem);
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(problem.TargetArea);
        ArgumentNullException.ThrowIfNull(problem.Heads);
        if (problem.AreaPubId == Guid.Empty || request.AreaPubId != problem.AreaPubId)
            throw new ArgumentException("The optimization request must identify the prepared irrigation area.", nameof(request));
        if (problem.Heads.Count is < 1 or > MaximumHeadCount)
            throw new ArgumentException($"The small-area optimizer supports between 1 and {MaximumHeadCount} influencing heads.", nameof(problem));
        if (problem.Heads.Any(x => x.HeadPubId == Guid.Empty) || problem.Heads.GroupBy(x => x.HeadPubId).Any(x => x.Count() > 1))
            throw new ArgumentException("Optimization heads must have unique public identifiers.", nameof(problem));
        if (problem.Heads.Any(x => x.Candidates is null || x.Candidates.Any(candidate =>
                candidate.NozzlePubId == Guid.Empty ||
                candidate.SimulationHead.Head.PubId != x.HeadPubId ||
                candidate.SimulationHead.Performance.NozzlePubId != candidate.NozzlePubId ||
                !candidate.SimulationHead.Performance.Supported ||
                !candidate.SimulationHead.Performance.FlowM3H.HasValue ||
                !candidate.SimulationHead.Performance.RadiusM.HasValue)))
            throw new ArgumentException("Every nozzle candidate must contain supported performance for its fixed head.", nameof(problem));
        if (!double.IsFinite(request.TargetDepthMm) || request.TargetDepthMm is <= 0d or > 100d)
            throw new ArgumentException("Target depth must be greater than 0 and no more than 100 mm.", nameof(request));
        if (request.MaximumSimultaneousFlowM3H.HasValue &&
            (!double.IsFinite(request.MaximumSimultaneousFlowM3H.Value) || request.MaximumSimultaneousFlowM3H.Value <= 0d))
            throw new ArgumentException("Maximum simultaneous flow must be a positive finite value.", nameof(request));
        if (request.MaximumNozzleChanges < 0 || request.MaximumNozzleChanges > problem.Heads.Count)
            throw new ArgumentException("Maximum nozzle changes must be between zero and the influencing-head count.", nameof(request));
        if (!double.IsFinite(request.MaximumRuntimeMinutes) || request.MaximumRuntimeMinutes is <= 0d or > 30d)
            throw new ArgumentException("Maximum runtime must be greater than 0 and no more than 30 minutes.", nameof(request));
        if (!double.IsFinite(request.MinimumPressureSuitability) || request.MinimumPressureSuitability is < 0d or > 1d)
            throw new ArgumentException("Minimum pressure suitability must be between zero and one.", nameof(request));
        if (!double.IsFinite(request.TargetToleranceFraction) || request.TargetToleranceFraction is < 0d or > 0.5d)
            throw new ArgumentException("Target tolerance must be between zero and 0.5.", nameof(request));
        if (!double.IsFinite(request.GridResolutionM) || request.GridResolutionM is < 0.25d or > MaximumOptimizationGridResolutionM)
            throw new ArgumentException("Optimization grid resolution must be between 0.25 and 2 metres.", nameof(request));
        if (request.RequestedOptionCount is < 1 or > 10)
            throw new ArgumentException("Request between one and ten ranked options.", nameof(request));
        if (!IrrigationOptimizationObjectiveCodes.IsValid(request.Objective))
            throw new ArgumentException($"Objective must be one of: {string.Join(", ", IrrigationOptimizationObjectiveCodes.All)}.", nameof(request));
        if (string.Equals(request.Objective, IrrigationOptimizationObjectiveCodes.CompositeWeighted, StringComparison.OrdinalIgnoreCase))
            ValidateWeights(request.CompositeWeights ?? new IrrigationOptimizationCompositeWeightsDto());
    }

    static void ValidateWeights(IrrigationOptimizationCompositeWeightsDto weights)
    {
        var values = new[]
        {
            weights.DUlq,
            weights.CU,
            weights.TargetDeviation,
            weights.OutsideTargetApplication,
            weights.TotalWaterVolume,
            weights.NozzleChanges
        };
        if (values.Any(x => !double.IsFinite(x) || x < 0d) || values.Sum() <= 0d)
            throw new ArgumentException("Composite objective weights must be finite, non-negative, and include at least one positive value.", nameof(weights));
    }

    sealed record PreparedHead(
        IrrigationOptimizationHead Source,
        PreparedCandidate? InstalledCandidate,
        IReadOnlyList<PreparedCandidate> EligibleCandidates);

    sealed record PreparedCandidate(
        IrrigationOptimizationNozzleCandidate Source,
        double[] DepthPerMinuteMm,
        double FlowM3H);

    sealed record FastMetrics(
        double FlowM3H,
        double? DUlq,
        double? CU,
        double MeanDepthMm,
        double TargetDeviationMm,
        double OutsideTargetPercent,
        double OutsideTargetAreaVolumeM3,
        double TotalWaterVolumeM3,
        int NozzleChangeCount);

    sealed record ScenarioEvaluation(
        PreparedCandidate?[] Assignment,
        double[] Runtimes,
        double[] Depth,
        FastMetrics Fast,
        double Score,
        double FeasibilityPenalty,
        bool Feasible);

    sealed class ScenarioEvaluationComparer : IComparer<ScenarioEvaluation>
    {
        public static ScenarioEvaluationComparer Instance { get; } = new();

        public int Compare(ScenarioEvaluation? left, ScenarioEvaluation? right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left is null) return 1;
            if (right is null) return -1;
            var value = left.FeasibilityPenalty.CompareTo(right.FeasibilityPenalty);
            if (value != 0) return value;
            value = left.Score.CompareTo(right.Score);
            if (value != 0) return value;
            value = left.Fast.TargetDeviationMm.CompareTo(right.Fast.TargetDeviationMm);
            if (value != 0) return value;
            value = left.Fast.OutsideTargetPercent.CompareTo(right.Fast.OutsideTargetPercent);
            if (value != 0) return value;
            value = left.Fast.TotalWaterVolumeM3.CompareTo(right.Fast.TotalWaterVolumeM3);
            if (value != 0) return value;
            value = left.Fast.NozzleChangeCount.CompareTo(right.Fast.NozzleChangeCount);
            if (value != 0) return value;
            return string.CompareOrdinal(ScenarioSignature(left), ScenarioSignature(right));
        }
    }
}
