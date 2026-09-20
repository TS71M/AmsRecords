using static AmsRecords.Irrigation.IrrigationAdvisorDesignDtos;

namespace AmsRecords.Irrigation;

/// <summary>Bounded, deterministic suggestions near the target edge, prioritizing under-watered gaps.</summary>
public static class IrrigationDesignPlacement
{
    public static IReadOnlyList<IrrigationPlanarPoint> Suggest(DesignOption baseline,
        IReadOnlyList<IrrigationPlanarPoint> existing, CancellationToken ct = default)
    {
        var grid = baseline.Grid;
        var candidates = new List<(IrrigationPlanarPoint Point, double Score)>();
        bool Inside(int x, int y) => x >= 0 && y >= 0 && x < grid.Width && y < grid.Height && grid.TargetMask[y * grid.Width + x];
        static double Distance(IrrigationPlanarPoint a, IrrigationPlanarPoint b) => Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        for (var y = 0; y < grid.Height; y++)
        for (var x = 0; x < grid.Width; x++)
        {
            ct.ThrowIfCancellationRequested();
            if (!Inside(x, y) || Inside(x - 1, y) && Inside(x + 1, y) && Inside(x, y - 1) && Inside(x, y + 1)) continue;
            var point = new IrrigationPlanarPoint(grid.OriginX + (x + .5) * grid.CellSizeM, grid.OriginY + (y + .5) * grid.CellSizeM);
            var nearest = existing.Count == 0 ? double.PositiveInfinity : existing.Min(p => Distance(p, point));
            if (nearest < 3) continue;
            candidates.Add((point, grid.ApplicationDepthMm[y * grid.Width + x] / (baseline.TargetDepthMm ?? 5) + 3 / nearest));
        }
        var selected = new List<IrrigationPlanarPoint>();
        foreach (var candidate in candidates.OrderBy(c => c.Score).ThenBy(c => c.Point.X).ThenBy(c => c.Point.Y))
        {
            if (selected.Any(p => Distance(p, candidate.Point) < 5)) continue;
            selected.Add(candidate.Point);
            if (selected.Count == 4) break;
        }
        return selected;
    }

    // An extra installation must improve uniformity, preserve coverage and improve the overall objective.
    public static bool Improves(DesignOption baseline, DesignOption proposed) =>
        proposed.Settings.Any(s => s.LayoutAction == "Add" && s.Enabled) &&
        ImprovesQuality(baseline, proposed, 1, .02);

    public static bool ImprovesQuality(DesignOption baseline, DesignOption proposed, double uniformityGain = .5, double scoreGain = .01) =>
        proposed.CoveragePercent >= baseline.CoveragePercent - .1 &&
        proposed.UniformityPercent >= baseline.UniformityPercent + uniformityGain &&
        proposed.Score < baseline.Score - scoreGain;
}
