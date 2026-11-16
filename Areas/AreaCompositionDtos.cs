namespace AmsRecords.Areas;

public static class AreaCompositionDtos
{
    [method: JsonConstructor]
    public sealed record AreaGrassSpeciesRowDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("grassSpeciesPubId")] Guid GrassSpeciesPubId,
        [property: JsonPropertyName("percent")] decimal Percent,
        [property: JsonPropertyName("speciesName"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? SpeciesName = null
    )
    {
        public AreaGrassSpeciesRowDto(Guid PubId, Guid GrassSpeciesPubId, decimal Percent)
            : this(PubId, GrassSpeciesPubId, Percent, null)
        {
        }

        public void Deconstruct(out Guid PubId, out Guid GrassSpeciesPubId, out decimal Percent)
            => (PubId, GrassSpeciesPubId, Percent) = (this.PubId, this.GrassSpeciesPubId, this.Percent);
    }

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