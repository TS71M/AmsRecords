namespace AmsRecords.Areas;

public static class AreaCompositionDtos
{
    public sealed record AreaGrassSpeciesRowDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("grassSpeciesPubId")] Guid GrassSpeciesPubId,
        [property: JsonPropertyName("percent")] decimal Percent
    );

    public sealed record AreaSoilTypeRowDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("soilTypePubId")] Guid SoilTypePubId,
        [property: JsonPropertyName("percent")] decimal Percent
    );

    // For “replace composition” POST/PUT
    public sealed record AreaCompositionUpsertDto(
        [property: JsonPropertyName("grassSpecies")] IReadOnlyList<AreaGrassSpeciesUpsertDto> GrassSpecies,
        [property: JsonPropertyName("soilTypes")] IReadOnlyList<AreaSoilTypeUpsertDto> SoilTypes
    );

    public sealed record AreaGrassSpeciesUpsertDto(
        [property: JsonPropertyName("grassSpeciesPubId")] Guid GrassSpeciesPubId,
        [property: JsonPropertyName("percent")] decimal Percent
    );

    public sealed record AreaSoilTypeUpsertDto(
        [property: JsonPropertyName("soilTypePubId")] Guid SoilTypePubId,
        [property: JsonPropertyName("percent")] decimal Percent
    );
}