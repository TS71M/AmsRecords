namespace AmsRecords.FieldMeasurements;

/// <summary>
/// Normalizes a complete clipping collection using the standard turfgrass
/// reporting unit recommended by the Asian Turfgrass Center.
/// </summary>
public static class ClippingVolumeNormalization
{
    public const string DailyRateUnit = "mL/m²/day";
    public const string StandardUnit = "mL/m²";

    public static decimal? CalculateMlPerM2(decimal canonicalLitres, decimal? sampleAreaM2)
    {
        if (canonicalLitres < 0m)
            throw new ArgumentOutOfRangeException(nameof(canonicalLitres));
        if (!sampleAreaM2.HasValue || sampleAreaM2.Value <= 0m)
            return null;

        return decimal.Round(canonicalLitres * 1_000m / sampleAreaM2.Value, 2,
            MidpointRounding.AwayFromZero);
    }

    public static decimal? CalculateMlPerM2PerDay(
        decimal canonicalLitres,
        decimal? sampleAreaM2,
        DateTime measuredAtUtc,
        DateTime? previousCutAtUtc)
    {
        var yield = CalculateMlPerM2(canonicalLitres, sampleAreaM2);
        if (!yield.HasValue || !previousCutAtUtc.HasValue)
            return null;

        var elapsedHours = (measuredAtUtc - previousCutAtUtc.Value).TotalHours;
        if (elapsedHours <= 0d)
            return null;

        return decimal.Round(yield.Value * 24m / (decimal)elapsedHours, 2,
            MidpointRounding.AwayFromZero);
    }
}
