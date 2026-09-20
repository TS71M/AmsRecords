namespace AmsRecords.FieldMeasurements;

public static class ClippingIntervalPolicy
{
    public const int Confirmed = 1;
    public const int Estimated = 2;

    public sealed record Assessment(string Status, decimal? Hours, decimal? SuggestedHours, decimal? RecordingGapHours);

    public sealed record DisplayRate(decimal? Hours, decimal? DailyRate, bool IsEstimated);

    // Derived presentation only: never turn inferred cadence into a confirmed mowing record.
    public static DisplayRate ResolveDisplay(FieldMeasurementDtos.ClippingVolumeDto measurement)
    {
        var confirmed = measurement.IntervalStatus == "Confirmed";
        decimal? hours = confirmed ? measurement.AccumulationHours
            : measurement.IntervalStatus == "Estimated" && measurement.PreviousCutAtUtc.HasValue
                ? (decimal)(measurement.MeasuredAtUtc - measurement.PreviousCutAtUtc.Value).TotalHours
                : measurement.SuggestedIntervalHours;
        if (hours is not > 0m) return new(null, null, !confirmed);
        var rate = measurement.YieldMlPerM2.HasValue
            ? decimal.Round(measurement.YieldMlPerM2.Value * 24m / hours.Value, 2, MidpointRounding.AwayFromZero)
            : (decimal?)null;
        return new(hours, confirmed ? measurement.YieldMlPerM2PerDay ?? rate : measurement.IntervalStatus == "Estimated" ? measurement.EstimatedDailyRate ?? rate : rate, !confirmed);
    }

    public static Assessment Assess(DateTime measuredAt, DateTime? previousCut, int? source, IReadOnlyList<DateTime> history)
    {
        var earlier = history.Where(x => x < measuredAt && (measuredAt - x).TotalDays <= 90).Distinct().OrderDescending().Take(21).ToArray();
        decimal? gap = earlier.Length == 0 ? null : (decimal)(measuredAt - earlier[0]).TotalHours;
        // Historical cadence is a suggestion, never evidence that every cut was recorded.
        var intervals = earlier.Zip(earlier.Skip(1), (a, b) => (decimal)(a - b).TotalHours).Where(x => x > 0).Order().ToArray();
        decimal? usual = intervals.Length < 3 ? null : intervals.Length % 2 == 0
            ? (intervals[intervals.Length / 2 - 1] + intervals[intervals.Length / 2]) / 2
            : intervals[intervals.Length / 2];
        if (previousCut.HasValue && previousCut < measuredAt && source is Confirmed or Estimated)
            return new(source == Confirmed ? "Confirmed" : "Estimated", (decimal)(measuredAt - previousCut.Value).TotalHours, usual, gap);
        return new(gap.HasValue && usual.HasValue && gap > usual * 2 ? "SuspiciousGap" : "Unknown", null, usual, gap);
    }
}
