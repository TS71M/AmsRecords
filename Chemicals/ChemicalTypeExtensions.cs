using static AmsRecords.Chemicals.ChemicalTypeDtos;

namespace AmsRecords.Chemicals;

public static class ChemicalTypeExtensions
{
    public static ChemicalTypeDto ToDto(this ChemicalType x)
        => new(
            x.PubId,
            x.ChemTypeName,
            x.DefaultBlockInProtectedZone,
            x.Active);

    public static ChemicalType ToEntity(this ChemicalTypeCreateDto dto)
        => new()
        {
            ChemTypeName = dto.ChemTypeName?.Trim() ?? "",
            DefaultBlockInProtectedZone = dto.DefaultBlockInProtectedZone,
            Active = true
        };

    public static void UpdateEntity(this ChemicalType x, ChemicalTypeUpdateDto dto)
    {
        x.ChemTypeName = dto.ChemTypeName?.Trim() ?? x.ChemTypeName;
        x.DefaultBlockInProtectedZone = dto.DefaultBlockInProtectedZone;
        x.Active = dto.Active;
    }
}
