namespace AmsRecords.RiskManagement;

public sealed record RiskLocationRow<T>(string Name, T? Day, bool IsSummary, bool HasData) where T : class;

/// <summary>Keeps a complete aggregate row followed by every location, including unavailable peers.</summary>
public static class RiskLocationRows
{
    public static IReadOnlyList<RiskLocationRow<T>> Build<T>(T summaryDay, DateOnly date,
        IReadOnlyList<LocationRiskSummary> locations, Func<LocationRiskSummary, IReadOnlyList<T>> days,
        Func<T, DateOnly> dayDate, Func<T, bool> hasData, DateTimeOffset? now = null) where T : class
    {
        LocationRiskAggregation.ValidateField(locations);
        var rows = new List<RiskLocationRow<T>> { new("", summaryDay, true, hasData(summaryDay)) };
        if (locations.Count <= 1) return rows;
        foreach (var location in locations)
        {
            var day = LocationRiskAggregation.IsFresh(location.Summary, now)
                ? days(location).FirstOrDefault(x => dayDate(x) == date) : null;
            rows.Add(new(location.Name, day, false, day is not null && hasData(day)));
        }
        return rows;
    }
}
