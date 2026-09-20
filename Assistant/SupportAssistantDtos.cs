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
    IReadOnlyList<SupportAssistantMessageDto> History,
    AssistantContextDto? Context = null);

public sealed record SupportAssistantLinkDto(
    string Label,
    string Url,
    string? Description = null);

public sealed record SupportAssistantAskResponseDto(
    string Answer,
    bool EscalationRecommended,
    string? EscalationReason,
    IReadOnlyList<string> SuggestedQuestions,
    IReadOnlyList<SupportAssistantLinkDto> Links,
    IReadOnlyList<CourseAssistantSurfaceDto>? Sources = null,
    bool CanGuideToSurfaces = false);

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

// Client context is a scope request, never an authorization grant. User identity comes from authentication.
public sealed record AssistantContextDto(
    Guid? SelectedFieldId = null,
    Guid? SelectedSurfaceId = null,
    int? SelectedHole = null,
    Guid? HistoryFieldId = null,
    AssistantDeviceLocationDto? DeviceLocation = null);

// Advisory match calculated on the device. Never conveys permission or raw coordinates.
public sealed record AssistantDeviceLocationDto(Guid NearbySurfaceId, DateTimeOffset CapturedAtUtc);
