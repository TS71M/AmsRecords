namespace AmsRecords.Research;

public static class ResearchRadarDtos
{
    public sealed record ResearchSourceDto(
        Guid PubId,
        string Name,
        string WebsiteUrl,
        string? FeedUrl,
        string SourceType,
        string? Topics,
        string? AccessNotes,
        bool IsEnabled,
        int ScanIntervalDays,
        DateTime? LastScanUtc,
        DateTime? NextScanUtc,
        DateTime? LastSuccessfulScanUtc,
        string? LastScanMessage);

    public sealed record ResearchSourceSaveDto(
        string Name,
        string WebsiteUrl,
        string? FeedUrl,
        string SourceType,
        string? Topics,
        string? AccessNotes,
        bool IsEnabled,
        int ScanIntervalDays);

    public sealed record ResearchArticleDto(
        Guid PubId,
        Guid SourcePubId,
        string SourceName,
        string OriginalUrl,
        string Title,
        string? Author,
        string? Publisher,
        DateTime? PublishedAtUtc,
        DateTime DiscoveredUtc,
        string? SourceExcerpt,
        string? EditorialSummary,
        string? WhyItMatters,
        string? EvidenceAssessment,
        string? Limitations,
        string? TopicTags,
        string ReviewStatus,
        bool SuggestELearningUpdate,
        bool PublishWebApp,
        bool PublishMobileApp,
        bool PublishPublicWeb,
        DateTime? PublishedUtc);

    public sealed record ResearchArticleUpdateDto(
        string? EditorialSummary,
        string? WhyItMatters,
        string? EvidenceAssessment,
        string? Limitations,
        string? TopicTags,
        string ReviewStatus,
        bool SuggestELearningUpdate,
        bool PublishWebApp,
        bool PublishMobileApp,
        bool PublishPublicWeb);

    public sealed record ResearchDiscussionMessageDto(
        Guid PubId,
        string Role,
        string Content,
        DateTime CreatedUtc);

    public sealed record ResearchAskDto(string Question);

    public sealed record ResearchAskResponseDto(
        string Answer,
        IReadOnlyList<ResearchDiscussionMessageDto> Messages);

    public sealed record ResearchScanResultDto(
        int SourcesScanned,
        int ArticlesAdded,
        int ArticlesPrepared,
        int NonArticleLinksRejected,
        int SourcesFailed,
        IReadOnlyList<string> Messages);
}
