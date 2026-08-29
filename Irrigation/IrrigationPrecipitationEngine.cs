using Lib.Constants;
using static AmsRecords.Irrigation.SprinklerPerformanceDtos;

namespace AmsRecords.Irrigation;

/// <summary>
/// Identifies the source of the radial water-distribution curve used for a head.
/// </summary>
public enum IrrigationRadialProfileSource
{
    GenericFallback = 0,
    CatalogProfile = 1
}

/// <summary>
/// Describes the evidence behind the simulated distribution. It is not an accuracy guarantee.
/// </summary>
public enum IrrigationSimulationConfidence
{
    NoWaterApplied = 0,
    GenericModeled = 1,
    ManufacturerDerived = 2,
    Measured = 3
}

/// <summary>
/// One operating Digital Twin head with pressure-resolved sprinkler performance. Map coordinates
/// are local planar metres. Orientation is the centre of a part-circle sector, measured
/// counter-clockwise from the positive X axis. Runtime is in seconds.
/// </summary>
public sealed record IrrigationSimulationHead(
    IrrigationDigitalTwinDtos.IrrigationHeadDto Head,
    double RuntimeSeconds,
    SprinklerPerformanceResult Performance,
    SprinklerDistributionProfileResult? DistributionProfile = null,
    bool IsOperating = true);

/// <summary>
/// A regular planar grid. Width and Height are cell counts. Row zero begins at GridOriginY and
/// column zero begins at GridOriginX.
/// </summary>
public sealed record IrrigationSimulationGrid(
    double GridOriginX,
    double GridOriginY,
    int Width,
    int Height,
    double CellSizeM = IrrigationPrecipitationEngine.DefaultCellSizeM)
{
    public static IrrigationSimulationGrid FromBounds(
        double minimumX,
        double minimumY,
        double maximumX,
        double maximumY,
        double cellSizeM = IrrigationPrecipitationEngine.DefaultCellSizeM)
    {
        if (!double.IsFinite(minimumX) || !double.IsFinite(minimumY) ||
            !double.IsFinite(maximumX) || !double.IsFinite(maximumY))
            throw new ArgumentException("Grid bounds must be finite.");
        if (!double.IsFinite(cellSizeM) || cellSizeM <= 0d)
            throw new ArgumentOutOfRangeException(nameof(cellSizeM), "Cell size must be a finite positive number of metres.");
        if (maximumX <= minimumX || maximumY <= minimumY)
            throw new ArgumentException("Maximum grid bounds must be greater than minimum grid bounds.");

        var width = Math.Ceiling((maximumX - minimumX) / cellSizeM);
        var height = Math.Ceiling((maximumY - minimumY) / cellSizeM);
        if (width > int.MaxValue || height > int.MaxValue)
            throw new ArgumentException("The requested grid dimensions exceed supported integer cell counts.");

        return new IrrigationSimulationGrid(minimumX, minimumY, (int)width, (int)height, cellSizeM);
    }
}

public sealed record IrrigationSimulationRequest(
    IrrigationSimulationGrid Grid,
    IReadOnlyList<IrrigationSimulationHead> Heads);

public sealed record IrrigationHeadSimulationSummary(
    Guid HeadPubId,
    string HeadName,
    Guid NozzlePubId,
    decimal RequestedPressureBar,
    double FlowM3H,
    double RuntimeSeconds,
    double DischargedVolumeM3,
    double AppliedVolumeM3,
    int AffectedCellCount,
    IrrigationRadialProfileSource ProfileSource,
    string DistributionConfidenceLevelCode,
    string DistributionProfileStatus,
    Guid? DistributionProfilePubId,
    string? DistributionProfileDataSource,
    string PerformanceStatus,
    string? PerformanceDataQualityCode,
    string? PerformanceDataSource,
    decimal? SupportedPressureMinBar,
    decimal? SupportedPressureMaxBar,
    decimal? LowerSourcePressureBar,
    decimal? UpperSourcePressureBar,
    IReadOnlyList<Guid> SourcePerformancePubIds,
    IReadOnlyList<string> SourceDataQualityCodes,
    bool IsClippedByGrid);

/// <summary>
/// Cells is indexed as [row, column] and contains millimetres applied during the simulation.
/// TotalAppliedVolumeM3 is the water represented inside this grid, so it can be lower than the
/// discharged volume when a sprinkler footprint is clipped by the grid.
/// </summary>
public sealed record IrrigationSimulationResult(
    double GridOriginX,
    double GridOriginY,
    double CellSizeM,
    int Width,
    int Height,
    double MeanMm,
    double MinMm,
    double MaxMm,
    double TotalAppliedVolumeM3,
    double[,] Cells,
    IReadOnlyList<IrrigationHeadSimulationSummary> PerHeadSummary,
    IReadOnlyList<string> Warnings,
    IrrigationSimulationConfidence Confidence);

/// <summary>
/// Deterministic finite-grid precipitation calculator. Each head is evaluated only inside its
/// radius bounding box. Head volumes are normalized independently before their depths are summed,
/// which conserves discharged volume and preserves overlapping application.
/// </summary>
public sealed class IrrigationPrecipitationEngine
{
    public const double DefaultCellSizeM = 0.25d;
    public const int MaximumCellCount = 4_000_000;
    public const long MaximumBoundingBoxCellEvaluationsPerHead = 16_000_000;

    public IrrigationSimulationResult Simulate(IrrigationSimulationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Grid);
        ArgumentNullException.ThrowIfNull(request.Heads);

        ValidateGrid(request.Grid);
        var preparedHeads = PrepareHeads(request.Heads);
        var cells = new double[request.Grid.Height, request.Grid.Width];
        var summaries = new List<IrrigationHeadSimulationSummary>(preparedHeads.Count);
        var warnings = new List<string>();

        foreach (var preparedHead in preparedHeads)
        {
            // Per-head calculations have no shared mutable state and may be parallelized later.
            // Contributions are deliberately aggregated in stable input order for deterministic sums.
            var calculation = CalculateHead(request.Grid, preparedHead);
            foreach (var contribution in calculation.Contributions)
                cells[contribution.Row, contribution.Column] += contribution.DepthMm;

            var summary = calculation.Summary;
            summaries.Add(summary);

            if (summary.ProfileSource == IrrigationRadialProfileSource.GenericFallback)
            {
                warnings.Add(
                    $"Head '{summary.HeadName}' used the generic parabolic radial profile because its catalog profile status was '{summary.DistributionProfileStatus}'; its distribution is a C-level estimate, not measured pattern accuracy.");
            }
            else if (summary.DistributionConfidenceLevelCode == SprinklerDistributionConfidenceLevelCodes.GenericModeled)
                warnings.Add($"Head '{summary.HeadName}' used a C-level generic modeled catalog profile; it is not a measured distribution.");

            if (!string.IsNullOrWhiteSpace(preparedHead.Input.Performance.Warning))
                warnings.Add($"Head '{summary.HeadName}' performance: {preparedHead.Input.Performance.Warning}");
            if (!string.IsNullOrWhiteSpace(preparedHead.Input.DistributionProfile?.Warning) &&
                summary.ProfileSource != IrrigationRadialProfileSource.GenericFallback)
                warnings.Add($"Head '{summary.HeadName}' distribution profile: {preparedHead.Input.DistributionProfile.Warning}");

            if (summary.IsClippedByGrid)
                warnings.Add($"Head '{summary.HeadName}' discharged some water outside the simulation grid.");
        }

        if (preparedHeads.Count == 0)
            warnings.Add("No operating heads with a positive runtime were supplied.");

        var statistics = CalculateStatistics(cells, request.Grid.CellSizeM);
        var confidence = ResolveConfidence(preparedHeads);

        return new IrrigationSimulationResult(
            request.Grid.GridOriginX,
            request.Grid.GridOriginY,
            request.Grid.CellSizeM,
            request.Grid.Width,
            request.Grid.Height,
            statistics.MeanMm,
            statistics.MinMm,
            statistics.MaxMm,
            statistics.VolumeM3,
            cells,
            summaries.AsReadOnly(),
            warnings.AsReadOnly(),
            confidence);
    }

    static void ValidateGrid(IrrigationSimulationGrid grid)
    {
        if (!double.IsFinite(grid.GridOriginX) || !double.IsFinite(grid.GridOriginY))
            throw new ArgumentException("Grid origin coordinates must be finite.", nameof(grid));
        if (!double.IsFinite(grid.CellSizeM) || grid.CellSizeM <= 0d)
            throw new ArgumentOutOfRangeException(nameof(grid), "Grid cell size must be a finite positive number of metres.");
        if (grid.Width <= 0 || grid.Height <= 0)
            throw new ArgumentOutOfRangeException(nameof(grid), "Grid width and height must be positive cell counts.");

        var cellCount = checked((long)grid.Width * grid.Height);
        if (cellCount > MaximumCellCount)
            throw new ArgumentException(
                $"The grid contains {cellCount:N0} cells; the maximum is {MaximumCellCount:N0}.",
                nameof(grid));
    }

    static IReadOnlyList<PreparedHead> PrepareHeads(IReadOnlyList<IrrigationSimulationHead> sourceHeads)
    {
        if (sourceHeads.Count > 10_000)
            throw new ArgumentException("A simulation may contain at most 10,000 sprinkler heads.", nameof(sourceHeads));

        var headIds = new HashSet<Guid>();
        var prepared = new List<PreparedHead>(sourceHeads.Count);

        foreach (var input in sourceHeads)
        {
            if (input is null)
                throw new ArgumentException("Simulation heads cannot contain null entries.", nameof(sourceHeads));
            if (input.Head is null)
                throw new ArgumentException("Every simulation input requires a Digital Twin head.", nameof(sourceHeads));
            if (input.Head.PubId == Guid.Empty)
                throw new ArgumentException("Every simulation head requires a public identifier.", nameof(sourceHeads));
            if (!headIds.Add(input.Head.PubId))
                throw new ArgumentException($"Simulation head '{input.Head.PubId}' is duplicated.", nameof(sourceHeads));
            if (!double.IsFinite(input.RuntimeSeconds) || input.RuntimeSeconds < 0d)
                throw new ArgumentException($"Head '{input.Head.Name}' runtime must be a finite non-negative number of seconds.", nameof(sourceHeads));

            if (!input.IsOperating || input.RuntimeSeconds == 0d)
                continue;
            if (!input.Head.Active)
                throw new ArgumentException($"Inactive Digital Twin head '{input.Head.Name}' cannot be simulated as operating.", nameof(sourceHeads));
            if (!input.Head.MapX.HasValue || !input.Head.MapY.HasValue ||
                !double.IsFinite(input.Head.MapX.Value) || !double.IsFinite(input.Head.MapY.Value))
                throw new ArgumentException($"Head '{input.Head.Name}' requires finite local planar MapX and MapY coordinates.", nameof(sourceHeads));

            var arcDegrees = input.Head.ArcDegrees ?? 360m;
            if (arcDegrees <= 0m || arcDegrees > 360m)
                throw new ArgumentException($"Head '{input.Head.Name}' arc must be greater than 0 and at most 360 degrees.", nameof(sourceHeads));
            if (input.Head.OrientationDegrees is < 0m or > 360m)
                throw new ArgumentException($"Head '{input.Head.Name}' orientation must be between 0 and 360 degrees.", nameof(sourceHeads));
            if (!input.Head.SprinklerNozzlePubId.HasValue || input.Head.SprinklerNozzlePubId == Guid.Empty)
                throw new ArgumentException($"Head '{input.Head.Name}' requires a Digital Twin nozzle reference.", nameof(sourceHeads));
            if (input.Performance is null)
                throw new ArgumentException($"Head '{input.Head.Name}' requires resolved nozzle performance data.", nameof(sourceHeads));
            if (input.Performance.NozzlePubId != input.Head.SprinklerNozzlePubId.Value)
                throw new ArgumentException($"Head '{input.Head.Name}' performance belongs to a different nozzle.", nameof(sourceHeads));

            ValidatePerformance(input.Head.Name, input.Performance, sourceHeads);
            var profile = PrepareProfile(input.Head.Name, input.Performance, input.DistributionProfile, sourceHeads);
            prepared.Add(new PreparedHead(input, profile));
        }

        return prepared.AsReadOnly();
    }

    static void ValidatePerformance(
        string headName,
        SprinklerPerformanceResult performance,
        IReadOnlyList<IrrigationSimulationHead> sourceHeads)
    {
        if (performance.NozzlePubId == Guid.Empty)
            throw new ArgumentException($"Head '{headName}' resolved performance requires a nozzle identifier.", nameof(sourceHeads));
        if (performance.RequestedPressureBar <= 0m)
            throw new ArgumentException($"Head '{headName}' requested pressure must be a positive number of bar.", nameof(sourceHeads));
        if (!performance.Supported)
            throw new ArgumentException(
                $"Head '{headName}' cannot be simulated because performance status '{performance.InterpolationStatus}' is unsupported.",
                nameof(sourceHeads));
        if (!performance.FlowM3H.HasValue || performance.FlowM3H <= 0m)
            throw new ArgumentException($"Head '{headName}' resolved flow must be a positive number of cubic metres per hour.", nameof(sourceHeads));
        if (!performance.RadiusM.HasValue || performance.RadiusM <= 0m)
            throw new ArgumentException($"Head '{headName}' resolved radius must be a positive number of metres.", nameof(sourceHeads));
        if (string.IsNullOrWhiteSpace(performance.InterpolationStatus))
            throw new ArgumentException($"Head '{headName}' resolved performance requires an interpolation status.", nameof(sourceHeads));
        if (!SprinklerPerformanceDataQualityCodes.IsValid(performance.DataQualityCode))
            throw new ArgumentException($"Head '{headName}' resolved performance has an invalid data-quality code.", nameof(sourceHeads));
    }

    static PreparedProfile PrepareProfile(
        string headName,
        SprinklerPerformanceResult performance,
        SprinklerDistributionProfileResult? source,
        IReadOnlyList<IrrigationSimulationHead> sourceHeads)
    {
        if (source is null)
            return PreparedProfile.GenericFallback(DistributionProfileStatuses.NoProfileData);
        if (source.NozzlePubId != performance.NozzlePubId)
            throw new ArgumentException($"Head '{headName}' distribution profile belongs to a different nozzle.", nameof(sourceHeads));
        if (source.RequestedPressureBar != performance.RequestedPressureBar)
            throw new ArgumentException($"Head '{headName}' distribution profile was requested at a different pressure.", nameof(sourceHeads));
        if (!string.Equals(source.Status, DistributionProfileStatuses.Exact, StringComparison.Ordinal))
            return PreparedProfile.GenericFallback(source.Status);
        if (source.Points is null || source.Points.Count < 2)
            throw new ArgumentException($"Head '{headName}' exact radial profile requires at least two points.", nameof(sourceHeads));
        if (!source.ProfilePressureBar.HasValue || source.ProfilePressureBar != performance.RequestedPressureBar)
            throw new ArgumentException($"Head '{headName}' exact radial profile pressure does not match the resolved performance pressure.", nameof(sourceHeads));
        if (!SprinklerDistributionConfidenceLevelCodes.IsValid(source.ConfidenceLevelCode))
            throw new ArgumentException($"Head '{headName}' exact radial profile has an invalid A/B/C confidence code.", nameof(sourceHeads));

        var points = source.Points.ToArray();
        if (points.Any(point => point is null))
            throw new ArgumentException($"Head '{headName}' radial profile cannot contain null points.", nameof(sourceHeads));
        if (points[0].NormalizedDistance != 0m || points[^1].NormalizedDistance != 1m)
            throw new ArgumentException($"Head '{headName}' radial profile must span normalized distances 0 through 1.", nameof(sourceHeads));

        var hasPositiveRate = false;
        for (var index = 0; index < points.Length; index++)
        {
            var point = points[index];
            if (point.NormalizedDistance < 0m || point.NormalizedDistance > 1m)
                throw new ArgumentException($"Head '{headName}' radial profile distances must be from 0 through 1.", nameof(sourceHeads));
            if (point.RelativeApplication < 0m || point.RelativeApplication > 1m)
                throw new ArgumentException($"Head '{headName}' normalized radial profile rates must be from 0 through 1.", nameof(sourceHeads));
            if (index > 0 && point.NormalizedDistance <= points[index - 1].NormalizedDistance)
                throw new ArgumentException($"Head '{headName}' radial profile distances must be strictly increasing.", nameof(sourceHeads));

            hasPositiveRate |= point.RelativeApplication > 0m;
        }

        if (!hasPositiveRate)
            throw new ArgumentException($"Head '{headName}' radial profile must contain a positive application rate.", nameof(sourceHeads));

        return new PreparedProfile(
            points,
            IrrigationRadialProfileSource.CatalogProfile,
            SprinklerDistributionConfidenceLevelCodes.Normalize(source.ConfidenceLevelCode!),
            source.Status,
            source.ProfilePubId,
            source.DataSource);
    }

    static HeadCalculation CalculateHead(
        IrrigationSimulationGrid grid,
        PreparedHead preparedHead)
    {
        var input = preparedHead.Input;
        var head = input.Head;
        var performance = input.Performance;
        var flowM3H = (double)performance.FlowM3H!.Value;
        var radiusM = (double)performance.RadiusM!.Value;
        var positionX = head.MapX!.Value;
        var positionY = head.MapY!.Value;
        var arcDegrees = (double)(head.ArcDegrees ?? 360m);
        var orientationDegrees = (double)(head.OrientationDegrees ?? 0m);
        var dischargedVolumeM3 = flowM3H * input.RuntimeSeconds / 3600d;
        if (!double.IsFinite(dischargedVolumeM3))
            throw new ArgumentException($"Head '{head.Name}' discharged volume exceeds the supported numeric range.");

        var bounds = GetBoundingCellRange(grid, head.Name, positionX, positionY, radiusM);
        var evaluationCount = checked(bounds.ColumnCount * bounds.RowCount);
        if (evaluationCount > MaximumBoundingBoxCellEvaluationsPerHead)
            throw new ArgumentException(
                $"Head '{head.Name}' requires {evaluationCount:N0} bounded cell evaluations; the maximum per head is {MaximumBoundingBoxCellEvaluationsPerHead:N0}.");

        var inGridWeights = new List<CellWeight>();
        var totalWeight = new CompensatedSum();
        var hasWeightOutsideGrid = false;
        var radiusSquared = radiusM * radiusM;

        for (var row = bounds.MinimumRow; row <= bounds.MaximumRow; row++)
        {
            var cellCenterY = grid.GridOriginY + ((row + 0.5d) * grid.CellSizeM);
            var deltaY = cellCenterY - positionY;

            for (var column = bounds.MinimumColumn; column <= bounds.MaximumColumn; column++)
            {
                var cellCenterX = grid.GridOriginX + ((column + 0.5d) * grid.CellSizeM);
                var deltaX = cellCenterX - positionX;
                var distanceSquared = (deltaX * deltaX) + (deltaY * deltaY);
                if (distanceSquared > radiusSquared || !IsInsideArc(deltaX, deltaY, arcDegrees, orientationDegrees))
                    continue;

                var radiusFraction = Math.Sqrt(distanceSquared) / radiusM;
                var weight = preparedHead.Profile.Evaluate(radiusFraction);
                if (weight <= 0d)
                    continue;

                totalWeight.Add(weight);
                if (row >= 0 && row < grid.Height && column >= 0 && column < grid.Width)
                    inGridWeights.Add(new CellWeight((int)row, (int)column, weight));
                else
                    hasWeightOutsideGrid = true;
            }
        }

        if (totalWeight.Value <= 0d)
            throw new InvalidOperationException(
                $"Head '{head.Name}' did not intersect a positive profile sample. Use a finer grid or verify its radial profile.");

        var cellAreaM2 = grid.CellSizeM * grid.CellSizeM;
        var appliedVolume = new CompensatedSum();
        var contributions = new List<CellContribution>(inGridWeights.Count);
        foreach (var cellWeight in inGridWeights)
        {
            var cellVolumeM3 = dischargedVolumeM3 * (cellWeight.Weight / totalWeight.Value);
            contributions.Add(new CellContribution(
                cellWeight.Row,
                cellWeight.Column,
                cellVolumeM3 * 1000d / cellAreaM2));
            appliedVolume.Add(cellVolumeM3);
        }

        var summary = new IrrigationHeadSimulationSummary(
            head.PubId,
            head.Name,
            performance.NozzlePubId,
            performance.RequestedPressureBar,
            flowM3H,
            input.RuntimeSeconds,
            dischargedVolumeM3,
            appliedVolume.Value,
            inGridWeights.Count,
            preparedHead.Profile.Source,
            preparedHead.Profile.ConfidenceLevelCode,
            preparedHead.Profile.Status,
            preparedHead.Profile.ProfilePubId,
            preparedHead.Profile.DataSource,
            performance.InterpolationStatus,
            performance.DataQualityCode,
            performance.DataSource,
            performance.SupportedPressureMinBar,
            performance.SupportedPressureMaxBar,
            performance.LowerSourcePressureBar,
            performance.UpperSourcePressureBar,
            performance.SourcePerformancePubIds?.ToArray() ?? [],
            performance.SourceDataQualityCodes?.ToArray() ?? [],
            hasWeightOutsideGrid);

        return new HeadCalculation(summary, contributions.AsReadOnly());
    }

    static CellRange GetBoundingCellRange(
        IrrigationSimulationGrid grid,
        string headName,
        double positionX,
        double positionY,
        double radiusM)
    {
        var minimumColumn = CeilingToLong(((positionX - radiusM - grid.GridOriginX) / grid.CellSizeM) - 0.5d, headName);
        var maximumColumn = FloorToLong(((positionX + radiusM - grid.GridOriginX) / grid.CellSizeM) - 0.5d, headName);
        var minimumRow = CeilingToLong(((positionY - radiusM - grid.GridOriginY) / grid.CellSizeM) - 0.5d, headName);
        var maximumRow = FloorToLong(((positionY + radiusM - grid.GridOriginY) / grid.CellSizeM) - 0.5d, headName);

        return new CellRange(minimumColumn, maximumColumn, minimumRow, maximumRow);
    }

    static long CeilingToLong(double value, string headName)
    {
        var rounded = Math.Ceiling(value);
        if (!double.IsFinite(rounded) || rounded < long.MinValue || rounded > long.MaxValue)
            throw new ArgumentException($"Head '{headName}' bounding coordinates exceed the supported numeric range.");
        return (long)rounded;
    }

    static long FloorToLong(double value, string headName)
    {
        var rounded = Math.Floor(value);
        if (!double.IsFinite(rounded) || rounded < long.MinValue || rounded > long.MaxValue)
            throw new ArgumentException($"Head '{headName}' bounding coordinates exceed the supported numeric range.");
        return (long)rounded;
    }

    static bool IsInsideArc(double deltaX, double deltaY, double arcDegrees, double orientationDegrees)
    {
        if (arcDegrees >= 360d)
            return true;

        var pointAngle = Math.Atan2(deltaY, deltaX) * 180d / Math.PI;
        var angularDifference = NormalizeSignedDegrees(pointAngle - orientationDegrees);
        return Math.Abs(angularDifference) <= (arcDegrees / 2d) + 1e-12;
    }

    static double NormalizeSignedDegrees(double angle)
    {
        var normalized = angle % 360d;
        if (normalized > 180d)
            normalized -= 360d;
        else if (normalized < -180d)
            normalized += 360d;
        return normalized;
    }

    static SimulationStatistics CalculateStatistics(double[,] cells, double cellSizeM)
    {
        var totalDepth = new CompensatedSum();
        var minimum = double.PositiveInfinity;
        var maximum = double.NegativeInfinity;

        foreach (var depthMm in cells)
        {
            totalDepth.Add(depthMm);
            minimum = Math.Min(minimum, depthMm);
            maximum = Math.Max(maximum, depthMm);
        }

        var cellCount = cells.LongLength;
        var mean = totalDepth.Value / cellCount;
        var volumeM3 = totalDepth.Value * cellSizeM * cellSizeM * 0.001d;
        return new SimulationStatistics(mean, minimum, maximum, volumeM3);
    }

    static IrrigationSimulationConfidence ResolveConfidence(IReadOnlyList<PreparedHead> heads)
    {
        if (heads.Count == 0)
            return IrrigationSimulationConfidence.NoWaterApplied;

        return heads.Min(x => x.Profile.Confidence);
    }

    readonly record struct PreparedHead(IrrigationSimulationHead Input, PreparedProfile Profile);

    readonly record struct PreparedProfile(
        SprinklerDistributionPointDto[]? Points,
        IrrigationRadialProfileSource Source,
        string ConfidenceLevelCode,
        string Status,
        Guid? ProfilePubId,
        string? DataSource)
    {
        public IrrigationSimulationConfidence Confidence => ConfidenceLevelCode switch
        {
            SprinklerDistributionConfidenceLevelCodes.Measured => IrrigationSimulationConfidence.Measured,
            SprinklerDistributionConfidenceLevelCodes.ManufacturerDerived => IrrigationSimulationConfidence.ManufacturerDerived,
            _ => IrrigationSimulationConfidence.GenericModeled
        };

        public static PreparedProfile GenericFallback(string? status)
            => new(
                null,
                IrrigationRadialProfileSource.GenericFallback,
                SprinklerDistributionConfidenceLevelCodes.GenericModeled,
                string.IsNullOrWhiteSpace(status) ? DistributionProfileStatuses.NoProfileData : status,
                null,
                null);

        public double Evaluate(double radiusFraction)
        {
            if (radiusFraction < 0d || radiusFraction > 1d)
                return 0d;

            // The generic curve is a parabolic radial estimate: f(r) = 1 - (r/R)^2.
            if (Points is null)
                return Math.Max(0d, 1d - (radiusFraction * radiusFraction));

            for (var index = 1; index < Points.Length; index++)
            {
                var right = Points[index];
                var rightDistance = (double)right.NormalizedDistance;
                if (radiusFraction > rightDistance)
                    continue;

                var left = Points[index - 1];
                var leftDistance = (double)left.NormalizedDistance;
                var leftApplication = (double)left.RelativeApplication;
                var rightApplication = (double)right.RelativeApplication;
                var fraction = (radiusFraction - leftDistance) / (rightDistance - leftDistance);
                return leftApplication + (fraction * (rightApplication - leftApplication));
            }

            return (double)Points[^1].RelativeApplication;
        }
    }

    readonly record struct CellWeight(int Row, int Column, double Weight);

    readonly record struct CellContribution(int Row, int Column, double DepthMm);

    readonly record struct HeadCalculation(
        IrrigationHeadSimulationSummary Summary,
        IReadOnlyList<CellContribution> Contributions);

    readonly record struct CellRange(
        long MinimumColumn,
        long MaximumColumn,
        long MinimumRow,
        long MaximumRow)
    {
        public long ColumnCount => checked(MaximumColumn - MinimumColumn + 1L);
        public long RowCount => checked(MaximumRow - MinimumRow + 1L);
    }

    readonly record struct SimulationStatistics(
        double MeanMm,
        double MinMm,
        double MaxMm,
        double VolumeM3);

    struct CompensatedSum
    {
        double _sum;
        double _compensation;

        public double Value => _sum;

        public void Add(double value)
        {
            var adjusted = value - _compensation;
            var next = _sum + adjusted;
            _compensation = (next - _sum) - adjusted;
            _sum = next;
        }
    }
}
