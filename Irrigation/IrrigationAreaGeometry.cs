namespace AmsRecords.Irrigation;

/// <summary>
/// Deterministic planar geometry for irrigation target areas. Coordinates are metres in the
/// course-local coordinate system; latitude and longitude are deliberately not supported here.
/// </summary>
public static class IrrigationAreaGeometry
{
    public const double MaximumAbsoluteCoordinateM = 100_000d;
    public const int MaximumPointCount = 2_000;

    const double Epsilon = 1e-9;

    public static IReadOnlyList<string> Validate(IReadOnlyList<IrrigationPlanarPoint>? points)
    {
        var errors = new List<string>();
        if (points is null)
        {
            errors.Add("A boundary is required.");
            return errors;
        }

        if (points.Count > MaximumPointCount)
            errors.Add($"A boundary may contain at most {MaximumPointCount:N0} points.");

        if (points.Any(point => !double.IsFinite(point.X) || !double.IsFinite(point.Y)))
            errors.Add("Boundary coordinates must be finite numbers.");

        if (points.Any(point =>
                double.IsFinite(point.X) && double.IsFinite(point.Y) &&
                (Math.Abs(point.X) > MaximumAbsoluteCoordinateM || Math.Abs(point.Y) > MaximumAbsoluteCoordinateM)))
            errors.Add($"Boundary coordinates must stay within {MaximumAbsoluteCoordinateM:N0} m of the local course origin.");

        var finitePoints = points
            .Where(point => double.IsFinite(point.X) && double.IsFinite(point.Y))
            .ToArray();
        if (finitePoints.Distinct().Count() < 3)
            errors.Add("A boundary needs at least three distinct points.");

        if (points.Count >= 2 && Enumerable.Range(0, points.Count).Any(index =>
                points[index] == points[(index + 1) % points.Count]))
            errors.Add("Adjacent boundary points must not be duplicates; the engine closes the polygon automatically.");

        if (errors.Count > 0 || points.Count < 3)
            return errors;

        var signedDoubleArea = SignedDoubleArea(points);
        if (Math.Abs(signedDoubleArea) <= Epsilon)
            errors.Add("Boundary points must form a polygon with a non-zero area.");

        if (HasSelfIntersection(points))
            errors.Add("Boundary edges must not cross or overlap.");

        return errors;
    }

    public static IrrigationAreaPolygon Create(IReadOnlyList<IrrigationPlanarPoint> points)
    {
        var errors = Validate(points);
        if (errors.Count > 0)
            throw new ArgumentException(string.Join(" ", errors), nameof(points));

        return new IrrigationAreaPolygon(points.ToArray(), CalculateMetricsUnchecked(points));
    }

    public static IrrigationAreaGeometryMetrics CalculateMetrics(IReadOnlyList<IrrigationPlanarPoint> points)
    {
        var errors = Validate(points);
        if (errors.Count > 0)
            throw new ArgumentException(string.Join(" ", errors), nameof(points));

        return CalculateMetricsUnchecked(points);
    }

    static IrrigationAreaGeometryMetrics CalculateMetricsUnchecked(IReadOnlyList<IrrigationPlanarPoint> points)
    {
        var signedDoubleArea = SignedDoubleArea(points);
        var centroidXNumerator = 0d;
        var centroidYNumerator = 0d;
        var perimeterM = 0d;
        for (var index = 0; index < points.Count; index++)
        {
            var current = points[index];
            var next = points[(index + 1) % points.Count];
            var cross = current.X * next.Y - next.X * current.Y;
            centroidXNumerator += (current.X + next.X) * cross;
            centroidYNumerator += (current.Y + next.Y) * cross;
            var deltaX = next.X - current.X;
            var deltaY = next.Y - current.Y;
            perimeterM += Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        var minX = points.Min(point => point.X);
        var minY = points.Min(point => point.Y);
        var maxX = points.Max(point => point.X);
        var maxY = points.Max(point => point.Y);
        return new IrrigationAreaGeometryMetrics(
            AreaM2: Math.Abs(signedDoubleArea) / 2d,
            PerimeterM: perimeterM,
            BoundingBox: new IrrigationBoundingBox(minX, minY, maxX, maxY),
            Centroid: new IrrigationPlanarPoint(
                centroidXNumerator / (3d * signedDoubleArea),
                centroidYNumerator / (3d * signedDoubleArea)));
    }

    static double SignedDoubleArea(IReadOnlyList<IrrigationPlanarPoint> points)
    {
        var result = 0d;
        for (var index = 0; index < points.Count; index++)
        {
            var current = points[index];
            var next = points[(index + 1) % points.Count];
            result += current.X * next.Y - next.X * current.Y;
        }

        return result;
    }

    static bool HasSelfIntersection(IReadOnlyList<IrrigationPlanarPoint> points)
    {
        for (var first = 0; first < points.Count; first++)
        {
            var firstNext = (first + 1) % points.Count;
            for (var second = first + 1; second < points.Count; second++)
            {
                var secondNext = (second + 1) % points.Count;
                if (first == second || firstNext == second || secondNext == first)
                    continue;

                if (SegmentsIntersect(points[first], points[firstNext], points[second], points[secondNext]))
                    return true;
            }
        }

        return false;
    }

    static bool SegmentsIntersect(
        IrrigationPlanarPoint firstStart,
        IrrigationPlanarPoint firstEnd,
        IrrigationPlanarPoint secondStart,
        IrrigationPlanarPoint secondEnd)
    {
        var o1 = Orientation(firstStart, firstEnd, secondStart);
        var o2 = Orientation(firstStart, firstEnd, secondEnd);
        var o3 = Orientation(secondStart, secondEnd, firstStart);
        var o4 = Orientation(secondStart, secondEnd, firstEnd);

        if (o1 != o2 && o3 != o4)
            return true;

        return o1 == 0 && IsOnSegment(firstStart, secondStart, firstEnd) ||
               o2 == 0 && IsOnSegment(firstStart, secondEnd, firstEnd) ||
               o3 == 0 && IsOnSegment(secondStart, firstStart, secondEnd) ||
               o4 == 0 && IsOnSegment(secondStart, firstEnd, secondEnd);
    }

    static int Orientation(IrrigationPlanarPoint start, IrrigationPlanarPoint end, IrrigationPlanarPoint point)
    {
        var cross = (end.X - start.X) * (point.Y - start.Y) -
                    (end.Y - start.Y) * (point.X - start.X);
        return Math.Abs(cross) <= Epsilon ? 0 : cross > 0d ? 1 : -1;
    }

    static bool IsOnSegment(IrrigationPlanarPoint start, IrrigationPlanarPoint point, IrrigationPlanarPoint end)
        => point.X >= Math.Min(start.X, end.X) - Epsilon &&
           point.X <= Math.Max(start.X, end.X) + Epsilon &&
           point.Y >= Math.Min(start.Y, end.Y) - Epsilon &&
           point.Y <= Math.Max(start.Y, end.Y) + Epsilon;

    internal static bool PointIsOnSegment(
        IrrigationPlanarPoint point,
        IrrigationPlanarPoint start,
        IrrigationPlanarPoint end)
        => Orientation(start, end, point) == 0 && IsOnSegment(start, point, end);
}

public readonly record struct IrrigationPlanarPoint(double X, double Y);

public readonly record struct IrrigationBoundingBox(double MinX, double MinY, double MaxX, double MaxY)
{
    public bool Contains(double x, double y)
        => x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
}

public readonly record struct IrrigationAreaGeometryMetrics(
    double AreaM2,
    double PerimeterM,
    IrrigationBoundingBox BoundingBox,
    IrrigationPlanarPoint Centroid);

/// <summary>A validated, immutable polygon compiled once for repeated point and grid queries.</summary>
public sealed class IrrigationAreaPolygon
{
    readonly IrrigationPlanarPoint[] _points;

    internal IrrigationAreaPolygon(
        IrrigationPlanarPoint[] points,
        IrrigationAreaGeometryMetrics metrics)
    {
        _points = points;
        Metrics = metrics;
    }

    public IReadOnlyList<IrrigationPlanarPoint> Points => _points;
    public IrrigationAreaGeometryMetrics Metrics { get; }

    public bool ContainsPoint(double x, double y)
    {
        if (!double.IsFinite(x) || !double.IsFinite(y) || !Metrics.BoundingBox.Contains(x, y))
            return false;

        var point = new IrrigationPlanarPoint(x, y);
        var inside = false;
        for (int currentIndex = 0, previousIndex = _points.Length - 1;
             currentIndex < _points.Length;
             previousIndex = currentIndex++)
        {
            var current = _points[currentIndex];
            var previous = _points[previousIndex];
            if (IrrigationAreaGeometry.PointIsOnSegment(point, previous, current))
                return true;

            if ((current.Y > y) != (previous.Y > y) &&
                x < (previous.X - current.X) * (y - current.Y) / (previous.Y - current.Y) + current.X)
                inside = !inside;
        }

        return inside;
    }

    public IrrigationAreaGridMask CreateGridMask(IrrigationGridDefinition grid)
    {
        grid.Validate();
        var cells = new bool[checked(grid.ColumnCount * grid.RowCount)];
        var firstColumn = Math.Max(0, (int)Math.Ceiling(
            (Metrics.BoundingBox.MinX - grid.OriginX) / grid.CellWidthM - 0.5d));
        var lastColumn = Math.Min(grid.ColumnCount - 1, (int)Math.Floor(
            (Metrics.BoundingBox.MaxX - grid.OriginX) / grid.CellWidthM - 0.5d));
        var firstRow = Math.Max(0, (int)Math.Ceiling(
            (Metrics.BoundingBox.MinY - grid.OriginY) / grid.CellHeightM - 0.5d));
        var lastRow = Math.Min(grid.RowCount - 1, (int)Math.Floor(
            (Metrics.BoundingBox.MaxY - grid.OriginY) / grid.CellHeightM - 0.5d));

        var targetCellCount = 0;
        if (firstColumn <= lastColumn && firstRow <= lastRow)
        {
            var intersections = new List<double>(_points.Length);

            void IncludeCell(int row, int column)
            {
                var index = row * grid.ColumnCount + column;
                if (cells[index])
                    return;
                cells[index] = true;
                targetCellCount++;
            }

            void IncludeRange(int row, double leftX, double rightX)
            {
                var minimumX = Math.Min(leftX, rightX);
                var maximumX = Math.Max(leftX, rightX);
                var minimumColumn = Math.Max(firstColumn, (int)Math.Ceiling(
                    (minimumX - grid.OriginX) / grid.CellWidthM - 0.5d));
                var maximumColumn = Math.Min(lastColumn, (int)Math.Floor(
                    (maximumX - grid.OriginX) / grid.CellWidthM - 0.5d));
                for (var column = minimumColumn; column <= maximumColumn; column++)
                    IncludeCell(row, column);
            }

            void IncludeBoundaryIntersection(int row, double x)
            {
                var column = (int)Math.Round(
                    (x - grid.OriginX) / grid.CellWidthM - 0.5d,
                    MidpointRounding.ToEven);
                if (column < firstColumn || column > lastColumn)
                    return;
                var centerX = grid.OriginX + (column + 0.5d) * grid.CellWidthM;
                if (Math.Abs(centerX - x) <= 1e-9)
                    IncludeCell(row, column);
            }

            for (var row = firstRow; row <= lastRow; row++)
            {
                var y = grid.OriginY + (row + 0.5d) * grid.CellHeightM;
                intersections.Clear();
                for (var edgeIndex = 0; edgeIndex < _points.Length; edgeIndex++)
                {
                    var start = _points[edgeIndex];
                    var end = _points[(edgeIndex + 1) % _points.Length];
                    if (Math.Abs(start.Y - end.Y) <= 1e-9)
                    {
                        if (Math.Abs(y - start.Y) <= 1e-9)
                            IncludeRange(row, start.X, end.X);
                        continue;
                    }

                    if ((start.Y > y) != (end.Y > y))
                        intersections.Add(start.X + (y - start.Y) * (end.X - start.X) / (end.Y - start.Y));

                    if (y >= Math.Min(start.Y, end.Y) - 1e-9 &&
                        y <= Math.Max(start.Y, end.Y) + 1e-9)
                    {
                        var boundaryX = start.X + (y - start.Y) * (end.X - start.X) / (end.Y - start.Y);
                        IncludeBoundaryIntersection(row, boundaryX);
                    }
                }

                intersections.Sort();
                for (var index = 0; index + 1 < intersections.Count; index += 2)
                    IncludeRange(row, intersections[index], intersections[index + 1]);
            }
        }

        return new IrrigationAreaGridMask(grid, cells, targetCellCount);
    }

    public IrrigationAreaGridMask CreateGridMask(IrrigationSimulationGrid grid)
    {
        ArgumentNullException.ThrowIfNull(grid);
        return CreateGridMask(new IrrigationGridDefinition(
            grid.GridOriginX,
            grid.GridOriginY,
            grid.Width,
            grid.Height,
            grid.CellSizeM,
            grid.CellSizeM));
    }

    /// <summary>
    /// Masks a precipitation result in the same local planar coordinate space and classifies all
    /// water represented by its finite grid. Water discharged beyond that finite grid remains the
    /// precipitation engine's clipped-volume concern.
    /// </summary>
    public IrrigationAreaSimulationMaskResult MaskSimulation(IrrigationSimulationResult simulation)
    {
        ArgumentNullException.ThrowIfNull(simulation);
        var grid = new IrrigationSimulationGrid(
            simulation.GridOriginX,
            simulation.GridOriginY,
            simulation.Width,
            simulation.Height,
            simulation.CellSizeM);
        var mask = CreateGridMask(grid);
        return new IrrigationAreaSimulationMaskResult(mask, mask.CalculateWaterMetrics(simulation));
    }
}

public readonly record struct IrrigationGridDefinition(
    double OriginX,
    double OriginY,
    int ColumnCount,
    int RowCount,
    double CellWidthM,
    double CellHeightM)
{
    public double CellAreaM2 => CellWidthM * CellHeightM;

    internal void Validate()
    {
        if (!double.IsFinite(OriginX) || !double.IsFinite(OriginY))
            throw new ArgumentOutOfRangeException(nameof(OriginX), "Grid origin coordinates must be finite.");
        if (ColumnCount <= 0 || RowCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(ColumnCount), "Grid dimensions must be positive.");
        if (!double.IsFinite(CellWidthM) || !double.IsFinite(CellHeightM) ||
            CellWidthM <= 0d || CellHeightM <= 0d)
            throw new ArgumentOutOfRangeException(nameof(CellWidthM), "Grid cell dimensions must be finite and positive.");
        var cellCount = checked((long)ColumnCount * RowCount);
        if (cellCount > IrrigationPrecipitationEngine.MaximumCellCount)
            throw new ArgumentOutOfRangeException(
                nameof(ColumnCount),
                $"The grid contains {cellCount:N0} cells; the maximum is {IrrigationPrecipitationEngine.MaximumCellCount:N0}.");
    }
}

public sealed class IrrigationAreaGridMask
{
    readonly bool[] _cells;

    internal IrrigationAreaGridMask(IrrigationGridDefinition grid, bool[] cells, int targetCellCount)
    {
        Grid = grid;
        _cells = cells;
        TargetCellCount = targetCellCount;
    }

    public IrrigationGridDefinition Grid { get; }
    public int CellCount => _cells.Length;
    public int TargetCellCount { get; }
    public double ApproximateTargetAreaM2 => TargetCellCount * Grid.CellAreaM2;
    public double ApproximateOutsideTargetAreaM2 => (CellCount - TargetCellCount) * Grid.CellAreaM2;
    public IReadOnlyList<bool> Cells => _cells;

    public bool IsWithinTarget(int row, int column)
    {
        if (row < 0 || row >= Grid.RowCount || column < 0 || column >= Grid.ColumnCount)
            throw new ArgumentOutOfRangeException(nameof(row));
        return _cells[row * Grid.ColumnCount + column];
    }

    /// <summary>
    /// Classifies applied depth at each grid-cell centre. Outside-target water is intentionally
    /// neutral terminology: it may serve a neighbouring collar or surround.
    /// </summary>
    public IrrigationAreaWaterMetrics CalculateWaterMetrics(IReadOnlyList<double> appliedDepthMm)
    {
        ArgumentNullException.ThrowIfNull(appliedDepthMm);
        if (appliedDepthMm.Count != _cells.Length)
            throw new ArgumentException("Applied-depth values must match the grid cell count.", nameof(appliedDepthMm));

        var targetAreaVolumeM3 = 0d;
        var outsideTargetAreaVolumeM3 = 0d;
        for (var index = 0; index < appliedDepthMm.Count; index++)
        {
            var depthMm = appliedDepthMm[index];
            if (!double.IsFinite(depthMm) || depthMm < 0d)
                throw new ArgumentOutOfRangeException(nameof(appliedDepthMm), "Applied depth must be finite and non-negative.");

            var volumeM3 = depthMm / 1_000d * Grid.CellAreaM2;
            if (_cells[index])
                targetAreaVolumeM3 += volumeM3;
            else
                outsideTargetAreaVolumeM3 += volumeM3;
        }

        var allAppliedVolumeM3 = targetAreaVolumeM3 + outsideTargetAreaVolumeM3;
        return new IrrigationAreaWaterMetrics(
            targetAreaVolumeM3,
            outsideTargetAreaVolumeM3,
            allAppliedVolumeM3,
            allAppliedVolumeM3 <= 0d ? 0d : targetAreaVolumeM3 / allAppliedVolumeM3 * 100d);
    }

    public IrrigationAreaWaterMetrics CalculateWaterMetrics(IrrigationSimulationResult simulation)
    {
        ArgumentNullException.ThrowIfNull(simulation);
        if (simulation.Width != Grid.ColumnCount || simulation.Height != Grid.RowCount ||
            simulation.GridOriginX != Grid.OriginX || simulation.GridOriginY != Grid.OriginY ||
            simulation.CellSizeM != Grid.CellWidthM || simulation.CellSizeM != Grid.CellHeightM)
            throw new ArgumentException("The precipitation result does not match this area mask's grid.", nameof(simulation));
        if (simulation.Cells.GetLength(0) != Grid.RowCount || simulation.Cells.GetLength(1) != Grid.ColumnCount)
            throw new ArgumentException("The precipitation cell array does not match its declared grid dimensions.", nameof(simulation));

        var targetAreaVolumeM3 = 0d;
        var outsideTargetAreaVolumeM3 = 0d;
        for (var row = 0; row < Grid.RowCount; row++)
        for (var column = 0; column < Grid.ColumnCount; column++)
        {
            var depthMm = simulation.Cells[row, column];
            if (!double.IsFinite(depthMm) || depthMm < 0d)
                throw new ArgumentOutOfRangeException(nameof(simulation), "Applied depth must be finite and non-negative.");

            var volumeM3 = depthMm / 1_000d * Grid.CellAreaM2;
            if (IsWithinTarget(row, column))
                targetAreaVolumeM3 += volumeM3;
            else
                outsideTargetAreaVolumeM3 += volumeM3;
        }

        var allAppliedVolumeM3 = targetAreaVolumeM3 + outsideTargetAreaVolumeM3;
        return new IrrigationAreaWaterMetrics(
            targetAreaVolumeM3,
            outsideTargetAreaVolumeM3,
            allAppliedVolumeM3,
            allAppliedVolumeM3 <= 0d ? 0d : targetAreaVolumeM3 / allAppliedVolumeM3 * 100d);
    }
}

public readonly record struct IrrigationAreaWaterMetrics(
    double TargetAreaVolumeM3,
    double OutsideTargetAreaVolumeM3,
    double AllAppliedVolumeM3,
    double TargetEfficiencyPercent);

public sealed record IrrigationAreaSimulationMaskResult(
    IrrigationAreaGridMask Mask,
    IrrigationAreaWaterMetrics WaterMetrics);
