namespace AmsRecords.AreaGroups;

public static class AreaGroupDtos
{
    public sealed record AreaGroupDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("supportsSurfaces")] bool SupportsSurfaces,
        [property: JsonPropertyName("supportsTurfDefaults")] bool SupportsTurfDefaults,
        [property: JsonPropertyName("supportsComposition")] bool SupportsComposition,
        [property: JsonPropertyName("sortOrder")] int SortOrder
    );

    public sealed record AreaGroupMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name
    );

    public sealed record AreaGroupCreateDto(
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("supportsSurfaces")] bool SupportsSurfaces,
        [property: JsonPropertyName("supportsTurfDefaults")] bool SupportsTurfDefaults,
        [property: JsonPropertyName("supportsComposition")] bool SupportsComposition,
        [property: JsonPropertyName("sortOrder")] int SortOrder
    );

    public sealed record AreaGroupUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("supportsSurfaces")] bool SupportsSurfaces,
        [property: JsonPropertyName("supportsTurfDefaults")] bool SupportsTurfDefaults,
        [property: JsonPropertyName("supportsComposition")] bool SupportsComposition,
        [property: JsonPropertyName("sortOrder")] int SortOrder
    );
}
