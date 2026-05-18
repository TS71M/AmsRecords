using static AmsRecords.GrassSpecies.GrassSpeciesChemicalDtos;

namespace AmsRecords.GrassSpecies;

public static class GrassSpeciesChemicalExtensions
{
    public static GrassSpeciesChemicalDto ToDto(this GrassSpeciesChemical x)
    => new(
        x.PubId,
        x.Active,
        x.GrassSpecieses?.PubId ?? Guid.Empty,
        x.GrassSpecieses?.Name ?? "",
        x.Chem?.PubId ?? Guid.Empty,
        x.Chem?.ChemName ?? "",
        x.UseMode,
        x.Notes
    );

    public static void UpdateEntity(this GrassSpeciesChemical x, GrassSpeciesChemicalUpdateDto dto)
    {
        x.Active = dto.Active;
        x.UseMode = dto.UseMode;
        x.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
    }
}