using AmsRecords.Surfaces;
using static AmsRecords.Surfaces.SurfaceMappingDtos;

namespace AmsRecords.Irrigation;

/// <summary>
/// A target depth and its symmetric acceptable tolerance, expressed as a fraction. Values exactly on either
/// calculated bound are within target.
/// </summary>
public sealed record IrrigationDistributionTarget(
    double ApplicationDepthMm,
    double ToleranceFraction)
{
    public double LowerBoundMm => ApplicationDepthMm * (1d - ToleranceFraction);
    public double UpperBoundMm => ApplicationDepthMm * (1d + ToleranceFraction);
}

/// <summary>A regular simulation-grid sample. One millimetre over one square metre equals one litre.</summary>
public sealed record IrrigationPrecipitationCell(
    int Row,
    int Column,
    double CenterLatitude,
    double CenterLongitude,
    double ApplicationDepthMm,
    double ApproximateAreaM2)
{
    /// <summary>Local planar X coordinate. The longitude-named field is retained for compatibility with the partial contract.</summary>
    public double CenterX => CenterLongitude;

    /// <summary>Local planar Y coordinate. The latitude-named field is retained for compatibility with the partial contract.</summary>
    public double CenterY => CenterLatitude;
}

public enum IrrigationProblemZoneKind
{
    LowZone,
    HighZone
}

/// <summary>A maximal group of off-range target cells connected through shared grid edges.</summary>
public sealed record IrrigationProblemZone(
    IrrigationProblemZoneKind Kind,
    int CellCount,
    double ApproximateAreaM2,
    double AverageApplicationDepthMm,
    double MinimumApplicationDepthMm,
    double MaximumApplicationDepthMm,
    double CentroidX,
    double CentroidY)
{
    /// <summary>Compatibility alias for callers of the earlier geographic partial contract.</summary>
    public double CentroidLongitude => CentroidX;

    /// <summary>Compatibility alias for callers of the earlier geographic partial contract.</summary>
    public double CentroidLatitude => CentroidY;
}

/// <summary>
/// Deterministic distribution metrics calculated from equal-area simulation samples in the canonical target mask.
/// Standard deviation is the population standard deviation. DUlq uses the lowest ceil(N / 4) whole-cell samples.
/// CU, CV, and DUlq are null when their zero mean denominator makes them undefined; an extreme valid CU may be
/// negative. Uniformity values are unitless ratios and area classifications are percentages.
/// </summary>
public sealed record IrrigationDistributionMetrics(
    int TargetCellCount,
    double ApproximateTargetAreaM2,
    double TargetApplicationDepthMm,
    double TargetToleranceFraction,
    double LowerTargetBoundMm,
    double UpperTargetBoundMm,
    double MeanApplicationDepthMm,
    double MinimumApplicationDepthMm,
    double MaximumApplicationDepthMm,
    double StandardDeviationMm,
    double? CoefficientOfVariation,
    double? DistributionUniformityLowQuarter,
    double? ChristiansenUniformityCoefficient,
    double MeanTargetDeviationMm,
    double MeanAbsoluteTargetDeviationMm,
    double BelowTargetPercent,
    double WithinTargetPercent,
    double AboveTargetPercent,
    double OutsideTargetPercent,
    double TotalApplicationVolumeLitres,
    double OutsideTargetApplicationVolumeLitres,
    double UnderApplicationDeficitVolumeLitres,
    double OverApplicationExcessVolumeLitres,
    double OutsideTargetDeviationVolumeLitres,
    IrrigationAreaWaterMetrics? AreaWaterMetrics,
    IReadOnlyList<IrrigationProblemZone> ProblemZones)
{
    /// <summary>The ceiling of 25% of target samples, with one sample minimum.</summary>
    public int LowQuarterSampleCellCount => Math.Max(1, (int)Math.Ceiling(TargetCellCount * 0.25d));

    public double TargetAreaApplicationVolumeM3 =>
        AreaWaterMetrics?.TargetAreaVolumeM3 ?? TotalApplicationVolumeLitres / 1_000d;

    /// <summary>Water applied to target cells whose depth is below or above the accepted range.</summary>
    public double OutsideTargetRangeApplicationVolumeM3 => OutsideTargetApplicationVolumeLitres / 1_000d;

    /// <summary>Volume adjustment needed to move off-range target cells to the nearest accepted bound.</summary>
    public double OutsideTargetRangeDeviationVolumeM3 => OutsideTargetDeviationVolumeLitres / 1_000d;

    public double UnderApplicationDeficitVolumeM3 => UnderApplicationDeficitVolumeLitres / 1_000d;

    public double OverApplicationExcessVolumeM3 => OverApplicationExcessVolumeLitres / 1_000d;

    /// <summary>Water represented by grid cells outside the target-area mask; null for target-cell-only analysis.</summary>
    public double? OutsideTargetAreaApplicationVolumeM3 => AreaWaterMetrics?.OutsideTargetAreaVolumeM3;

    public double? AllGridApplicationVolumeM3 => AreaWaterMetrics?.AllAppliedVolumeM3;

    public double? TargetApplicationEfficiencyPercent => AreaWaterMetrics?.TargetEfficiencyPercent;
}

public enum IrrigationDistributionMetricUnit
{
    CellCount,
    SquareMetres,
    Millimetres,
    Ratio,
    Percent,
    Litres,
    CubicMetres
}

public sealed record IrrigationMetricChange(
    IrrigationDistributionMetricUnit Unit,
    double? ScenarioAValue,
    double? ScenarioBValue)
{
    /// <summary>Signed Scenario B minus Scenario A change, in <see cref="Unit"/>.</summary>
    public double? Delta => ScenarioAValue.HasValue && ScenarioBValue.HasValue
        ? ScenarioBValue.Value - ScenarioAValue.Value
        : null;

    /// <summary>Compatibility alias for the earlier partial comparison contract. This value is signed.</summary>
    public double? AbsoluteChange => Delta;
}

public sealed record IrrigationDistributionComparison(
    string ScenarioAName,
    string ScenarioBName,
    IrrigationMetricChange MeanApplicationDepthMm,
    IrrigationMetricChange MinimumApplicationDepthMm,
    IrrigationMetricChange MaximumApplicationDepthMm,
    IrrigationMetricChange StandardDeviationMm,
    IrrigationMetricChange CoefficientOfVariation,
    IrrigationMetricChange DistributionUniformityLowQuarter,
    IrrigationMetricChange ChristiansenUniformityCoefficient,
    IrrigationMetricChange MeanTargetDeviationMm,
    IrrigationMetricChange MeanAbsoluteTargetDeviationMm,
    IrrigationMetricChange BelowTargetPercent,
    IrrigationMetricChange WithinTargetPercent,
    IrrigationMetricChange AboveTargetPercent,
    IrrigationMetricChange OutsideTargetPercent,
    IrrigationMetricChange OutsideTargetApplicationVolumeLitres,
    IrrigationMetricChange OutsideTargetDeviationVolumeLitres,
    IrrigationMetricChange TargetAreaApplicationVolumeM3,
    IrrigationMetricChange OutsideTargetRangeApplicationVolumeM3,
    IrrigationMetricChange OutsideTargetAreaApplicationVolumeM3,
    IrrigationMetricChange TargetApplicationEfficiencyPercent);

/// <summary>Uniformity statistics shared by equal-area simulated cells and equally weighted field catch cans.</summary>
public sealed record IrrigationUniformityMetrics(
    double MeanMm,
    double MinimumMm,
    double MaximumMm,
    double StandardDeviationMm,
    double? CoefficientOfVariation,
    double? DistributionUniformityLowQuarter,
    double? ChristiansenUniformityCoefficient);

/// <summary>Pure analytical calculations over a simulated irrigation precipitation grid.</summary>
public static class IrrigationDistributionAnalytics
{
    static readonly (int Row, int Column)[] CardinalOffsets =
    [
        (-1, 0),
        (0, -1),
        (0, 1),
        (1, 0)
    ];

    public static IrrigationUniformityMetrics AnalyzeEqualAreaDepths(IReadOnlyList<double> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        if (values.Count == 0)
            throw new ArgumentException("At least one precipitation depth is required.", nameof(values));
        if (values.Any(value => !double.IsFinite(value) || value < 0d))
            throw new ArgumentOutOfRangeException(nameof(values), "Precipitation depths must be finite and non-negative.");

        var mean = values.Average();
        var standardDeviation = Math.Sqrt(values.Sum(value => Square(value - mean)) / values.Count);
        var lowQuarterCount = Math.Max(1, (int)Math.Ceiling(values.Count * 0.25d));
        var lowQuarterMean = values.OrderBy(value => value).Take(lowQuarterCount).Average();
        return new IrrigationUniformityMetrics(
            mean,
            values.Min(),
            values.Max(),
            standardDeviation,
            mean > 0d ? standardDeviation / mean : null,
            mean > 0d ? lowQuarterMean / mean : null,
            mean > 0d
                ? 1d - values.Sum(value => Math.Abs(value - mean)) / (values.Count * mean)
                : null);
    }

    /// <summary>
    /// Analyzes one precipitation result through the canonical planar target mask. The mask and result must describe
    /// the exact same grid. Target statistics ignore outside-mask cells, while area water metrics retain their volume
    /// as outside-target application.
    /// </summary>
    public static IrrigationDistributionMetrics Analyze(
        IrrigationSimulationResult simulation,
        IrrigationAreaGridMask targetMask,
        IrrigationDistributionTarget target)
    {
        ArgumentNullException.ThrowIfNull(simulation);
        ArgumentNullException.ThrowIfNull(targetMask);
        ValidateTarget(target);

        var waterMetrics = targetMask.CalculateWaterMetrics(simulation);
        if (targetMask.TargetCellCount == 0)
            throw new ArgumentException("The target-area mask does not contain any simulation cell centres.", nameof(targetMask));

        var cells = new List<IrrigationPrecipitationCell>(targetMask.TargetCellCount);
        for (var row = 0; row < simulation.Height; row++)
        for (var column = 0; column < simulation.Width; column++)
        {
            if (!targetMask.IsWithinTarget(row, column))
                continue;

            var centerX = simulation.GridOriginX + (column + 0.5d) * simulation.CellSizeM;
            var centerY = simulation.GridOriginY + (row + 0.5d) * simulation.CellSizeM;
            cells.Add(new IrrigationPrecipitationCell(
                row,
                column,
                CenterLatitude: centerY,
                CenterLongitude: centerX,
                ApplicationDepthMm: simulation.Cells[row, column],
                ApproximateAreaM2: targetMask.Grid.CellAreaM2));
        }

        return Calculate(cells, target, waterMetrics);
    }

    /// <summary>Creates the canonical grid mask from a validated planar area and analyzes the precipitation result.</summary>
    public static IrrigationDistributionMetrics Analyze(
        IrrigationSimulationResult simulation,
        IrrigationAreaPolygon targetArea,
        IrrigationDistributionTarget target)
    {
        ArgumentNullException.ThrowIfNull(simulation);
        ArgumentNullException.ThrowIfNull(targetArea);
        var grid = new IrrigationSimulationGrid(
            simulation.GridOriginX,
            simulation.GridOriginY,
            simulation.Width,
            simulation.Height,
            simulation.CellSizeM);
        return Analyze(simulation, targetArea.CreateGridMask(grid), target);
    }

    /// <summary>
    /// Selects target samples by cell-centre containment and analyzes only those samples. Cell area therefore provides
    /// a raster approximation at polygon edges.
    /// </summary>
    public static IrrigationDistributionMetrics Analyze(
        IReadOnlyList<IrrigationPrecipitationCell> cells,
        string targetGeoJson,
        IrrigationDistributionTarget target)
    {
        ValidateInputs(cells, target);
        if (string.IsNullOrWhiteSpace(targetGeoJson))
            throw new ArgumentException("A target Polygon or MultiPolygon GeoJSON value is required.", nameof(targetGeoJson));

        var containment = SurfaceGeometry.ContainsPoints(
            targetGeoJson,
            cells.Select(x => new CoordinateDto(x.CenterLatitude, x.CenterLongitude)).ToArray());
        var targetCells = cells.Where((_, index) => containment[index]).ToArray();
        if (targetCells.Length == 0)
            throw new ArgumentException("The target polygon does not contain any simulation cell centres.", nameof(targetGeoJson));

        return Calculate(targetCells, target, areaWaterMetrics: null);
    }

    /// <summary>Analyzes cells already clipped or masked to the target area by the simulation engine.</summary>
    public static IrrigationDistributionMetrics AnalyzeTargetCells(
        IReadOnlyList<IrrigationPrecipitationCell> targetCells,
        IrrigationDistributionTarget target)
    {
        ValidateInputs(targetCells, target);
        if (targetCells.Count == 0)
            throw new ArgumentException("At least one target simulation cell is required.", nameof(targetCells));

        return Calculate(targetCells, target, areaWaterMetrics: null);
    }

    public static IrrigationDistributionComparison Compare(
        string scenarioAName,
        IrrigationDistributionMetrics scenarioA,
        string scenarioBName,
        IrrigationDistributionMetrics scenarioB)
    {
        ArgumentNullException.ThrowIfNull(scenarioA);
        ArgumentNullException.ThrowIfNull(scenarioB);
        if (string.IsNullOrWhiteSpace(scenarioAName))
            throw new ArgumentException("A scenario name is required.", nameof(scenarioAName));
        if (string.IsNullOrWhiteSpace(scenarioBName))
            throw new ArgumentException("A scenario name is required.", nameof(scenarioBName));

        return new IrrigationDistributionComparison(
            scenarioAName.Trim(),
            scenarioBName.Trim(),
            Change(IrrigationDistributionMetricUnit.Millimetres, scenarioA.MeanApplicationDepthMm, scenarioB.MeanApplicationDepthMm),
            Change(IrrigationDistributionMetricUnit.Millimetres, scenarioA.MinimumApplicationDepthMm, scenarioB.MinimumApplicationDepthMm),
            Change(IrrigationDistributionMetricUnit.Millimetres, scenarioA.MaximumApplicationDepthMm, scenarioB.MaximumApplicationDepthMm),
            Change(IrrigationDistributionMetricUnit.Millimetres, scenarioA.StandardDeviationMm, scenarioB.StandardDeviationMm),
            Change(IrrigationDistributionMetricUnit.Ratio, scenarioA.CoefficientOfVariation, scenarioB.CoefficientOfVariation),
            Change(IrrigationDistributionMetricUnit.Ratio, scenarioA.DistributionUniformityLowQuarter, scenarioB.DistributionUniformityLowQuarter),
            Change(IrrigationDistributionMetricUnit.Ratio, scenarioA.ChristiansenUniformityCoefficient, scenarioB.ChristiansenUniformityCoefficient),
            Change(IrrigationDistributionMetricUnit.Millimetres, scenarioA.MeanTargetDeviationMm, scenarioB.MeanTargetDeviationMm),
            Change(IrrigationDistributionMetricUnit.Millimetres, scenarioA.MeanAbsoluteTargetDeviationMm, scenarioB.MeanAbsoluteTargetDeviationMm),
            Change(IrrigationDistributionMetricUnit.Percent, scenarioA.BelowTargetPercent, scenarioB.BelowTargetPercent),
            Change(IrrigationDistributionMetricUnit.Percent, scenarioA.WithinTargetPercent, scenarioB.WithinTargetPercent),
            Change(IrrigationDistributionMetricUnit.Percent, scenarioA.AboveTargetPercent, scenarioB.AboveTargetPercent),
            Change(IrrigationDistributionMetricUnit.Percent, scenarioA.OutsideTargetPercent, scenarioB.OutsideTargetPercent),
            Change(IrrigationDistributionMetricUnit.Litres, scenarioA.OutsideTargetApplicationVolumeLitres, scenarioB.OutsideTargetApplicationVolumeLitres),
            Change(IrrigationDistributionMetricUnit.Litres, scenarioA.OutsideTargetDeviationVolumeLitres, scenarioB.OutsideTargetDeviationVolumeLitres),
            Change(IrrigationDistributionMetricUnit.CubicMetres, scenarioA.TargetAreaApplicationVolumeM3, scenarioB.TargetAreaApplicationVolumeM3),
            Change(IrrigationDistributionMetricUnit.CubicMetres, scenarioA.OutsideTargetRangeApplicationVolumeM3, scenarioB.OutsideTargetRangeApplicationVolumeM3),
            Change(IrrigationDistributionMetricUnit.CubicMetres, scenarioA.OutsideTargetAreaApplicationVolumeM3, scenarioB.OutsideTargetAreaApplicationVolumeM3),
            Change(IrrigationDistributionMetricUnit.Percent, scenarioA.TargetApplicationEfficiencyPercent, scenarioB.TargetApplicationEfficiencyPercent));
    }

    static IrrigationDistributionMetrics Calculate(
        IReadOnlyList<IrrigationPrecipitationCell> targetCells,
        IrrigationDistributionTarget target,
        IrrigationAreaWaterMetrics? areaWaterMetrics)
    {
        var values = targetCells.Select(x => x.ApplicationDepthMm).ToArray();
        var uniformity = AnalyzeEqualAreaDepths(values);
        var mean = uniformity.MeanMm;

        var lowerBound = target.LowerBoundMm;
        var upperBound = target.UpperBoundMm;
        var below = targetCells.Where(x => x.ApplicationDepthMm < lowerBound).ToArray();
        var above = targetCells.Where(x => x.ApplicationDepthMm > upperBound).ToArray();
        var outside = below.Concat(above).ToArray();
        var targetArea = targetCells.Sum(x => x.ApproximateAreaM2);
        var belowPercent = PercentOfArea(below, targetArea);
        var abovePercent = PercentOfArea(above, targetArea);
        var outsidePercent = belowPercent + abovePercent;
        var withinPercent = 100d - outsidePercent;
        var underApplicationDeficit = below.Sum(x => (lowerBound - x.ApplicationDepthMm) * x.ApproximateAreaM2);
        var overApplicationExcess = above.Sum(x => (x.ApplicationDepthMm - upperBound) * x.ApproximateAreaM2);

        return new IrrigationDistributionMetrics(
            targetCells.Count,
            targetArea,
            target.ApplicationDepthMm,
            target.ToleranceFraction,
            lowerBound,
            upperBound,
            mean,
            uniformity.MinimumMm,
            uniformity.MaximumMm,
            uniformity.StandardDeviationMm,
            uniformity.CoefficientOfVariation,
            uniformity.DistributionUniformityLowQuarter,
            uniformity.ChristiansenUniformityCoefficient,
            mean - target.ApplicationDepthMm,
            values.Average(x => Math.Abs(x - target.ApplicationDepthMm)),
            belowPercent,
            withinPercent,
            abovePercent,
            outsidePercent,
            targetCells.Sum(ApplicationVolumeLitres),
            outside.Sum(ApplicationVolumeLitres),
            underApplicationDeficit,
            overApplicationExcess,
            underApplicationDeficit + overApplicationExcess,
            areaWaterMetrics,
            FindProblemZones(targetCells, lowerBound, upperBound));
    }

    static IReadOnlyList<IrrigationProblemZone> FindProblemZones(
        IReadOnlyList<IrrigationPrecipitationCell> cells,
        double lowerBound,
        double upperBound)
    {
        var zones = new List<IrrigationProblemZone>();
        AddZones(cells.Where(x => x.ApplicationDepthMm < lowerBound), IrrigationProblemZoneKind.LowZone, zones);
        AddZones(cells.Where(x => x.ApplicationDepthMm > upperBound), IrrigationProblemZoneKind.HighZone, zones);
        return zones;
    }

    static void AddZones(
        IEnumerable<IrrigationPrecipitationCell> candidates,
        IrrigationProblemZoneKind kind,
        ICollection<IrrigationProblemZone> destination)
    {
        var cellsByPosition = candidates.ToDictionary(x => (x.Row, x.Column));
        var visited = new HashSet<(int Row, int Column)>();
        foreach (var start in cellsByPosition.Values.OrderBy(x => x.Row).ThenBy(x => x.Column))
        {
            if (!visited.Add((start.Row, start.Column)))
                continue;

            var cluster = new List<IrrigationPrecipitationCell>();
            var queue = new Queue<IrrigationPrecipitationCell>();
            queue.Enqueue(start);
            while (queue.TryDequeue(out var current))
            {
                cluster.Add(current);
                foreach (var offset in CardinalOffsets)
                {
                    var position = (current.Row + offset.Row, current.Column + offset.Column);
                    if (cellsByPosition.TryGetValue(position, out var neighbour) && visited.Add(position))
                        queue.Enqueue(neighbour);
                }
            }

            var area = cluster.Sum(x => x.ApproximateAreaM2);
            destination.Add(new IrrigationProblemZone(
                kind,
                cluster.Count,
                area,
                cluster.Average(x => x.ApplicationDepthMm),
                cluster.Min(x => x.ApplicationDepthMm),
                cluster.Max(x => x.ApplicationDepthMm),
                cluster.Sum(x => x.CenterX * x.ApproximateAreaM2) / area,
                cluster.Sum(x => x.CenterY * x.ApproximateAreaM2) / area));
        }
    }

    static void ValidateInputs(
        IReadOnlyList<IrrigationPrecipitationCell> cells,
        IrrigationDistributionTarget target)
    {
        ArgumentNullException.ThrowIfNull(cells);
        ValidateTarget(target);

        var positions = new HashSet<(int Row, int Column)>();
        foreach (var cell in cells)
        {
            if (cell.Row < 0 || cell.Column < 0)
                throw new ArgumentOutOfRangeException(nameof(cells), "Grid row and column indexes cannot be negative.");
            if (!positions.Add((cell.Row, cell.Column)))
                throw new ArgumentException($"Grid position ({cell.Row}, {cell.Column}) occurs more than once.", nameof(cells));
            if (!double.IsFinite(cell.CenterLatitude) || !double.IsFinite(cell.CenterLongitude))
                throw new ArgumentOutOfRangeException(nameof(cells), "Every grid cell must have finite centre coordinates.");
            if (!double.IsFinite(cell.ApplicationDepthMm) || cell.ApplicationDepthMm < 0d)
                throw new ArgumentOutOfRangeException(nameof(cells), "Application depth must be finite and cannot be negative.");
            if (!double.IsFinite(cell.ApproximateAreaM2) || cell.ApproximateAreaM2 <= 0d)
                throw new ArgumentOutOfRangeException(nameof(cells), "Approximate cell area must be finite and greater than zero.");
        }
    }

    static void ValidateTarget(IrrigationDistributionTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);
        if (!double.IsFinite(target.ApplicationDepthMm) || target.ApplicationDepthMm <= 0d)
            throw new ArgumentOutOfRangeException(nameof(target), "Target application depth must be finite and greater than zero.");
        if (!double.IsFinite(target.ToleranceFraction) || target.ToleranceFraction is < 0d or > 1d)
            throw new ArgumentOutOfRangeException(nameof(target), "Target tolerance must be a finite fraction from zero to one.");
    }

    static double PercentOfArea(IEnumerable<IrrigationPrecipitationCell> cells, double targetArea)
        => cells.Sum(x => x.ApproximateAreaM2) / targetArea * 100d;

    static double ApplicationVolumeLitres(IrrigationPrecipitationCell cell)
        => cell.ApplicationDepthMm * cell.ApproximateAreaM2;

    static double Square(double value) => value * value;

    static IrrigationMetricChange Change(
        IrrigationDistributionMetricUnit unit,
        double? scenarioA,
        double? scenarioB)
        => new(unit, scenarioA, scenarioB);
}
