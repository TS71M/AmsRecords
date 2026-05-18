using AmsRecords.Messages;
using static Lib.Enums.RiskLevels;

namespace AmsRecords.RiskManagement;

public static class DollarSpotDtos
{
    public record DollarSpotNDayRiskDto(
        [property: JsonPropertyName("riskKey")] string RiskKey,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("generatedAtUtc")] DateTimeOffset GeneratedAtUtc,
        [property: JsonPropertyName("days")] IReadOnlyList<DollarSpotDayRiskDto> Days,
        [property: JsonPropertyName("messageCategories")] AppMessageCategoryDto[] MessageCategories,
        [property: JsonPropertyName("hoursHave")] int? HoursHave,
        [property: JsonPropertyName("hoursNeed")] int? HoursNeed,
        [property: JsonPropertyName("hoursCoveragePct")] int? HoursCoveragePct,
        [property: JsonPropertyName("consecutiveDaysHave")] int? ConsecutiveDaysHave,
        [property: JsonPropertyName("consecutiveDaysNeed")] int? ConsecutiveDaysNeed,
        [property: JsonPropertyName("consecutiveDaysCoveragePct")] int? ConsecutiveDaysCoveragePct,
        [property: JsonPropertyName("missingReason")] AppMessageDto? MissingReason
    );

    public record DollarSpotDayRiskDto(
        [property: JsonPropertyName("dateLocal")] DateOnly DateLocal,
        [property: JsonPropertyName("hasData")] bool HasData,
        [property: JsonPropertyName("probabilityPct")] decimal? ProbabilityPct,
        [property: JsonPropertyName("meanAtC")] decimal? MeanAtC,
        [property: JsonPropertyName("meanRhPct")] decimal? MeanRhPct,
        [property: JsonPropertyName("riskLevel")] RiskLevel Level,
        [property: JsonPropertyName("isAction")] bool IsAction,
        [property: JsonPropertyName("messages")] AppMessageDto[] Messages
    );

    public record DollarSpotMetricsDto(
        [property: JsonPropertyName("favorableHours")] int FavorableHours,
        [property: JsonPropertyName("avgTempC")] decimal AvgTempC,
        [property: JsonPropertyName("avgRh")] decimal AvgRh,
        [property: JsonPropertyName("windowHours")] int WindowHours
        );

    public record RiskIndicatorDto(
        [property: JsonPropertyName("riskKey")] string RiskKey,
        [property: JsonPropertyName("level")] RiskLevel Level,
        [property: JsonPropertyName("score")] decimal Score,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("metrics")] DollarSpotMetricsDto Metrics,
        [property: JsonPropertyName("notes")] string[] Notes
    );
}
