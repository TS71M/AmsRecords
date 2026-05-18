namespace AmsRecords.Units;

public static class UnitExtensions
{
    public static UnitDto ToDto(this Unit unit)
            => new(
                unit.PubId,
                unit.UnitShort,
                unit.UnitName,
                unit.IsBase,
                unit.ConversionFactor,
                unit.Offset,
                unit.Precision,
                unit.UnitType.PubId,
                unit.UnitType.UnitTypeName
                );
}
