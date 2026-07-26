using static AmsRecords.LeafNitrate.LeafNitrateDtos;

namespace AmsRecords.LeafNitrate;

/// <summary>
/// Shared, deterministic rules for the HORIBA LAQUAtwin NO3-11 measurement basis.
/// Raw readings remain unchanged; this helper only produces the comparable NO3-N value.
/// </summary>
public static class LeafNitrateRules
{
    public const decimal No3ToNo3NFactor = 14m / 62m;
    public const decimal MinimumNo3Ppm = 6m;
    public const decimal MaximumNo3Ppm = 9900m;
    public const decimal MinimumNo3NPpm = 1.4m;
    public const decimal MaximumNo3NPpm = 2200m;
    public const decimal MeterUncertaintyPercent = 10m;
    public const decimal MinimumOperatingTemperatureC = 5m;
    public const decimal MaximumOperatingTemperatureC = 40m;
    public const decimal DefaultCalibrationLowPpmNo3 = 150m;
    public const decimal DefaultCalibrationHighPpmNo3 = 2000m;
    public const int CalibrationMeasurementInterval = 30;
    public const int CalibrationRecommendedAfterDays = 3;
    public const int CalibrationOverdueAfterDays = 5;

    public sealed record CalibrationStatus(
        DateTime? LastCalibrationAtUtc,
        int MeasurementsSinceCalibration,
        int? DaysSinceCalibration,
        bool IsRecommended,
        bool IsOverdue);

    public static CalibrationStatus ResolveCalibrationStatus(
        IEnumerable<LeafNitrateMeasurementDto> measurements,
        DateTime nowUtc)
    {
        var items = measurements.OrderBy(x => x.SampledAtUtc).ToList();
        var lastCalibrationAtUtc = items
            .Where(x => x.CalibrationAtUtc.HasValue)
            .Select(x => x.CalibrationAtUtc!.Value)
            .OrderByDescending(x => x)
            .FirstOrDefault();

        if (lastCalibrationAtUtc == default)
            return new(null, items.Count, null, true, true);

        var normalizedNow = nowUtc.Kind == DateTimeKind.Utc ? nowUtc : nowUtc.ToUniversalTime();
        var elapsed = normalizedNow - lastCalibrationAtUtc;
        var days = Math.Max(0, (int)Math.Floor(elapsed.TotalDays));
        var count = items.Count(x => x.SampledAtUtc > lastCalibrationAtUtc);
        return new(
            lastCalibrationAtUtc,
            count,
            days,
            count >= CalibrationMeasurementInterval || elapsed >= TimeSpan.FromDays(CalibrationRecommendedAfterDays),
            elapsed >= TimeSpan.FromDays(CalibrationOverdueAfterDays));
    }

    public static decimal NormalizeToNo3NPpm(decimal rawValue, string rawBasis)
    {
        if (IsNo3Basis(rawBasis))
            return decimal.Round(rawValue * No3ToNo3NFactor, 2);
        if (IsNo3NBasis(rawBasis))
            return decimal.Round(rawValue, 2);

        throw new ArgumentException("Unsupported nitrate measurement basis.", nameof(rawBasis));
    }

    public static bool IsSupportedBasis(string? rawBasis)
        => IsNo3Basis(rawBasis) || IsNo3NBasis(rawBasis);

    public static bool IsSupportedUnit(string? rawUnit)
        => string.Equals(rawUnit?.Trim(), MeasurementUnits.Ppm, StringComparison.OrdinalIgnoreCase) ||
           string.Equals(rawUnit?.Trim(), MeasurementUnits.MgPerL, StringComparison.OrdinalIgnoreCase);

    public static bool IsWithinMeterRange(decimal rawValue, string rawBasis)
        => IsNo3Basis(rawBasis)
            ? rawValue is >= MinimumNo3Ppm and <= MaximumNo3Ppm
            : IsNo3NBasis(rawBasis) && rawValue is >= MinimumNo3NPpm and <= MaximumNo3NPpm;

    public static string NormalizeBasis(string rawBasis)
        => IsNo3NBasis(rawBasis) ? MeasurementBases.No3N : MeasurementBases.No3;

    public static string NormalizeUnit(string rawUnit)
        => string.Equals(rawUnit?.Trim(), MeasurementUnits.MgPerL, StringComparison.OrdinalIgnoreCase)
            ? MeasurementUnits.MgPerL
            : MeasurementUnits.Ppm;

    static bool IsNo3Basis(string? rawBasis)
        => string.Equals(rawBasis?.Trim(), MeasurementBases.No3, StringComparison.OrdinalIgnoreCase);

    static bool IsNo3NBasis(string? rawBasis)
        => string.Equals(rawBasis?.Trim(), MeasurementBases.No3N, StringComparison.OrdinalIgnoreCase);
}
