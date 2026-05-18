namespace AmsRecords.Assistant;

public sealed record SupportAssistantMessageDto(
    string Role,
    string Content);

public sealed record SupportAssistantPageInfoDto(
    string? Area,
    string? Page,
    string? Title,
    string? Intro,
    IReadOnlyList<string> Items,
    string? Url,
    string? BrowserTitle,
    IReadOnlyList<string> VisibleHeadings,
    IReadOnlyList<string> VisibleActions,
    IReadOnlyList<string> FormLabels);

public sealed record SupportAssistantUserContextDto(
    Guid? UserPubId,
    Guid? IbuPubId,
    string? IbuName,
    IReadOnlyList<string> Roles,
    string? Culture);

public sealed record SupportAssistantKnowledgeSnippetDto(
    string Title,
    string Content,
    string? Source = null);

public sealed record SupportAssistantAskRequestDto(
    string Question,
    SupportAssistantPageInfoDto Page,
    SupportAssistantUserContextDto User,
    IReadOnlyList<SupportAssistantKnowledgeSnippetDto> Knowledge,
    IReadOnlyList<SupportAssistantMessageDto> History);

public sealed record SupportAssistantLinkDto(
    string Label,
    string Url,
    string? Description = null);

public sealed record SupportAssistantAskResponseDto(
    string Answer,
    bool EscalationRecommended,
    string? EscalationReason,
    IReadOnlyList<string> SuggestedQuestions,
    IReadOnlyList<SupportAssistantLinkDto> Links);

public sealed record SupportAssistantLogDto(
    Guid PubId,
    DateTime AskedUtc,
    string Question,
    string Answer,
    string? PageTitle,
    string? PageArea,
    string? PageRoute,
    string? Url,
    bool EscalationRecommended,
    string? EscalationReason,
    string? Culture);
