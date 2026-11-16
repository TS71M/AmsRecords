namespace AmsRecords.Assistant;

public sealed record SupportAssistantMessageDto(
    string Role,
    string Content,
    Guid? FieldPubId = null,
    bool ScopeKnown = false,
    IReadOnlyList<Guid>? EvidenceFieldIds = null,
    IReadOnlyList<AssistantLabEvidenceReferenceDto>? LabEvidence = null)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<Guid>? DiaryEvidenceFieldIds { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AssistantPlanningHubEvidenceDto>? PlanningHubEvidence { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantRevisionComparisonContextDto? RevisionComparisonContext { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantTaskDefinitionContextDto? TaskDefinitionContext { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantMlsnZoneContextDto? MlsnZoneContext { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantAlternativeBasisReviewDto? AlternativeBasisReview { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? FieldApplicationContinuationId { get; init; }
}

public sealed record AssistantMlsnZoneContextDto(AssistantPlannerContextDto Planner, string Instruction);
public sealed record AssistantAlternativeBasisReviewDto(AssistantPlannerContextDto Planner, string FormStamp);
public sealed record AssistantPlanningHubEvidenceDto(int Year, string Fingerprint);
public sealed record AssistantRevisionComparisonContextDto(Guid? PlanPubId, int? Year);
public sealed record AssistantFrozenSourceContextDto(string Kind, Guid SnapshotId);
public sealed record AssistantTaskDefinitionContextDto(Guid? TaskPubId);

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
    AssistantNavigationSnapshotDto? Navigation = null)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FieldResearchReviewName { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantFrozenSourceContextDto? FrozenSourceComparison { get; init; }
}

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
public sealed record AssistantImageDto(string Base64)
{
    public const int MaximumReportPdfBytes = 10 * 1024 * 1024;
}

public sealed record SupportAssistantLinkDto(
    string Label,
    string Url,
    string? Description = null,
    bool IsProductMatch = false,
    string? ProductMatchName = null);

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
    IReadOnlyList<AmsRecords.Products.ProductsDtos.ProductWebsiteImportMatchDto>? ProductMatches = null,
    IReadOnlyList<Guid>? EvidenceFieldIds = null,
    IReadOnlyList<AssistantLabEvidenceReferenceDto>? LabEvidence = null)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? FieldApplicationContinuationId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool AwaitingProductCatalogueChoice { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool AwaitingAlternativeBasis { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantMlsnZoneContextDto? MlsnZoneContext { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantAlternativeBasisReviewDto? AlternativeBasisReview { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<Guid>? DiaryEvidenceFieldIds { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AssistantPlanningHubEvidenceDto>? PlanningHubEvidence { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantRevisionComparisonContextDto? RevisionComparisonContext { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantTaskDefinitionContextDto? TaskDefinitionContext { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<AssistantDiagnosticReviewSourceDto>? DiagnosticReviewSources { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool OpenDiagnosticReview { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Guid? RefreshProductListId { get; init; }
}

public sealed record AssistantDiagnosticReviewSourceDto(Guid FieldPubId, Guid DiagnosticRequestPubId,
    string FieldName, string? SurfaceName, string DiagnosisTitle, DateTime OccurredAtUtc);

public sealed record AssistantLabEvidenceReferenceDto(Guid FieldPubId, string Tool,
    Guid? ResultPubId = null, Guid? DocumentPubId = null, string? Label = null)
{
    public Guid? AnalytePubId { get; init; }
    public string? WaterResultCode { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantSoilTimelineReferenceDto? SoilTimeline { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantWaterTimelineReferenceDto? WaterTimeline { get; init; }
}

public sealed record AssistantWaterTimelineReferenceDto(string Title, string ResultCode, DateOnly? From, DateOnly? To);

public sealed record AssistantSoilTimelineReferenceDto(Guid AreaId, Guid? SurfaceId, int? Hole,
    string? Method, DateOnly? From, DateOnly? To, AssistantSoilChartContextDto? Chart);

public sealed record AssistantLabReportSourceDto(Guid DocumentPubId, Guid FieldPubId, string FileName,
    string MimeType, int ByteLength, int PageNumber, int PageCount);

public static class AssistantActionKinds
{
    public const string CreateProduct = "create_product";
    public const string RecordMeasurement = "record_measurement";
    public const string ReviewDollarSpotDewRemoval = "review_dollar_spot_dew_removal";
    public const string ReviewSnagAssignment = "review_snag_assignment";
    public const string ReviewSnagPhoto = "review_snag_photo";
    public const string FillForm = "fill_form";
    public const string Navigate = "navigate";
    public const string ImportLab = "import_lab";
    public const string AppendSoilImportReading = "append_soil_import_reading";
    public const string AppendWaterImportReading = "append_water_import_reading";
    public const string ViewLabReport = "view_lab_report";
    public const string ViewProduct = "view_product";
    public const string ViewDiagnosticReview = "view_diagnostic_review";
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
    bool UseCurrentMeasurementSelection = false)
{
    public AssistantLabReportPageContextDto? LabReportPage { get; init; }
    public Guid? LabResultPubId { get; init; }
    public Guid? SelectedProductPubId { get; init; }
    public AssistantTdrChartContextDto? TdrChart { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantClippingChartContextDto? ClippingChart { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantSoilChartContextDto? SoilChart { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AssistantIrrigationDesignContextDto? IrrigationDesign { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateOnly? GtsAsOfDate { get; init; }
}

public sealed record AssistantLabReportPageContextDto(Guid DocumentPubId, int PageNumber);

public sealed record AssistantTdrChartContextDto(
    [property: JsonRequired] DateOnly FromDayUtc,
    [property: JsonRequired] DateOnly ToDayUtc,
    Guid? SurfacePubId = null,
    Guid? SessionId = null);

public sealed record AssistantClippingChartContextDto(
    [property: JsonRequired] DateOnly FromDayUtc,
    [property: JsonRequired] DateOnly ToDayUtc,
    Guid? AreaPubId = null);

public sealed record AssistantSoilChartContextDto(
    [property: JsonRequired] IReadOnlyList<Guid> VisibleAnalytePubIds,
    Guid? ThresholdAnalytePubId = null);

public sealed record AssistantIrrigationDesignContextDto(Guid SurfacePubId, IReadOnlyList<Guid> ScenarioPubIds)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PreviewToken { get; init; }
}

public sealed record AssistantPlannerContextDto(int? Year = null, Guid? PlanPubId = null,
    Guid? RevisionPubId = null, Guid? ZonePubId = null);

// Advisory match calculated on the device. Never conveys permission or raw coordinates.
public sealed record AssistantDeviceLocationDto(Guid NearbySurfaceId, DateTimeOffset CapturedAtUtc);
