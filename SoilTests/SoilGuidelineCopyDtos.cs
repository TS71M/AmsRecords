namespace AmsRecords.SoilTests;

public static class SoilGuidelineCopyDtos
{
    public sealed record CopyStandardToOverridesRequestDto(
        [property: JsonPropertyName("ownerType")] int OwnerType,
        [property: JsonPropertyName("ownerPubId")] Guid OwnerPubId,
        [property: JsonPropertyName("methodCode")] string? MethodCode
    );

    public sealed record CopyStandardToOverridesResultDto(
        [property: JsonPropertyName("ownerType")] int OwnerType,
        [property: JsonPropertyName("ownerPubId")] Guid OwnerPubId,
        [property: JsonPropertyName("methodCodeNormalized")] string MethodCodeNormalized,
        [property: JsonPropertyName("standardPubId")] Guid StandardPubId,
        [property: JsonPropertyName("standardName")] string StandardName,
        [property: JsonPropertyName("created")] int Created,
        [property: JsonPropertyName("updated")] int Updated,
        [property: JsonPropertyName("skipped")] int Skipped
    );
}
