namespace AmsRecords.Weather;

public static class WeatherBackfillStatsDtos
{
    public sealed record WeatherBackfillStatsDto(
        [property: JsonPropertyName("generatedAtUtc")] DateTime GeneratedAtUtc,
        [property: JsonPropertyName("historySupportedFromUtc")] DateTime HistorySupportedFromUtc,
        [property: JsonPropertyName("backfillTargetToUtc")] DateTime BackfillTargetToUtc,
        [property: JsonPropertyName("lastScheduledRunStartedAtUtc")] DateTime LastScheduledRunStartedAtUtc,
        [property: JsonPropertyName("lastScheduledRunWindowToUtc")] DateTime LastScheduledRunWindowToUtc,
        [property: JsonPropertyName("totalFields")] int TotalFields,
        [property: JsonPropertyName("completedFields")] int CompletedFields,
        [property: JsonPropertyName("activeCursorFields")] int ActiveCursorFields,
        [property: JsonPropertyName("fieldsWithoutCursor")] int FieldsWithoutCursor,
        [property: JsonPropertyName("daysAddedInLastRunWindow")] int DaysAddedInLastRunWindow,
        [property: JsonPropertyName("rows")] IReadOnlyList<WeatherBackfillFieldStatsDto> Rows);

    public sealed record WeatherBackfillFieldStatsDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("ibuName")] string IbuName,
        [property: JsonPropertyName("hasCoordinates")] bool HasCoordinates,
        [property: JsonPropertyName("cursorExists")] bool CursorExists,
        [property: JsonPropertyName("cursorCompleted")] bool CursorCompleted,
        [property: JsonPropertyName("cursorNextDateUtc")] DateTime? CursorNextDateUtc,
        [property: JsonPropertyName("cursorMinDateUtc")] DateTime? CursorMinDateUtc,
        [property: JsonPropertyName("cursorUpdatedAtUtc")] DateTime? CursorUpdatedAtUtc,
        [property: JsonPropertyName("oldestContiguousBackfilledDateUtc")] DateTime? OldestContiguousBackfilledDateUtc,
        [property: JsonPropertyName("earliestStoredSummaryDateUtc")] DateTime? EarliestStoredSummaryDateUtc,
        [property: JsonPropertyName("latestStoredSummaryDateUtc")] DateTime? LatestStoredSummaryDateUtc,
        [property: JsonPropertyName("storedSummaryDayCount")] int StoredSummaryDayCount,
        [property: JsonPropertyName("daysAddedInLastRunWindow")] int DaysAddedInLastRunWindow,
        [property: JsonPropertyName("latestAddedSummaryDateUtc")] DateTime? LatestAddedSummaryDateUtc);
}
