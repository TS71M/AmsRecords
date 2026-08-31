namespace AmsRecords.Irrigation;

public static class IrrigationSprinklerReviewDecisions
{
    public const string ConfirmAsRecorded = "ConfirmAsRecorded";
    public const string CorrectAndConfirm = "CorrectAndConfirm";
    public const string ConfirmMismatch = "ConfirmMismatch";
    public const string NeedsFurtherReview = "NeedsFurtherReview";

    public static string? Canonicalize(string? value)
        => new[] { ConfirmAsRecorded, CorrectAndConfirm, ConfirmMismatch, NeedsFurtherReview }
            .FirstOrDefault(candidate => string.Equals(candidate, value?.Trim(), StringComparison.OrdinalIgnoreCase));

    public static IrrigationNozzleConfigurationEvaluator.Result ApplyTo(
        IrrigationNozzleConfigurationEvaluator.Result evaluation,
        string? decision)
    {
        var canonical = Canonicalize(decision);
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
