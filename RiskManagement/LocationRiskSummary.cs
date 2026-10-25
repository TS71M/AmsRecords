using static AmsRecords.RiskManagement.RiskDtos;

namespace AmsRecords.RiskManagement;

public sealed record LocationRiskSummary(Guid LocationPubId, string Name, bool IsPrimary, RiskSummaryDto Summary);

/// <summary>Selects a complete row from the highest-risk available location; never mixes its inputs.</summary>
public static class LocationRiskAggregation
{
    // A daily forecast summary older than a day is unknown, not a low-risk peer.
    public static bool IsFresh(RiskSummaryDto summary, DateTimeOffset? now = null)
        => (now ?? DateTimeOffset.UtcNow) - summary.GeneratedAtUtc <= TimeSpan.FromHours(24);

    public static RiskSummaryDto? Summarize(IReadOnlyList<LocationRiskSummary> locations, DateTimeOffset? now = null)
    {
        ValidateField(locations);
        var fresh = locations.Where(x => IsFresh(x.Summary, now)).ToArray();
        var first = fresh.FirstOrDefault()?.Summary;
        if (first is null) return null;
        var all = fresh.Select(x => x.Summary).ToArray();
        var dollarSpot = all.Select(x => x.DollarSpot).FirstOrDefault(x => x is not null);
        var dew = all.Select(x => x.DewPoint).FirstOrDefault(x => x is not null);
        var frost = all.Select(x => x.Frost).FirstOrDefault(x => x is not null);
        var heat = all.Select(x => x.HeatStress).FirstOrDefault(x => x is not null);
        var pythium = all.Select(x => x.Pythium).FirstOrDefault(x => x is not null);
        var microdochium = all.Select(x => x.Microdochium).FirstOrDefault(x => x is not null);
        return first with
        {
            GeneratedAtUtc = all.Min(x => x.GeneratedAtUtc),
            DollarSpot = dollarSpot is null ? null : dollarSpot with { Days = Select(all.SelectMany(x => x.DollarSpot?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.Level.ToString(), x => x.ProbabilityPct ?? 0), EarlyWarning = null,
                MissingReason = all.Any(x => x.DollarSpot?.Days.Any(d => d.HasData) == true) ? null : dollarSpot.MissingReason },
            DewPoint = dew is null ? null : dew with { Days = Select(all.SelectMany(x => x.DewPoint?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.RiskLevel.ToString(), x => x.HoursNearDew ?? 0) },
            Frost = frost is null ? null : frost with { Days = Select(all.SelectMany(x => x.Frost?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.RiskLevel, x => -(x.MinTempC ?? 0)) },
            HeatStress = heat is null ? null : heat with { Days = Select(all.SelectMany(x => x.HeatStress?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.RiskLevel, x => x.MaxTempC ?? 0) },
            Pythium = pythium is null ? null : pythium with { Days = Select(all.SelectMany(x => x.Pythium?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.RiskLevel, x => x.HoursTemp22OrMore ?? 0) },
            Microdochium = microdochium is null ? null : microdochium with { Days = Select(all.SelectMany(x => x.Microdochium?.Days ?? []), x => x.DateLocal, x => x.HasData, x => x.RiskLevel, x => x.HoursNearDew ?? 0) }
        };
    }

    static List<T> Select<T>(IEnumerable<T> rows, Func<T, DateOnly> date, Func<T, bool> available, Func<T, string> level, Func<T, decimal> pressure)
        => rows.GroupBy(date).OrderBy(x => x.Key).Select(group => group
            .OrderByDescending(available).ThenByDescending(x => Severity(level(x))).ThenByDescending(pressure).First()).ToList();

    public static int Severity(string level) => level.ToLowerInvariant() switch
    {
        "high" => 3, "moderate" or "medium" => 2, "low" => 1, "none" => 0, _ => -1
    };

    internal static void ValidateField(IReadOnlyList<LocationRiskSummary> locations)
    {
        if (locations.Select(x => x.Summary.FieldPubId).Distinct().Skip(1).Any())
            throw new ArgumentException("Risk locations must belong to the same field.", nameof(locations));
    }
}
