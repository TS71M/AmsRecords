namespace AmsRecords.Competitors;

public static class CompetitorRadarDtos
{
    public sealed record CompetitorSourceDto(
        Guid PubId,
        string Name,
        string WebsiteUrl,
        string? FocusAreas,
        string? Notes,
        bool IsEnabled,
        int ScanIntervalDays,
        IReadOnlyList<string> MonitoredUrls,
        DateTime? LastScanUtc,
        DateTime? NextScanUtc,
        DateTime? LastSuccessfulScanUtc,
        string? LastScanMessage);

    public sealed record CompetitorSourceSaveDto(
        string Name,
        string WebsiteUrl,
        string? FocusAreas,
        string? Notes,
        bool IsEnabled,
        int ScanIntervalDays,
        string? MonitoredUrls);

    public sealed record CompetitorFindingDto(
        Guid PubId,
        Guid SourcePubId,
        string SourceName,
        string EvidenceUrl,
        string Title,
        string FindingType,
        DateTime DetectedUtc,
        DateTime? PublishedAtUtc,
        string? ChangeSummary,
        string? AgronomyManagerRelevance,
        string? CustomerValue,
        string? StrategicFit,
        string? EstimatedEffort,
        string? RisksAndUnknowns,
        string Recommendation,
        string? RecommendationReason,
        string? EvidenceAssessment,
        int RelevanceScore,
        string Confidence,
        string ReviewStatus,
        string? CodexBrief,
        string CodexStatus,
        string? CodexThreadId,
        DateTime? CodexPreparedUtc);

    public sealed record CompetitorFindingUpdateDto(
        string? ChangeSummary,
        string? AgronomyManagerRelevance,
        string? CustomerValue,
        string? StrategicFit,
        string? EstimatedEffort,
        string? RisksAndUnknowns,
        string Recommendation,
        string? RecommendationReason,
        string? EvidenceAssessment,
        int RelevanceScore,
        string Confidence,
        string ReviewStatus);

    public sealed record CompetitorDiscussionMessageDto(Guid PubId, string Role, string Content, DateTime CreatedUtc);
    public sealed record CompetitorAskDto(string Question);
    public sealed record CompetitorAskResponseDto(string Answer, IReadOnlyList<CompetitorDiscussionMessageDto> Messages);

    public sealed record CompetitorScanResultDto(
        int SourcesScanned,
        int PagesScanned,
        int BaselinesCreated,
        int FindingsAdded,
        int ChangesIgnored,
        int SourcesFailed,
        IReadOnlyList<string> Messages);

    public sealed record CompetitorScanJobStatusDto(
        string State,
        bool Force,
        DateTime? QueuedUtc,
        DateTime? StartedUtc,
        DateTime? CompletedUtc,
        CompetitorScanResultDto? Result,
        string? ErrorMessage);

    public sealed record CompetitorStatusCountsDto(
        int All,
        int New,
        int Reviewing,
        int Watching,
        int Planned,
        int ReadyForCodex,
        int Dismissed);
}
