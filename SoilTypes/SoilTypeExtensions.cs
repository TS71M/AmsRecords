using static AmsRecords.SoilTypes.SoilTypeDtos;

namespace AmsRecords.SoilTypes;

public static class SoilTypeExtensions
{
    public static SoilTypeDto ToDto(this SoilType soilType)
        => new(
            PubId: soilType.PubId,
            IsActive: soilType.Active,
            Name: soilType.SoiTypName
        );
    public static SoilType ToEntity(this SoilTypeCreateDto createDto)
        => new()
        {
            PubId = Guid.NewGuid(),
            SoiTypName = createDto.Name
        };
    public static void UpdateEntity(this SoilType entity, SoilTypeUpdateDto updateDto)
    {
        entity.PubId = updateDto.PubId;
        entity.Active = updateDto.IsActive;
        entity.SoiTypName = updateDto.Name;
    }
}