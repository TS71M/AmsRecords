using static AmsRecords.Units.UnitTypeDtos;

namespace AmsRecords.Units;

public static class UnitTypeExtensions
{
    public static UnitTypeDto ToDto(this UnitType unitType)
                => new(unitType.PubId, unitType.UnitTypeName);

    public static UnitType FromDto(this UnitTypeDto unitTypeDto)
        => new()
        {
            PubId = unitTypeDto.PubId,
            UnitTypeName = unitTypeDto.UnitTypeName
        };
}
