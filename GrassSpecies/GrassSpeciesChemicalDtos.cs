using Lib.Flags;

namespace AmsRecords.GrassSpecies;

public static class GrassSpeciesChemicalDtos
{
    public sealed record GrassSpeciesChemicalDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("active")] bool Active,

        [property: JsonPropertyName("grassSpeciesPubId")] Guid GrassSpeciesPubId,
        [property: JsonPropertyName("grassSpeciesName")] string GrassSpeciesName,

        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("chemName")] string ChemName,

        [property: JsonPropertyName("useMode")] ChemUseMode UseMode,
        [property: JsonPropertyName("notes")] string? Notes
    );

    public sealed record GrassSpeciesChemicalCreateDto(
        [property: JsonPropertyName("grassSpeciesPubId")] Guid GrassSpeciesPubId,
        [property: JsonPropertyName("chemPubId")] Guid ChemPubId,
        [property: JsonPropertyName("useMode")] ChemUseMode UseMode,
        [property: JsonPropertyName("notes")] string? Notes,
        [property: JsonPropertyName("active")] bool Active = true
    );

    public sealed record GrassSpeciesChemicalUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("useMode")] ChemUseMode UseMode,
        [property: JsonPropertyName("notes")] string? Notes,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record GrassSpeciesChemicalSetActiveDto(
        [property: JsonPropertyName("active")] bool Active
    );
}