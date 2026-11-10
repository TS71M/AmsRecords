using static AmsRecords.Tdr.TdrMeasurementDtos;

namespace AmsRecords.Tdr;

public static class TdrDailyMoistureAnalytics
{
    public static string Classify(decimal average, decimal dryThreshold, decimal wetThreshold)
        => average < dryThreshold ? "dry" : average > wetThreshold ? "wet" : "ideal";

    public sealed record AssignedReading(Guid? SurfacePubId, DateTimeOffset CapturedAtUtc, decimal? VwcPercent);
    public sealed record DailyAverage(
        Guid SurfacePubId,
        string SurfaceLabel,
        DateOnly Day,
        decimal AverageVwc,
        int ReadingCount,
        decimal DryVwcThreshold,
        decimal WetVwcThreshold);

    public sealed record SurfaceSummary(
        Guid SurfacePubId,
        string SurfaceLabel,
        decimal LatestDailyAverage,
        DateOnly LatestDay,
        decimal PeriodDailyAverage,
        int ReadingCount,
        int MeasuredDayCount,
        decimal DryVwcThreshold,
        decimal WetVwcThreshold);

    public sealed record Result(
        IReadOnlyList<DailyAverage> DailyAverages,
        IReadOnlyList<SurfaceSummary> SurfaceSummaries,
        DateOnly? LatestDay,
        decimal? LatestFieldDailyAverage,
        int AssignedReadingCount,
        int MeasuredDayCount)
    {
        public static Result Empty { get; } = new([], [], null, null, 0, 0);
    }

    public static Result Build(
        IEnumerable<TimelineItem> measurements,
        IEnumerable<TimelineSurface> surfaces)
        => BuildAssigned(measurements.Select(x => new AssignedReading(x.SurfacePubId, x.CapturedAtUtc, x.VwcPercent)), surfaces);

    public static Result BuildAssigned(
        IEnumerable<AssignedReading> measurements,
        IEnumerable<TimelineSurface> surfaces)
    {
        var surfaceById = surfaces.ToDictionary(x => x.SurfacePubId);
        var assignedReadings = measurements
            .Where(x => x.SurfacePubId.HasValue && x.VwcPercent.HasValue && surfaceById.ContainsKey(x.SurfacePubId.Value))
            .Select(x => new
            {
                Surface = surfaceById[x.SurfacePubId!.Value],
                Day = DateOnly.FromDateTime(x.CapturedAtUtc.UtcDateTime.Date),
                Vwc = x.VwcPercent!.Value
            })
            .ToList();

        if (assignedReadings.Count == 0)
            return Result.Empty;

        var dailyAverages = assignedReadings
            .GroupBy(x => new { x.Surface.SurfacePubId, x.Day })
            .Select(group =>
            {
                var surface = group.First().Surface;
                return new DailyAverage(
                    surface.SurfacePubId,
                    surface.Label,
                    group.Key.Day,
                    decimal.Round(group.Average(x => x.Vwc), 1),
                    group.Count(),
                    surface.DryVwcThreshold,
                    surface.WetVwcThreshold);
            })
            .OrderBy(x => x.Day)
            .ThenBy(x => x.SurfaceLabel)
            .ThenBy(x => x.SurfacePubId)
            .ToList();

        var summaries = dailyAverages
            .GroupBy(x => x.SurfacePubId)
            .Select(group =>
            {
                var latest = group.MaxBy(x => x.Day)!;
                return new SurfaceSummary(
                    group.Key,
                    latest.SurfaceLabel,
                    latest.AverageVwc,
                    latest.Day,
                    decimal.Round(group.Average(x => x.AverageVwc), 1),
                    group.Sum(x => x.ReadingCount),
                    group.Count(),
                    latest.DryVwcThreshold,
                    latest.WetVwcThreshold);
            })
            .OrderBy(x => x.SurfaceLabel)
            .ThenBy(x => x.SurfacePubId)
            .ToList();

        var latestDay = dailyAverages.Max(x => x.Day);
        var latestFieldDailyAverage = decimal.Round(
            dailyAverages.Where(x => x.Day == latestDay).Average(x => x.AverageVwc), 1);

        return new Result(
            dailyAverages,
            summaries,
            latestDay,
            latestFieldDailyAverage,
            assignedReadings.Count,
            dailyAverages.Select(x => x.Day).Distinct().Count());
    }
}
