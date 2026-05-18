using AmsRecords.Messages;

namespace AmsRecords.RiskManagement;

public static class MicrodochiumDtos
{
    public sealed record MicrodochiumNDaysDto(
       [property: JsonPropertyName("riskKey")] string RiskKey,
       [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
       [property: JsonPropertyName("generatedAtUtc")] DateTimeOffset GeneratedAtUtc,
       [property: JsonPropertyName("days")] List<MicrodochiumDayDto> Days,
       [property: JsonPropertyName("messageCategories")] AppMessageCategoryDto[] MessageCategories
   );

    public sealed record MicrodochiumDayDto(
        [property: JsonPropertyName("dateLocal")] DateOnly DateLocal,
        [property: JsonPropertyName("hasData")] bool HasData,
        [property: JsonPropertyName("hoursRh90OrMore")] int? HoursRh90OrMore,
        [property: JsonPropertyName("hoursTempAboveZero")] int? HoursTempAboveZero,
        [property: JsonPropertyName("hoursTemp2To18")] int? HoursTemp2To18,
        [property: JsonPropertyName("hoursTemp5To12")] int? HoursTemp5To12,
        [property: JsonPropertyName("hoursNearDew")] int? HoursNearDew,
        [property: JsonPropertyName("minTempC")] decimal? MinTempC,
        [property: JsonPropertyName("maxTempC")] decimal? MaxTempC,
        [property: JsonPropertyName("riskLevel")] string RiskLevel,
        [property: JsonPropertyName("isInfectionWindowLikely")] bool IsInfectionWindowLikely
    );
}