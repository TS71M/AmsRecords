using AmsRecords.Messages;
using AmsRecords.Units;

namespace AmsRecords.RiskManagement;

public static class HeatStressDtos
{
    public record HeatStressDayDto(
        [property: JsonPropertyName("dateLocal")] DateOnly DateLocal,
        [property: JsonPropertyName("hasData")] bool HasData,
        [property: JsonPropertyName("maxTempC")] decimal? MaxTempC,
        [property: JsonPropertyName("maxTemp")] UnitValueDto? MaxTemp,
        [property: JsonPropertyName("avgTempC")] decimal? AvgTempC,
        [property: JsonPropertyName("avgTemp")] UnitValueDto? AvgTemp,
        [property: JsonPropertyName("hoursAbove30C")] int? HoursAbove30C,
        [property: JsonPropertyName("riskLevel")] string RiskLevel,
        [property: JsonPropertyName("isStressLikely")] bool IsStressLikely
    );

    public record HeatStressNDaysRiskDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("generatedAtUtc")] DateTimeOffset GeneratedAtUtc,
        [property: JsonPropertyName("days")] IReadOnlyList<HeatStressDayDto> Days,
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

        [JsonPropertyName("moderateHoursThreshold")]
        public int? ModerateHoursThreshold { get; init; }

        [JsonPropertyName("highHoursThreshold")]
        public int? HighHoursThreshold { get; init; }
    }
}
