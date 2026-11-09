using static AmsRecords.GrassSpecies.GrassSpeciesDtos;

namespace AmsRecords.GrassSpecies;

public static class GrassSpeciesExtensions
{
    public static GrassSpeciesCreateDto ToCreateDto(this GrassSpeciesUpdateDto dto)
        => new(
            dto.Name,
            dto.LatinName,
            dto.GrassTypePubId,
            dto.ShowInGtsInterpretation,
            dto.NMaxYear,
            dto.NMaxMonth,
            dto.KNRatioMin,
            dto.KNRatioMax,
            dto.PHMin,
            dto.PHMax,
            dto.SaltToleranceMin,
            dto.SaltToleranceMax,
            dto.GrowthHabit,
            dto.LiguleLeafMargin,
            dto.AuriclesLeafHairs,
            dto.VernationLeafStructure,
            dto.CollarLeafArrangement,
            dto.SeedheadColorFlowerColor,
            dto.RootType
            );

    public static GrassSpeciesUpdateDto ToUpdateDto(this GrassSpeciesDto dto)
        => new(
            dto.PubId,
            dto.Active,
            dto.Name,
            dto.LatinName,
            dto.GrassTypePubId,
            dto.ShowInGtsInterpretation,
            dto.NMaxYear,
            dto.NMaxMonth,
            dto.KNRatioMin,
            dto.KNRatioMax,
            dto.PHMin,
            dto.PHMax,
            dto.SaltToleranceMin,
            dto.SaltToleranceMax,
            dto.GrowthHabit,
            dto.LiguleLeafMargin,
            dto.AuriclesLeafHairs,
            dto.VernationLeafStructure,
            dto.CollarLeafArrangement,
            dto.SeedheadColorFlowerColor,
            dto.RootType
        );

    public static string? Validate(GrassSpeciesCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            return "Name is required.";

        if (dto.Name.Length > 250)
            return "Name is too long (max 250).";

        if (dto.NMaxYear < 0 || dto.NMaxMonth < 0)
            return "N values must be >= 0.";

        // Range checks (should also be enforced via DB check constraints)
        if (dto.KNRatioMin < 0 || dto.KNRatioMax < dto.KNRatioMin)
            return "Invalid K:N ratio range.";

        if (dto.PHMin < 0 || dto.PHMax < dto.PHMin)
            return "Invalid pH range.";

        if (dto.SaltToleranceMin < 0 || dto.SaltToleranceMax < dto.SaltToleranceMin)
            return "Invalid salt tolerance range.";

        return null;
    }

    public static string? Validate(GrassSpeciesUpdateDto dto)
        => Validate(new GrassSpeciesCreateDto(
            dto.Name,
            dto.LatinName,
            dto.GrassTypePubId,
            dto.ShowInGtsInterpretation,
            dto.NMaxYear,
            dto.NMaxMonth,
            dto.KNRatioMin,
            dto.KNRatioMax,
            dto.PHMin,
            dto.PHMax,
            dto.SaltToleranceMin,
            dto.SaltToleranceMax,
            dto.GrowthHabit,
            dto.LiguleLeafMargin,
            dto.AuriclesLeafHairs,
            dto.VernationLeafStructure,
            dto.CollarLeafArrangement,
            dto.SeedheadColorFlowerColor,
            dto.RootType
        ));
}
