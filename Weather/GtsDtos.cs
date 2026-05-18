using AmsRecords.Messages;

namespace AmsRecords.Weather;

public static class GtsDtos
{
    public sealed record GtsMiniDto(
        [property: JsonPropertyName("year")] short Year,
        [property: JsonPropertyName("asOfDate")] DateOnly AsOfDate,
        [property: JsonPropertyName("currentGts")] decimal CurrentGts,
        [property: JsonPropertyName("stageCode")] string StageCode,
        [property: JsonPropertyName("stageName")] string StageName,
        [property: JsonPropertyName("thresholds")] IReadOnlyList<GtsThresholdHitDto> Thresholds,
        [property: JsonPropertyName("weedTimings")] IReadOnlyList<GtsTimingDto> WeedTimings,
        [property: JsonPropertyName("completeness")] GtsDataCompletenessDto? Completeness = null
    );

    public sealed record GtsDataCompletenessDto(
        [property: JsonPropertyName("isComplete")] bool IsComplete,
        [property: JsonPropertyName("requiredFromDate")] DateOnly RequiredFromDate,
        [property: JsonPropertyName("requiredToDate")] DateOnly RequiredToDate,
        [property: JsonPropertyName("requiredDayCount")] int RequiredDayCount,
        [property: JsonPropertyName("availableDayCount")] int AvailableDayCount,
        [property: JsonPropertyName("missingDayCount")] int MissingDayCount,
        [property: JsonPropertyName("firstAvailableDate")] DateOnly? FirstAvailableDate,
        [property: JsonPropertyName("lastAvailableDate")] DateOnly? LastAvailableDate,
        [property: JsonPropertyName("backfillRateDaysPerRun")] int BackfillRateDaysPerRun,
        [property: JsonPropertyName("estimatedReadyDays")] int EstimatedReadyDays
    );

    public sealed record GtsThresholdHitDto(
        [property: JsonPropertyName("threshold")] decimal Threshold,
        [property: JsonPropertyName("hitDate")] DateOnly? HitDate,
        [property: JsonPropertyName("isReached")] bool IsReached
    );

    public sealed record GtsTimingDto(
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("category")] string Category,
        [property: JsonPropertyName("statusCode")] string StatusCode,
        [property: JsonPropertyName("statusName")] string StatusName,
        [property: JsonPropertyName("preferredFromGts")] decimal? PreferredFromGts,
        [property: JsonPropertyName("preferredToGts")] decimal? PreferredToGts,
        [property: JsonPropertyName("acceptableFromGts")] decimal? AcceptableFromGts,
        [property: JsonPropertyName("acceptableToGts")] decimal? AcceptableToGts,
        [property: JsonPropertyName("note")] string? Note,
        [property: JsonPropertyName("noteMessage")] AppMessageDto? NoteMessage
    );
}
