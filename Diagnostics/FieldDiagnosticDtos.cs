namespace AmsRecords.Diagnostics;

public static class FieldDiagnosticDtos
{
    public sealed record FieldDiagnosticFindingDto(
        [property: JsonPropertyName("conditionKey")] string ConditionKey,
        [property: JsonPropertyName("conditionTitle")] string ConditionTitle,
        [property: JsonPropertyName("role")] string Role,
        [property: JsonPropertyName("confidence")] decimal? Confidence,
        [property: JsonPropertyName("note")] string? Note);

    public sealed record FieldDiagnosticSurfaceOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("displayName")] string DisplayName,
        [property: JsonPropertyName("grassSpeciesSummary")] string? GrassSpeciesSummary
    );

    public sealed record FieldDiagnosticResultDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("diagnosisTitle")] string DiagnosisTitle,
        [property: JsonPropertyName("category")] string Category,
        [property: JsonPropertyName("diseaseLikely")] bool DiseaseLikely,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("relatedRiskKey")] string RelatedRiskKey,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("recommendation")] string Recommendation,
        [property: JsonPropertyName("recommendedActions")] IReadOnlyList<string> RecommendedActions,
        [property: JsonPropertyName("findings")] IReadOnlyList<FieldDiagnosticFindingDto> Findings,
        [property: JsonPropertyName("currentRiskLevel")] string CurrentRiskLevel,
        [property: JsonPropertyName("thresholdAdjustment")] FieldDiagnosticThresholdAdjustmentDto ThresholdAdjustment
    );

    public sealed record FieldDiagnosticHistoryQueryDto(
        [property: JsonPropertyName("take")] int? Take = null
    );

    public sealed record FieldDiagnosticReviewQueryDto(
        [property: JsonPropertyName("take")] int? Take = null,
        [property: JsonPropertyName("skip")] int? Skip = null
    );

    public sealed record FieldDiagnosticTrainingDatasetQueryDto(
        [property: JsonPropertyName("take")] int? Take = null,
        [property: JsonPropertyName("includeUnreviewed")] bool IncludeUnreviewed = false
    );

    public sealed record FieldDiagnosticDebugTestDto(
        [property: JsonPropertyName("diagnosticRequestPubId")] Guid? DiagnosticRequestPubId = null,
        [property: JsonPropertyName("diagnosticSubmittedAtUtc")] DateTimeOffset? DiagnosticSubmittedAtUtc = null,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId = null,
        [property: JsonPropertyName("note")] string? Note = null
    );

    public sealed record FieldDiagnosticReviewDecisionDto(
        [property: JsonPropertyName("diagnosticRequestPubId")] Guid DiagnosticRequestPubId,
        [property: JsonPropertyName("reviewNote")] string? ReviewNote = null
    );

    public sealed record FieldDiagnosticDeleteDto(
        [property: JsonPropertyName("diagnosticRequestPubId")] Guid DiagnosticRequestPubId,
        [property: JsonPropertyName("deleteNote")] string? DeleteNote = null
    );

    public sealed record FieldDiagnosticFeedbackDto(
        [property: JsonPropertyName("diagnosticRequestPubId")] Guid DiagnosticRequestPubId,
        [property: JsonPropertyName("correctedRiskKey")] string CorrectedRiskKey,
        [property: JsonPropertyName("correctedDiagnosisTitle")] string? CorrectedDiagnosisTitle,
        [property: JsonPropertyName("correctionNote")] string? CorrectionNote
    );

    public sealed record FieldDiagnosticRerunRequestDto(
        [property: JsonPropertyName("diagnosticRequestPubId")] Guid DiagnosticRequestPubId
    );

    public sealed record FieldDiagnosticRerunComparisonDto(
        [property: JsonPropertyName("diagnosticRequestPubId")] Guid DiagnosticRequestPubId,
        [property: JsonPropertyName("original")] FieldDiagnosticReviewItemDto Original,
        [property: JsonPropertyName("rerunResult")] FieldDiagnosticResultDto RerunResult,
        [property: JsonPropertyName("rerunEvidence")] FieldDiagnosticEvidenceDto? RerunEvidence,
        [property: JsonPropertyName("rerunAtUtc")] DateTimeOffset RerunAtUtc
    );

    public sealed record FieldDiagnosticReplaceWithRerunDto(
        [property: JsonPropertyName("diagnosticRequestPubId")] Guid DiagnosticRequestPubId,
        [property: JsonPropertyName("rerunResult")] FieldDiagnosticResultDto RerunResult,
        [property: JsonPropertyName("rerunEvidence")] FieldDiagnosticEvidenceDto? RerunEvidence,
        [property: JsonPropertyName("reviewNote")] string? ReviewNote = null
    );

    public sealed record FieldDiagnosticCorrectionDto(
        [property: JsonPropertyName("correctedAtUtc")] DateTime CorrectedAtUtc,
        [property: JsonPropertyName("correctedRiskKey")] string CorrectedRiskKey,
        [property: JsonPropertyName("correctedDiagnosisTitle")] string CorrectedDiagnosisTitle,
        [property: JsonPropertyName("correctionNote")] string? CorrectionNote,
        [property: JsonPropertyName("correctedByName")] string? CorrectedByName,
        [property: JsonPropertyName("reviewedOutcome")] FieldDiagnosticReviewedOutcomeDto? ReviewedOutcome = null
    );

    public sealed record FieldDiagnosticReviewedOutcomeDto(
        [property: JsonPropertyName("diagnosisTitle")] string DiagnosisTitle,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("recommendation")] string Recommendation,
        [property: JsonPropertyName("recommendedActions")] IReadOnlyList<string> RecommendedActions,
        [property: JsonPropertyName("findings")] IReadOnlyList<FieldDiagnosticFindingDto> Findings
    );

    public sealed record FieldDiagnosticEvidenceItemDto(
        [property: JsonPropertyName("label")] string Label,
        [property: JsonPropertyName("value")] string Value
    );

    public sealed record FieldDiagnosticEvidenceDto(
        [property: JsonPropertyName("fieldContext")] IReadOnlyList<FieldDiagnosticEvidenceItemDto> FieldContext,
        [property: JsonPropertyName("weatherContext")] IReadOnlyList<FieldDiagnosticEvidenceItemDto> WeatherContext,
        [property: JsonPropertyName("riskContext")] IReadOnlyList<FieldDiagnosticEvidenceItemDto> RiskContext,
        [property: JsonPropertyName("recentCorrectionContext")] string? RecentCorrectionContext,
        [property: JsonPropertyName("imageQualityContext")] string? ImageQualityContext = null
    );

    public sealed record FieldDiagnosticImageDto(
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId,
        [property: JsonPropertyName("imageFileName")] string? ImageFileName,
        [property: JsonPropertyName("roleLabel")] string? RoleLabel,
        [property: JsonPropertyName("imageThumbnailUrl")] string? ImageThumbnailUrl
    );

    public sealed record FieldDiagnosticReviewItemDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("diagnosticRequestPubId")] Guid DiagnosticRequestPubId,
        [property: JsonPropertyName("occurredAtUtc")] DateTime OccurredAtUtc,
        [property: JsonPropertyName("diagnosisTitle")] string DiagnosisTitle,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("relatedRiskKey")] string RelatedRiskKey,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("recommendation")] string Recommendation,
        [property: JsonPropertyName("findings")] IReadOnlyList<FieldDiagnosticFindingDto> Findings,
        [property: JsonPropertyName("currentRiskLevel")] string CurrentRiskLevel,
        [property: JsonPropertyName("surfaceName")] string? SurfaceName,
        [property: JsonPropertyName("userNote")] string? UserNote,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId,
        [property: JsonPropertyName("imageFileName")] string? ImageFileName,
        [property: JsonPropertyName("imageThumbnailUrl")] string? ImageThumbnailUrl,
        [property: JsonPropertyName("images")] IReadOnlyList<FieldDiagnosticImageDto> Images,
        [property: JsonPropertyName("createdByName")] string? CreatedByName,
        [property: JsonPropertyName("evidence")] FieldDiagnosticEvidenceDto? Evidence
    );

    public sealed record FieldDiagnosticReviewQueueDto(
        [property: JsonPropertyName("query")] FieldDiagnosticReviewQueryDto Query,
        [property: JsonPropertyName("totalPendingCount")] int TotalPendingCount,
        [property: JsonPropertyName("items")] IReadOnlyList<FieldDiagnosticReviewItemDto> Items
    );

    public sealed record FieldDiagnosticTrainingDatasetDto(
        [property: JsonPropertyName("generatedAtUtc")] DateTime GeneratedAtUtc,
        [property: JsonPropertyName("query")] FieldDiagnosticTrainingDatasetQueryDto Query,
        [property: JsonPropertyName("itemCount")] int ItemCount,
        [property: JsonPropertyName("items")] IReadOnlyList<FieldDiagnosticTrainingDatasetItemDto> Items
    );

    public sealed record FieldDiagnosticTrainingDatasetItemDto(
        [property: JsonPropertyName("diagnosticRequestPubId")] Guid DiagnosticRequestPubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("occurredAtUtc")] DateTime OccurredAtUtc,
        [property: JsonPropertyName("surfaceName")] string? SurfaceName,
        [property: JsonPropertyName("userNote")] string? UserNote,
        [property: JsonPropertyName("images")] IReadOnlyList<FieldDiagnosticImageDto> Images,
        [property: JsonPropertyName("aiDiagnosisTitle")] string AiDiagnosisTitle,
        [property: JsonPropertyName("aiRelatedRiskKey")] string AiRelatedRiskKey,
        [property: JsonPropertyName("aiConfidence")] decimal Confidence,
        [property: JsonPropertyName("aiSummary")] string AiSummary,
        [property: JsonPropertyName("reviewStatus")] string ReviewStatus,
        [property: JsonPropertyName("labelSource")] string LabelSource,
        [property: JsonPropertyName("labelRiskKey")] string LabelRiskKey,
        [property: JsonPropertyName("labelDiagnosisTitle")] string LabelDiagnosisTitle,
        [property: JsonPropertyName("labelNote")] string? LabelNote,
        [property: JsonPropertyName("evidence")] FieldDiagnosticEvidenceDto? Evidence
    );

    public sealed record FieldDiagnosticHistoryItemDto(
        [property: JsonPropertyName("diagnosticRequestPubId")] Guid DiagnosticRequestPubId,
        [property: JsonPropertyName("occurredAtUtc")] DateTime OccurredAtUtc,
        [property: JsonPropertyName("diagnosisTitle")] string DiagnosisTitle,
        [property: JsonPropertyName("category")] string Category,
        [property: JsonPropertyName("diseaseLikely")] bool DiseaseLikely,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("relatedRiskKey")] string RelatedRiskKey,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("recommendation")] string Recommendation,
        [property: JsonPropertyName("recommendedActions")] IReadOnlyList<string> RecommendedActions,
        [property: JsonPropertyName("findings")] IReadOnlyList<FieldDiagnosticFindingDto> Findings,
        [property: JsonPropertyName("currentRiskLevel")] string CurrentRiskLevel,
        [property: JsonPropertyName("thresholdAdjustment")] FieldDiagnosticThresholdAdjustmentDto ThresholdAdjustment,
        [property: JsonPropertyName("surfaceName")] string? SurfaceName,
        [property: JsonPropertyName("userNote")] string? UserNote,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId,
        [property: JsonPropertyName("imageFileName")] string? ImageFileName,
        [property: JsonPropertyName("imageThumbnailUrl")] string? ImageThumbnailUrl,
        [property: JsonPropertyName("images")] IReadOnlyList<FieldDiagnosticImageDto> Images,
        [property: JsonPropertyName("createdByName")] string? CreatedByName,
        [property: JsonPropertyName("correction")] FieldDiagnosticCorrectionDto? Correction,
        [property: JsonPropertyName("evidence")] FieldDiagnosticEvidenceDto? Evidence
    );

    public sealed record FieldDiagnosticHistoryDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("items")] IReadOnlyList<FieldDiagnosticHistoryItemDto> Items
    );

    public sealed record FieldDiagnosticThresholdAdjustmentDto(
        [property: JsonPropertyName("relatedRiskKey")] string RelatedRiskKey,
        [property: JsonPropertyName("noAlarmRaised")] bool NoAlarmRaised,
        [property: JsonPropertyName("wasApplied")] bool WasApplied,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("previousValue")] string? PreviousValue,
        [property: JsonPropertyName("updatedValue")] string? UpdatedValue
    );
}
