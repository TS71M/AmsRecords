using static AmsRecords.RiskManagement.RiskDtos;

namespace AmsRecords.RiskManagement;

public sealed record LocationRiskSummary(Guid LocationPubId, string Name, bool IsPrimary, RiskSummaryDto Summary);

/// <summary>Selects a complete row from the highest-risk available location; never mixes its inputs.</summary>
public static class LocationRiskAggregation
{
    public static RiskSummaryDto? Summarize(IReadOnlyList<LocationRiskSummary> locations)
    {
        var first = locations.FirstOrDefault()?.Summary;
        if (first is null) return null;
        var all = locations.Select(x => x.Summary).ToArray();
        return first with
        {
            DollarSpot = first.DollarSpot is null ? null : first.DollarSpot with { Days = Select(all.SelectMany(x => x.DollarSpot?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.Level.ToString(), x => x.ProbabilityPct ?? 0), MissingReason = null },
            DewPoint = first.DewPoint is null ? null : first.DewPoint with { Days = Select(all.SelectMany(x => x.DewPoint?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.RiskLevel.ToString(), x => x.HoursNearDew ?? 0) },
            Frost = first.Frost is null ? null : first.Frost with { Days = Select(all.SelectMany(x => x.Frost?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.RiskLevel, x => -(x.MinTempC ?? 0)) },
            HeatStress = first.HeatStress is null ? null : first.HeatStress with { Days = Select(all.SelectMany(x => x.HeatStress?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.RiskLevel, x => x.MaxTempC ?? 0) },
            Pythium = first.Pythium is null ? null : first.Pythium with { Days = Select(all.SelectMany(x => x.Pythium?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.RiskLevel, x => x.HoursTemp22OrMore ?? 0) },
            Microdochium = first.Microdochium is null ? null : first.Microdochium with { Days = Select(all.SelectMany(x => x.Microdochium?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.RiskLevel, x => x.HoursNearDew ?? 0) }
        };
    }

    static List<T> Select<T>(IEnumerable<T> rows, Func<T, DateOnly> date, Func<T, bool> available, Func<T, string> level, Func<T, decimal> pressure)
        => rows.GroupBy(date).OrderBy(x => x.Key).Select(group => group
            .OrderByDescending(available).ThenByDescending(x => Severity(level(x))).ThenByDescending(pressure).First()).ToList();

    static int Severity(string level) => level.ToLowerInvariant() switch
    {
        "high" => 3, "moderate" or "medium" => 2, "low" => 1, "none" => 0, _ => -1
    };
}
