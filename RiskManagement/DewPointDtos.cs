using AmsRecords.Messages;
using AmsRecords.Units;
using static Lib.Enums.RiskLevels;

namespace AmsRecords.RiskManagement;

public static class DewPointDtos
{
    public record DewPointNDaysDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("generatedAtUtc")] DateTimeOffset GeneratedAtUtc,
        [property: JsonPropertyName("days")] IReadOnlyList<DewPointDayDto> Days,
        [property: JsonPropertyName("messageCategories")] AppMessageCategoryDto[] MessageCategories
    );

    public record DewPointDayDto(
        [property: JsonPropertyName("dateLocal")] DateOnly DateLocal,
        [property: JsonPropertyName("hasData")] bool HasData,
        [property: JsonPropertyName("avgDewPointC")] decimal? AvgDewPointC,
        [property: JsonPropertyName("avgDewPoint")] UnitValueDto? AvgDewPoint,
        [property: JsonPropertyName("minDewPointDepressionC")] decimal? MinDewPointDepressionC,
        [property: JsonPropertyName("minDewPointDepression")] UnitValueDto? MinDewPointDepression,
        [property: JsonPropertyName("hoursNearDew")] int? HoursNearDew,
        [property: JsonPropertyName("riskLevel")] RiskLevel RiskLevel // "low"|"moderate"|"high"|"na"
    );
}
