namespace AmsRecords.Chemicals;

public static class ChemicalDtos
{
    public sealed record ChemicalDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("chemName")] string ChemName,
        [property: JsonPropertyName("chemForm")] string? ChemForm,
        [property: JsonPropertyName("isElement")] bool IsElemental,
        [property: JsonPropertyName("chemTypePubId")] Guid ChemTypePubId,
        [property: JsonPropertyName("chemTypeName")] string ChemTypeName,
        [property: JsonPropertyName("unitTypePubId")] Guid? UnitTypePubId,
        [property: JsonPropertyName("unitTypeName")] string UnitTypeName,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record ChemicalMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("chemName")] string ChemName,
        [property: JsonPropertyName("unitTypePubId")] Guid? UnitTypePubId
    );

    public sealed record ChemicalCreateDto(
        [property: JsonPropertyName("chemName")] string ChemName,
        [property: JsonPropertyName("chemForm")] string? ChemForm,
        [property: JsonPropertyName("isElement")] bool IsElemental,
        [property: JsonPropertyName("chemTypePubId")] Guid ChemTypePubId,
        [property: JsonPropertyName("unitTypePubId")] Guid? UnitTypePubId
    );

    public sealed record ChemicalUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("chemName")] string ChemName,
        [property: JsonPropertyName("chemForm")] string? ChemForm,
        [property: JsonPropertyName("isElement")] bool IsElemental,
        [property: JsonPropertyName("chemTypePubId")] Guid ChemTypePubId,
        [property: JsonPropertyName("unitTypePubId")] Guid? UnitTypePubId,
        [property: JsonPropertyName("active")] bool Active
    );
}
