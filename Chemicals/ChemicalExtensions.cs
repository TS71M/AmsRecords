using static AmsRecords.Chemicals.ChemicalDtos;

namespace AmsRecords.Chemicals;

public static class ChemicalExtensions
{
    public static ChemicalDto ToDto(this Chemical x)
        => new(
            PubId: x.PubId,
            ChemName: x.ChemName,
            ChemForm: x.ChemicalFormula ?? "",
            IsElemental: x.IsElemental,
            ChemTypePubId: x.ChemType?.PubId ?? Guid.Empty,
            ChemTypeName: x.ChemType?.ChemTypeName ?? "",
            UnitTypePubId: x.UnitType?.PubId,
            UnitTypeName: x.UnitType?.UnitTypeName ?? "",
            x.Active);

    public static ChemicalMiniDto ToMiniDto(this Chemical x)
        => new(x.PubId, x.ChemName, x.UnitType?.PubId);

    public static void UpdateEntity(this Chemical entity, ChemicalUpdateDto dto, int chemTypeId)
    {
        entity.ChemName = dto.ChemName.Trim();
        entity.ChemicalFormula = dto.ChemForm?.Trim();
        entity.IsElemental = dto.IsElemental;
        entity.ChemTypeId = chemTypeId;
        entity.Active = dto.Active;
    }

    public static Chemical ToEntity(this ChemicalCreateDto dto, int chemTypeId)
        => new()
        {
            ChemName = dto.ChemName.Trim(),
            ChemicalFormula = dto.ChemForm?.Trim(),
            IsElemental = dto.IsElemental,
            ChemTypeId = chemTypeId,
        };
}
