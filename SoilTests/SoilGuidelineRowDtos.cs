namespace AmsRecords.SoilTests;

public static class SoilGuidelineRowDtos
{
    public sealed record SoilStandardAnalyteGuidelineRowDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("standardPubId")] Guid StandardPubId,
        [property: JsonPropertyName("methodCode")] string? MethodCode, // null == ""
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("chemName")] string ChemName,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("unitShort")] string? UnitShort,
        [property: JsonPropertyName("minValue")] decimal? MinValue,
        [property: JsonPropertyName("optValue")] decimal? OptValue,
        [property: JsonPropertyName("maxValue")] decimal? MaxValue,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record SoilStandardAnalyteGuidelineRowUpsertDto(
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("minValue")] decimal? MinValue,
        [property: JsonPropertyName("optValue")] decimal? OptValue,
        [property: JsonPropertyName("maxValue")] decimal? MaxValue,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record SoilStandardAnalyteGuidelineBulkUpsertDto(
        [property: JsonPropertyName("rows")] List<SoilStandardAnalyteGuidelineRowUpsertDto> Rows
    );

    public sealed record SoilStandardAnalyteGuidelineBulkUpsertResultDto(
        [property: JsonPropertyName("created")] int Created,
        [property: JsonPropertyName("updated")] int Updated,
        [property: JsonPropertyName("skipped")] int Skipped
    );
}
