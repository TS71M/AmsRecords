namespace AmsRecords.SoilTests;

public sealed class SoilAnalyteDtos
{
    public sealed record SoilAnalyteGuidelineDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("chemName")] string ChemName,
        [property: JsonPropertyName("ownerType")] int OwnerType,
        [property: JsonPropertyName("ownerPubId")] Guid OwnerPubId,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("unitShort")] string? UnitShort,
        [property: JsonPropertyName("minValue")] decimal? MinValue,
        [property: JsonPropertyName("optValue")] decimal? OptValue,
        [property: JsonPropertyName("maxValue")] decimal? MaxValue,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record SoilAnalyteGuidelineUpsertDto(
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("ownerType")] int OwnerType,
        [property: JsonPropertyName("ownerPubId")] Guid OwnerPubId,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("minValue")] decimal? MinValue,
        [property: JsonPropertyName("optValue")] decimal? OptValue,
        [property: JsonPropertyName("maxValue")] decimal? MaxValue,
        [property: JsonPropertyName("active")] bool Active
    );

}
