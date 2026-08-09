namespace AmsRecords.Fields;

public static class FieldDiaryDtos
{
    public static class Categories
    {
        public const string WorkExecuted = "WorkExecuted";
        public const string AiDiagnosticRequested = "AiDiagnosticRequested";
        public const string AiDiagnosticResult = "AiDiagnosticResult";
        public const string AiDiagnosticCorrection = "AiDiagnosticCorrection";
        public const string AiDiagnosticReviewConfirmed = "AiDiagnosticReviewConfirmed";
        public const string RiskThresholdReached = "RiskThresholdReached";
        public const string RiskThresholdCleared = "RiskThresholdCleared";
        public const string NearbyRiskAdvisory = "NearbyRiskAdvisory";
        public const string WeatherExtremeStarted = "WeatherExtremeStarted";
        public const string WeatherExtremeEnded = "WeatherExtremeEnded";
        public const string ManualEvent = "ManualEvent";
        public const string ManualNote = "ManualNote";
        public const string AttachmentAdded = "AttachmentAdded";
        public const string LeafNitrateMeasured = "LeafNitrateMeasured";
        public const string ClippingVolumeMeasured = "ClippingVolumeMeasured";
        public const string GreenSpeedMeasured = "GreenSpeedMeasured";
        public const string CuttingHeightMeasured = "CuttingHeightMeasured";
    }

    public static class Sources
    {
        public const string WebApp = "WebApp";
        public const string MobileApp = "MobileApp";
        public const string RiskEngine = "RiskEngine";
        public const string WeatherEngine = "WeatherEngine";
        public const string UserManual = "UserManual";
        public const string System = "System";
    }

    public static class ManualEventTypes
    {
        public const string Observation = "observation";
        public const string Application = "application";
        public const string CulturalPractice = "cultural-practice";
        public const string Irrigation = "irrigation";
        public const string WeatherImpact = "weather-impact";
        public const string Other = "other";
    }

    public sealed record FieldDiaryManualEventUpsertDto(
        [property: JsonPropertyName("occurredAtUtc")] DateTime OccurredAtUtc,
        [property: JsonPropertyName("eventType")] string EventType,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("note")] string? Note,
        [property: JsonPropertyName("severity")] string Severity,
        [property: JsonPropertyName("areaPubId")] Guid? AreaPubId,
        [property: JsonPropertyName("surfacePubId")] Guid? SurfacePubId
    );

    public sealed record FieldDiaryTimelineQueryDto(
        [property: JsonPropertyName("fromUtc")] DateTime? FromUtc,
        [property: JsonPropertyName("toUtc")] DateTime? ToUtc,
        [property: JsonPropertyName("category")] string? Category,
        [property: JsonPropertyName("severity")] string? Severity,
        [property: JsonPropertyName("source")] string? Source,
        [property: JsonPropertyName("importantOnly")] bool ImportantOnly = false,
        [property: JsonPropertyName("categories")] string? Categories = null,
        [property: JsonPropertyName("skip")] int? Skip = null,
        [property: JsonPropertyName("take")] int? Take = null
    );

    public sealed record FieldDiaryItemDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("occurredAtUtc")] DateTime OccurredAtUtc,
        [property: JsonPropertyName("recordedAtUtc")] DateTime RecordedAtUtc,
        [property: JsonPropertyName("category")] string Category,
        [property: JsonPropertyName("eventCode")] string EventCode,
        [property: JsonPropertyName("source")] string Source,
        [property: JsonPropertyName("severity")] string Severity,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("detailsJson")] string? DetailsJson,
        [property: JsonPropertyName("relatedRiskKey")] string? RelatedRiskKey,
        [property: JsonPropertyName("relatedEntityType")] string? RelatedEntityType,
        [property: JsonPropertyName("relatedPubId")] Guid? RelatedPubId,
        [property: JsonPropertyName("createdByName")] string? CreatedByName,
        [property: JsonPropertyName("createdByPubId")] Guid? CreatedByPubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("translatedTitle")] string? TranslatedTitle = null,
        [property: JsonPropertyName("translatedNote")] string? TranslatedNote = null
    );

    public sealed record FieldDiaryTimelineDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("query")] FieldDiaryTimelineQueryDto Query,
        [property: JsonPropertyName("items")] IReadOnlyList<FieldDiaryItemDto> Items,
        [property: JsonPropertyName("totalCount")] int TotalCount = 0
    );
}
