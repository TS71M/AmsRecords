using AmsRecords.Messages;

namespace AmsRecords.RiskManagement;

public static class PythiumDtos
{
    public sealed record PythiumBlightNDaysDto(
        [property: JsonPropertyName("riskKey")] string RiskKey,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("generatedAtUtc")] DateTimeOffset GeneratedAtUtc,
        [property: JsonPropertyName("days")] List<PythiumBlightDayDto> Days,
        [property: JsonPropertyName("messageCategories")] AppMessageCategoryDto[] MessageCategories
    );

    public sealed record PythiumBlightDayDto(
        [property: JsonPropertyName("dateLocal")] DateOnly DateLocal,
        [property: JsonPropertyName("hasData")] bool HasData,
        [property: JsonPropertyName("hoursRh90OrMore")] int? HoursRh90OrMore,
        [property: JsonPropertyName("hoursTemp20OrMore")] int? HoursTemp20OrMore,
        [property: JsonPropertyName("hoursTemp22OrMore")] int? HoursTemp22OrMore,
        [property: JsonPropertyName("minNightTempC")] decimal? MinNightTempC,
        [property: JsonPropertyName("avgNightTempC")] decimal? AvgNightTempC,
        [property: JsonPropertyName("hoursNearDew")] int? HoursNearDew,
        [property: JsonPropertyName("riskLevel")] string RiskLevel,
        [property: JsonPropertyName("isOutbreakLikely")] bool IsOutbreakLikely
    );
}