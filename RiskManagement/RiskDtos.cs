using AmsRecords.Messages;
using static AmsRecords.RiskManagement.AnthracnoseDtos;
using static AmsRecords.RiskManagement.DewPointDtos;
using static AmsRecords.RiskManagement.DollarSpotDtos;
using static AmsRecords.RiskManagement.FrostDtos;
using static AmsRecords.RiskManagement.HeatStressDtos;
using static AmsRecords.RiskManagement.MicrodochiumDtos;
using static AmsRecords.RiskManagement.PythiumDtos;
using static Lib.Enums.RiskLevels;

namespace AmsRecords.RiskManagement;

public static class RiskDtos
{
    public readonly record struct HourPoint(DateTimeOffset HourUtc, decimal TempC, short Rh);
    public readonly record struct DailyMean(DateOnly Date, decimal AvgTempC, decimal AvgRh, int HourCount);
    public record RiskWeatherPackage(IReadOnlyList<HourPoint> Hourly, IReadOnlyList<DailyMean> DailyMeans);

    public record RiskSummaryDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("generatedAtUtc")] DateTimeOffset GeneratedAtUtc,
        [property: JsonPropertyName("dollarSpot")] DollarSpotNDayRiskDto? DollarSpot,
        [property: JsonPropertyName("dewPoint")] DewPointNDaysDto? DewPoint,
        [property: JsonPropertyName("pythium")] PythiumBlightNDaysDto? Pythium,
        [property: JsonPropertyName("microdochium")] MicrodochiumNDaysDto? Microdochium,
        [property: JsonPropertyName("anthracnose")] Anthracnose3DayRiskDto? Anthracnose,
        [property: JsonPropertyName("frost")] FrostNDaysDto? Frost,
        [property: JsonPropertyName("heatStress")] HeatStressNDaysRiskDto? HeatStress
    );

    public sealed record RiskMiniDto(
        [property: JsonPropertyName("level")] RiskLevel Level,
        [property: JsonPropertyName("items")] AppMessageDto[] Items,
        [property: JsonPropertyName("trend")] string? Trend = null
    );

    public sealed record RiskHistoryDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fromUtc")] DateTime FromUtc,
        [property: JsonPropertyName("toUtc")] DateTime ToUtc,
        [property: JsonPropertyName("series")] IReadOnlyList<RiskHistorySeriesDto> Series,
        [property: JsonPropertyName("insights")] IReadOnlyList<RiskHistoryInsightDto> Insights
    );

    public sealed record RiskHistorySeriesDto(
        [property: JsonPropertyName("riskKey")] string RiskKey,
        [property: JsonPropertyName("labelKey")] string LabelKey,
        [property: JsonPropertyName("points")] IReadOnlyList<RiskHistoryPointDto> Points,
        [property: JsonPropertyName("metrics")] RiskHistoryMetricsDto Metrics
    );

    public sealed record RiskHistoryPointDto(
        [property: JsonPropertyName("dateLocal")] DateOnly DateLocal,
        [property: JsonPropertyName("generatedAtUtc")] DateTime GeneratedAtUtc,
        [property: JsonPropertyName("level")] RiskLevel Level,
        [property: JsonPropertyName("levelScore")] int LevelScore,
        [property: JsonPropertyName("valuePct")] decimal? ValuePct
    );

    public sealed record RiskHistoryMetricsDto(
        [property: JsonPropertyName("daysWithData")] int DaysWithData,
        [property: JsonPropertyName("lowDays")] int LowDays,
        [property: JsonPropertyName("moderateDays")] int ModerateDays,
        [property: JsonPropertyName("highDays")] int HighDays,
        [property: JsonPropertyName("elevatedDays")] int ElevatedDays,
        [property: JsonPropertyName("longestElevatedRunDays")] int LongestElevatedRunDays,
        [property: JsonPropertyName("firstElevatedDate")] DateOnly? FirstElevatedDate,
        [property: JsonPropertyName("lastElevatedDate")] DateOnly? LastElevatedDate,
        [property: JsonPropertyName("maxLevel")] RiskLevel MaxLevel
    );

    public sealed record RiskHistoryInsightDto(
        [property: JsonPropertyName("riskKey")] string? RiskKey,
        [property: JsonPropertyName("severity")] string Severity,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("message")] string Message,
        [property: JsonPropertyName("seasonStartMonth")] int? SeasonStartMonth = null,
        [property: JsonPropertyName("seasonEndMonth")] int? SeasonEndMonth = null,
        [property: JsonPropertyName("seasonDay")] int? SeasonDay = null,
        [property: JsonPropertyName("highDays")] int? HighDays = null,
        [property: JsonPropertyName("moderateDays")] int? ModerateDays = null,
        [property: JsonPropertyName("longestElevatedRunDays")] int? LongestElevatedRunDays = null
    );
}
