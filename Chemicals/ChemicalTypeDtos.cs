namespace AmsRecords.Chemicals;

public static class ChemicalTypeDtos
{
    public sealed record ChemicalTypeDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("chemTypeName")] string ChemTypeName,
        [property: JsonPropertyName("defaultBlockInProtectedZone")] bool DefaultBlockInProtectedZone,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record ChemicalTypeMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("chemTypeName")] string ChemTypeName
    );

    public sealed record ChemicalTypeCreateDto(
        [property: JsonPropertyName("chemTypeName")] string ChemTypeName,
        [property: JsonPropertyName("defaultBlockInProtectedZone")] bool DefaultBlockInProtectedZone
    );

    public sealed record ChemicalTypeUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("chemTypeName")] string ChemTypeName,
        [property: JsonPropertyName("defaultBlockInProtectedZone")] bool DefaultBlockInProtectedZone,
        [property: JsonPropertyName("active")] bool Active
    );
}
