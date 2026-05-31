namespace AmsRecords.Areas;

public static class SurfaceCompositionTransmissionDtos
{
    public sealed record SurfaceCompositionTransmissionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("nextAllowedAtUtc")] DateTime NextAllowedAtUtc,
        [property: JsonPropertyName("blocksNewAnalysis")] bool BlocksNewAnalysis,
        [property: JsonPropertyName("photoCount")] int PhotoCount,
        [property: JsonPropertyName("modelUsed")] string? ModelUsed,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("summary")] string? Summary,
        [property: JsonPropertyName("suitabilitySummary")] string? SuitabilitySummary,
        [property: JsonPropertyName("weedCoveragePercent")] decimal WeedCoveragePercent,
        [property: JsonPropertyName("species")] IReadOnlyList<SurfaceCompositionTransmissionSpeciesDto> Species,
        [property: JsonPropertyName("rerunStatus")] string RerunStatus = SurfaceCompositionRerunStatuses.NotChecked,
        [property: JsonPropertyName("rerunStatusName")] string? RerunStatusName = null,
        [property: JsonPropertyName("rerunLastCheckedAtUtc")] DateTime? RerunLastCheckedAtUtc = null,
        [property: JsonPropertyName("rerunLastModelUsed")] string? RerunLastModelUsed = null,
        [property: JsonPropertyName("rerunLastConfidence")] decimal? RerunLastConfidence = null,
        [property: JsonPropertyName("rerunDecisionAtUtc")] DateTime? RerunDecisionAtUtc = null,
        [property: JsonPropertyName("rerunReviewNote")] string? RerunReviewNote = null
    );

    public sealed record SurfaceCompositionTransmissionSpeciesDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("grassSpeciesPubId")] Guid? GrassSpeciesPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("percent")] decimal Percent,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("notes")] string? Notes
    );

    public sealed record SurfaceCompositionAnalysisAdminQueryDto(
        [property: JsonPropertyName("skip")] int Skip = 0,
        [property: JsonPropertyName("take")] int Take = 50,
        [property: JsonPropertyName("rerunStatus")] string? RerunStatus = null,
        [property: JsonPropertyName("search")] string? Search = null
    );

    public sealed record SurfaceCompositionAnalysisAdminListDto(
        [property: JsonPropertyName("query")] SurfaceCompositionAnalysisAdminQueryDto Query,
        [property: JsonPropertyName("totalCount")] int TotalCount,
        [property: JsonPropertyName("items")] IReadOnlyList<SurfaceCompositionAnalysisAdminItemDto> Items
    );

    public sealed record SurfaceCompositionAnalysisAdminItemDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("surfacePubId")] Guid SurfacePubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("ibuName")] string IbuName,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("holeNumber")] int HoleNumber,
        [property: JsonPropertyName("surfaceName")] string SurfaceName,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("photoCount")] int PhotoCount,
        [property: JsonPropertyName("modelUsed")] string? ModelUsed,
        [property: JsonPropertyName("confidence")] decimal Confidence,
        [property: JsonPropertyName("summary")] string? Summary,
        [property: JsonPropertyName("weedCoveragePercent")] decimal WeedCoveragePercent,
        [property: JsonPropertyName("species")] IReadOnlyList<SurfaceCompositionTransmissionSpeciesDto> Species,
        [property: JsonPropertyName("rerunStatus")] string RerunStatus,
        [property: JsonPropertyName("rerunStatusName")] string RerunStatusName,
        [property: JsonPropertyName("rerunLastCheckedAtUtc")] DateTime? RerunLastCheckedAtUtc,
        [property: JsonPropertyName("rerunLastModelUsed")] string? RerunLastModelUsed,
        [property: JsonPropertyName("rerunLastConfidence")] decimal? RerunLastConfidence,
        [property: JsonPropertyName("rerunDecisionAtUtc")] DateTime? RerunDecisionAtUtc,
        [property: JsonPropertyName("rerunReviewNote")] string? RerunReviewNote
    );

    public sealed record SurfaceCompositionRerunRequestDto(
        [property: JsonPropertyName("transmissionPubId")] Guid TransmissionPubId
    );

    public sealed record SurfaceCompositionRerunComparisonDto(
        [property: JsonPropertyName("transmissionPubId")] Guid TransmissionPubId,
        [property: JsonPropertyName("original")] SurfaceCompositionTransmissionDto Original,
        [property: JsonPropertyName("rerunResult")] AreaCompositionAiAnalysisDtos.AreaCompositionAiAnalysisDto RerunResult,
        [property: JsonPropertyName("rerunAtUtc")] DateTimeOffset RerunAtUtc
    );

    public sealed record SurfaceCompositionReplaceWithRerunDto(
        [property: JsonPropertyName("transmissionPubId")] Guid TransmissionPubId,
        [property: JsonPropertyName("rerunResult")] AreaCompositionAiAnalysisDtos.AreaCompositionAiAnalysisDto RerunResult,
        [property: JsonPropertyName("reviewNote")] string? ReviewNote = null
    );

    public sealed record SurfaceCompositionCancelRerunDto(
        [property: JsonPropertyName("transmissionPubId")] Guid TransmissionPubId,
        [property: JsonPropertyName("reviewNote")] string? ReviewNote = null
    );

    public static class SurfaceCompositionRerunStatuses
    {
        public const string NotChecked = "not_checked";
        public const string PendingDecision = "pending_decision";
        public const string Accepted = "accepted";
        public const string OriginalKept = "original_kept";

        public static string DisplayName(string? status)
            => status switch
            {
                PendingDecision => "Rerun pending",
                Accepted => "Rerun accepted",
                OriginalKept => "Original kept",
                _ => "Not checked"
            };
    }
}
