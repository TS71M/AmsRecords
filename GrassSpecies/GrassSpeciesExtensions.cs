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

    public static GrassSpeciesDto ToDto(AmsModels.GrassSpecies dto) =>
       new(
           dto.PubId,
           dto.Active,
           dto.Name,
           dto.LatinName,
           dto.GrassType.PubId,
           dto.GrassType.GrassTypeName,
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
           dto.RootType,
           dto.CreatedAtUtc,
           dto.UpdatedAtUtc,
           dto.MainAppImage?.PubId
       );

    public static AmsModels.GrassSpecies ToEntity(this GrassSpeciesCreateDto dto, int grassTypeId)
        => new()
        {
            Name = dto.Name,
            LatinName = dto.LatinName,
            GrassTypeId = grassTypeId,
            NMaxYear = dto.NMaxYear,
            NMaxMonth = dto.NMaxMonth,
            KNRatioMin = dto.KNRatioMin,
            KNRatioMax = dto.KNRatioMax,
            PHMin = dto.PHMin,
            PHMax = dto.PHMax,
            SaltToleranceMin = dto.SaltToleranceMin,
            SaltToleranceMax = dto.SaltToleranceMax,
            GrowthHabit = dto.GrowthHabit,
            RootType = dto.RootType,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            ShowInGtsInterpretation = dto.ShowInGtsInterpretation
        };

    public static void UpdateEntity(this AmsModels.GrassSpecies entity, GrassSpeciesUpdateDto dto, int grassTypeId)
    {
        entity.PubId = dto.PubId;
        entity.Active = dto.Active;
        entity.Name = dto.Name;
        entity.LatinName = dto.LatinName;
        entity.GrassTypeId = grassTypeId;
        entity.NMaxYear = dto.NMaxYear;
        entity.NMaxMonth = dto.NMaxMonth;
        entity.KNRatioMin = dto.KNRatioMin;
        entity.KNRatioMax = dto.KNRatioMax;
        entity.PHMin = dto.PHMin;
        entity.PHMax = dto.PHMax;
        entity.SaltToleranceMin = dto.SaltToleranceMin;
        entity.SaltToleranceMax = dto.SaltToleranceMax;
        entity.GrowthHabit = dto.GrowthHabit;
        entity.LiguleLeafMargin = dto.LiguleLeafMargin;
        entity.AuriclesLeafHairs = dto.AuriclesLeafHairs;
        entity.VernationLeafStructure = dto.VernationLeafStructure;
        entity.CollarLeafArrangement = dto.CollarLeafArrangement;
        entity.SeedheadColorFlowerColor = dto.SeedheadColorFlowerColor;
        entity.RootType = dto.RootType;
        entity.UpdatedAtUtc = DateTime.UtcNow;
        entity.CreatedAtUtc = entity.CreatedAtUtc; 
        entity.MainImageId = entity.MainImageId; 
        entity.ShowInGtsInterpretation = dto.ShowInGtsInterpretation;
    }

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