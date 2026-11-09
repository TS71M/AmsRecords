namespace AmsRecords.Assistant;

public sealed record SupportAssistantMessageDto(
    string Role,
    string Content,
    Guid? FieldPubId = null,
    bool ScopeKnown = false);

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
    IReadOnlyList<string> FormLabels,
    AssistantFormSnapshotDto? Form = null,
    AssistantNavigationSnapshotDto? Navigation = null);

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
    AssistantContextDto? Context = null,
    IReadOnlyList<string>? SupportedDraftKinds = null,
    AssistantImageDto? Image = null,
    Guid? ActionId = null,
    IReadOnlyList<string>? SupportedActions = null,
    Guid? RequestId = null,
    string? ActionKind = null,
    AssistantContextDto? ActionContext = null,
    bool ReviewBeforeSave = true,
    bool NavigationOnly = false,
    bool FormOnly = false,
    bool PhotoReview = false);

// Current request only; never included in history or conversation logging.
public sealed record AssistantImageDto(string Base64);

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
    bool CanGuideToSurfaces = false,
    AssistantDraftDto? Draft = null,
    AssistantConversationActionDto? Action = null,
    AssistantFormPatchDto? Form = null,
    bool OpenDraftForReview = false,
    string? ReviewUrl = null,
    AssistantNavigationPatchDto? Navigation = null,
    IReadOnlyList<AssistantLabReportSourceDto>? LabReportSources = null,
    IReadOnlyList<AmsRecords.Products.ProductsDtos.ProductWebsiteImportMatchDto>? ProductMatches = null);

public sealed record AssistantLabReportSourceDto(Guid DocumentPubId, Guid FieldPubId, string FileName,
    string MimeType, int ByteLength, int PageNumber, int PageCount);

public static class AssistantActionKinds
{
    public const string CreateProduct = "create_product";
    public const string RecordMeasurement = "record_measurement";
    public const string FillForm = "fill_form";
    public const string Navigate = "navigate";
    public const string ImportLab = "import_lab";
    public const string ViewLabReport = "view_lab_report";
    public const string ViewProduct = "view_product";
}

// Opaque, user-bound continuation. This is never an authorization grant.
public sealed record AssistantConversationActionDto(Guid Id, string Kind, string Status, Guid? CreatedRecordPubId = null);

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
    AssistantDeviceLocationDto? DeviceLocation = null,
    DateOnly? AnalysisFrom = null,
    DateOnly? AnalysisTo = null,
    int? UtcOffsetMinutes = null,
    AssistantPlannerContextDto? Planner = null,
    Guid? SelectedAreaId = null,
    string? SoilMethodCode = null, string? TimeZoneId = null,
    bool UseCurrentMeasurementSelection = false);

public sealed record AssistantPlannerContextDto(int? Year = null, Guid? PlanPubId = null,
    Guid? RevisionPubId = null, Guid? ZonePubId = null);

// Advisory match calculated on the device. Never conveys permission or raw coordinates.
public sealed record AssistantDeviceLocationDto(Guid NearbySurfaceId, DateTimeOffset CapturedAtUtc);
