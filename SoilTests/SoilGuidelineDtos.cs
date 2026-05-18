namespace AmsRecords.SoilTests;

public static class SoilGuidelineDtos
{
    public sealed record SoilGuidelineStandardDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ownerType")] int OwnerType,
        [property: JsonPropertyName("ownerPubId")] Guid OwnerPubId,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("version")] string? Version,
        [property: JsonPropertyName("sourceUrl")] string? SourceUrl,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record SoilGuidelineStandardUpsertDto(
        [property: JsonPropertyName("ownerType")] int OwnerType,
        [property: JsonPropertyName("ownerPubId")] Guid OwnerPubId,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("version")] string? Version,
        [property: JsonPropertyName("sourceUrl")] string? SourceUrl,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record SoilStandardAnalyteGuidelineDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("standardPubId")] Guid StandardPubId,
        [property: JsonPropertyName("standardCode")] string StandardCode,
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("chemName")] string ChemName,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("unitShort")] string? UnitShort,
        [property: JsonPropertyName("minValue")] decimal? MinValue,
        [property: JsonPropertyName("optValue")] decimal? OptValue,
        [property: JsonPropertyName("maxValue")] decimal? MaxValue,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record SoilStandardAnalyteGuidelineUpsertDto(
        [property: JsonPropertyName("standardPubId")] Guid StandardPubId,
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("minValue")] decimal? MinValue,
        [property: JsonPropertyName("optValue")] decimal? OptValue,
        [property: JsonPropertyName("maxValue")] decimal? MaxValue,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record SoilGuidelineStandardSelectionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ownerType")] int OwnerType,
        [property: JsonPropertyName("ownerPubId")] Guid OwnerPubId,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("standardPubId")] Guid StandardPubId,
        [property: JsonPropertyName("standardCode")] string StandardCode,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record SoilGuidelineStandardSelectionUpsertDto(
        [property: JsonPropertyName("ownerType")] int OwnerType,
        [property: JsonPropertyName("ownerPubId")] Guid OwnerPubId,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("standardPubId")] Guid StandardPubId,
        [property: JsonPropertyName("active")] bool Active
    );

    // Effective result row (baseline + override provenance)
    public sealed record SoilEffectiveAnalyteGuidelineDto(
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("chemName")] string ChemName,
        [property: JsonPropertyName("methodCode")] string? MethodCode,
        [property: JsonPropertyName("unitPubId")] Guid? UnitPubId,
        [property: JsonPropertyName("unitShort")] string? UnitShort,
        [property: JsonPropertyName("minValue")] decimal? MinValue,
        [property: JsonPropertyName("optValue")] decimal? OptValue,
        [property: JsonPropertyName("maxValue")] decimal? MaxValue,

        // provenance (what baseline was chosen + whether override applied)
        [property: JsonPropertyName("baselineStandardCode")] string? BaselineStandardCode,
        [property: JsonPropertyName("baselineOwnerType")] int? BaselineOwnerType,
        [property: JsonPropertyName("baselineOwnerPubId")] Guid? BaselineOwnerPubId,
        [property: JsonPropertyName("overrideOwnerType")] int? OverrideOwnerType,
        [property: JsonPropertyName("overrideOwnerPubId")] Guid? OverrideOwnerPubId
    );


}
