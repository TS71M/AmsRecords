namespace AmsRecords.Diseases;

public static class DiseaseTrainingReviewPolicy
{
    public const string SourceLabelMethod = "sourceLabel";
    public static readonly IReadOnlyList<string> ConfirmationMethods =
        ["fieldInspection", "microscopy", "labTest", "expertReview", "other"];

    public static string? NormalizeGroup(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant();

    public static string? Validate(bool approved, string? finalCondition, string? method, string? note, string? group,
        bool useForLearning = false)
    {
        if (method is not null && method != SourceLabelMethod && !ConfirmationMethods.Contains(method, StringComparer.Ordinal))
            return "Choose a supported confirmation method.";
        if (method == SourceLabelMethod && useForLearning)
            return "Source-labelled references are not independently confirmed. Disable learning/evaluation use; they may be used for reference comparison only.";
        if ((group?.Trim().Length ?? 0) > 120)
            return "The outbreak identifier must not exceed 120 characters.";
        if (!approved) return null;
        if (string.IsNullOrWhiteSpace(finalCondition) || finalCondition.Trim().StartsWith("Uncertain", StringComparison.OrdinalIgnoreCase))
            return "An approved example requires a final condition label.";
        if (method == SourceLabelMethod && string.IsNullOrWhiteSpace(note))
            return "Record the source collection and provisional condition label before approving a source-labelled reference.";
        if (string.IsNullOrWhiteSpace(method) || string.IsNullOrWhiteSpace(note))
            return "Record how the diagnosis was independently confirmed and describe the observed evidence before approval. An AI suggestion alone is not confirmation.";
        if (string.IsNullOrWhiteSpace(group))
            return "Assign an outbreak identifier before approval. Use the same identifier for every photo from the same outbreak so evaluation can keep them together.";
        return null;
    }
}
