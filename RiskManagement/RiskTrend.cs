namespace AmsRecords.RiskManagement;

/// <summary>Next-day pressure change, in percentage points (not relative percent).</summary>
public static class RiskTrend
{
    public static string Compare(decimal? current, decimal? next)
        => current is null || next is null ? "unavailable"
            : next - current > 1m ? "up"
            : next - current < -1m ? "down" : "flat";

    public static string At(IReadOnlyList<(DateOnly Date, string? Level, decimal? PressureScore)> days, int index)
    {
        if (index < 0 || index + 1 >= days.Count)
            return "unavailable";
        var current = days[index];
        var next = days[index + 1];
        // Never bridge missing dates or turn absent observations into low risk.
        if (next.Date.DayNumber - current.Date.DayNumber != 1 || !HasData(current.Level) || !HasData(next.Level))
            return "unavailable";
        return Compare(current.PressureScore, next.PressureScore);
    }

    static bool HasData(string? level)
        => level?.ToLowerInvariant() is "none" or "low" or "moderate" or "medium" or "high";

    public static string Glyph(string? trend) => trend switch
    {
        "up" => "\u2197\uFE0E", "down" => "\u2198\uFE0E", "flat" => "\u2192\uFE0E", _ => "\u2014"
    };

    public static string TextKey(string? trend) => trend switch
    {
        "up" => "Rising", "down" => "Falling", "flat" => "Steady", _ => "NoTrendAvailable"
    };
}
