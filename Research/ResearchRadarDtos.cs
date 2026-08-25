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
        DateTime? PublishedUtc,
        ResearchArticleEngagementDto? Engagement = null,
        string ContentCulture = "en");

    public sealed record ResearchArticleEngagementDto(
        int LikeCount,
        int ApplauseCount,
        int CommentCount,
        bool IsLiked,
        bool IsApplauded,
        bool IsSaved);

    public sealed record ResearchReactionSaveDto(bool IsActive);

    public sealed record ResearchCommentSaveDto(string Content, Guid? ParentPubId = null);

    public sealed record ResearchCommentDto(
        Guid PubId,
        Guid ArticlePubId,
        Guid? ParentPubId,
        string AuthorDisplayName,
        string Content,
        string LanguageCode,
        DateTime CreatedUtc,
        DateTime? UpdatedUtc,
        bool IsRemoved,
        bool IsOwn,
        bool CanEdit,
        bool CanRemove);

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

    public sealed record ResearchCodexTaskDto(
        Guid PubId,
        string Status,
        DateTime QueuedUtc,
        DateTime? StartedUtc,
        DateTime? CompletedUtc,
        int Attempts,
        string? LastError,
        string? ThreadId = null,
        string? Brief = null,
        string? Result = null);

    public sealed record RadarWorkItemSaveDto(string Brief);

    public sealed record ResearchCodexTaskClaimDto(
        Guid PubId,
        Guid LeaseId,
        string Brief);

    public sealed record ResearchCodexTaskCompletionDto(
        Guid LeaseId,
        bool Succeeded,
        string? Result,
        string? ErrorMessage,
        string? ThreadId = null);

    public sealed record ResearchScanResultDto(
        int SourcesScanned,
        int ArticlesAdded,
        int ArticlesPrepared,
        int NonArticleLinksRejected,
        int SourcesFailed,
        IReadOnlyList<string> Messages);

    public sealed record ResearchScanJobStatusDto(
        string State,
        bool Force,
        DateTime? QueuedUtc,
        DateTime? StartedUtc,
        DateTime? CompletedUtc,
        ResearchScanResultDto? Result,
        string? ErrorMessage);

    public sealed record ResearchStatusCountsDto(
        int All,
        int New,
        int Reviewing,
        int Saved,
        int ForImplementation,
        int Published,
        int Rejected);
}
