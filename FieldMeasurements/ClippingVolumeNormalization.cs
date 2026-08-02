namespace AmsRecords.FieldMeasurements;

/// <summary>
/// Normalizes a complete clipping collection using the standard turfgrass
/// reporting unit recommended by the Asian Turfgrass Center.
/// </summary>
public static class ClippingVolumeNormalization
{
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
}
