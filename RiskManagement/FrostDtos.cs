using AmsRecords.Messages;
using AmsRecords.Units;

namespace AmsRecords.RiskManagement;

public static class FrostDtos
{
    public record FrostNDaysDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("generatedAtUtc")] DateTimeOffset GeneratedAtUtc,
        [property: JsonPropertyName("days")] IReadOnlyList<FrostDayDto> Days,
        [property: JsonPropertyName("thresholdTemp")] UnitValueDto? ThresholdTemp,
        [property: JsonPropertyName("messageCategories")] AppMessageCategoryDto[] MessageCategories
    )
    {
        [JsonPropertyName("highThresholdTemp")]
        public UnitValueDto? HighThresholdTemp { get; init; }

        [JsonPropertyName("moderateThresholdTempC")]
        public decimal? ModerateThresholdTempC { get; init; }

        [JsonPropertyName("highThresholdTempC")]
        public decimal? HighThresholdTempC { get; init; }
    }

    public record FrostDayDto(
        [property: JsonPropertyName("dateLocal")] DateOnly DateLocal,
        [property: JsonPropertyName("hasData")] bool HasData,
        [property: JsonPropertyName("minTempC")] decimal? MinTempC,
        [property: JsonPropertyName("minTemp")] UnitValueDto? MinTemp,
        [property: JsonPropertyName("minDewPointDepressionC")] decimal? MinDewPointDepressionC,
        [property: JsonPropertyName("minDewPointDepression")] UnitValueDto? MinDewPointDepression,
        [property: JsonPropertyName("hoursBelow2C")] int? HoursBelow2C,
        [property: JsonPropertyName("riskLevel")] string RiskLevel,
        [property: JsonPropertyName("isFrostLikely")] bool IsFrostLikely
    );
}
