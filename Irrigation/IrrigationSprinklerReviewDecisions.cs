namespace AmsRecords.Irrigation;

public static class IrrigationSprinklerReviewDecisions
{
    public const string ConfirmAsRecorded = "ConfirmAsRecorded";
    public const string CorrectAndConfirm = "CorrectAndConfirm";
    public const string ConfirmMismatch = "ConfirmMismatch";
    public const string ConfirmMinorErrors = "ConfirmMinorErrors";
    public const string NeedsFurtherReview = "NeedsFurtherReview";

    public static IReadOnlyList<string> Completed { get; } = Array.AsReadOnly(new[]
        { ConfirmAsRecorded, CorrectAndConfirm, ConfirmMismatch, ConfirmMinorErrors });

    public static string? Canonicalize(string? value)
        => new[] { ConfirmAsRecorded, CorrectAndConfirm, ConfirmMismatch, ConfirmMinorErrors, NeedsFurtherReview }
            .FirstOrDefault(candidate => string.Equals(candidate, value?.Trim(), StringComparison.OrdinalIgnoreCase));

    public static bool IsCompleted(string? decision)
        => Canonicalize(decision) is { } value && Completed.Contains(value);

    public static bool RequiresReview(string? decision,
        Lib.Enums.IrrigationNozzleConfigurationAssessment assessment)
        => !IsCompleted(decision) && (Canonicalize(decision) == NeedsFurtherReview ||
            assessment is Lib.Enums.IrrigationNozzleConfigurationAssessment.ReviewRequired or
                Lib.Enums.IrrigationNozzleConfigurationAssessment.NotEvaluated);

    public static IrrigationNozzleConfigurationEvaluator.Result ApplyTo(
        IrrigationNozzleConfigurationEvaluator.Result evaluation,
        string? decision)
    {
        var canonical = Canonicalize(decision);
        // A saved minor decision cannot override new front discrepancies or missing evidence.
        if (canonical == ConfirmMinorErrors) return evaluation;
        if (canonical == ConfirmMismatch)
        {
            return new IrrigationNozzleConfigurationEvaluator.Result(
                Lib.Enums.IrrigationNozzleConfigurationAssessment.Incompatible,
                evaluation.Issues
                    .Append("An administrator confirmed that the observed installation does not match the selected reference configuration.")
                    .Distinct()
                    .ToList());
        }

        if (canonical == NeedsFurtherReview &&
            evaluation.Assessment != Lib.Enums.IrrigationNozzleConfigurationAssessment.Incompatible)
        {
            return new IrrigationNozzleConfigurationEvaluator.Result(
                Lib.Enums.IrrigationNozzleConfigurationAssessment.ReviewRequired,
                evaluation.Issues
                    .Append("An administrator kept this installation under review pending additional evidence.")
                    .Distinct()
                    .ToList());
        }

        return evaluation;
    }
}
