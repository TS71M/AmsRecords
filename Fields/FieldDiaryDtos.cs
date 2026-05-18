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
        public const string ManualNote = "ManualNote";
        public const string AttachmentAdded = "AttachmentAdded";
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

    public sealed record FieldDiaryTimelineQueryDto(
        [property: JsonPropertyName("fromUtc")] DateTime? FromUtc,
        [property: JsonPropertyName("toUtc")] DateTime? ToUtc,
        [property: JsonPropertyName("category")] string? Category,
        [property: JsonPropertyName("severity")] string? Severity,
        [property: JsonPropertyName("source")] string? Source,
        [property: JsonPropertyName("importantOnly")] bool ImportantOnly = false
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
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record FieldDiaryTimelineDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("query")] FieldDiaryTimelineQueryDto Query,
        [property: JsonPropertyName("items")] IReadOnlyList<FieldDiaryItemDto> Items
    );
}
