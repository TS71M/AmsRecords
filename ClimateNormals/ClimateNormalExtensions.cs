using AmsRecords.Units;
using static AmsRecords.ClimateNormals.ClimateNormalDtos;

namespace AmsRecords.ClimateNormals;

public static class ClimateNormalExtensions
{
    public static FieldClimateNormalPivotDto ToPivotDto(this FieldClimateNormal n, Unit? displayUnit = null)
    {
        var fieldPubId = n.Field?.PubId ?? Guid.Empty;

        decimal Get(byte month)
            => ConvertForDisplay(n.Months.FirstOrDefault(x => x.Active && x.Month == month)?.Value ?? 0m, displayUnit);

        return new FieldClimateNormalPivotDto(
            PubId: n.PubId,
            FieldPubId: fieldPubId,
            SourceType: (short)n.SourceType,
            Active: n.Active,
            FromYear: n.FromYear,
            ToYear: n.ToYear,
            UpdatedUtc: n.UpdatedUtc,
            Jan: Get(1),
            Feb: Get(2),
            Mar: Get(3),
            Apr: Get(4),
            May: Get(5),
            Jun: Get(6),
            Jul: Get(7),
            Aug: Get(8),
            Sep: Get(9),
            Oct: Get(10),
            Nov: Get(11),
            Dec: Get(12)
        );
    }

    public static IReadOnlyDictionary<byte, decimal> ToMonthMap(this FieldClimateNormalPivotUpsertDto dto)
        => new Dictionary<byte, decimal>
        {
            [1] = dto.Jan,
            [2] = dto.Feb,
            [3] = dto.Mar,
            [4] = dto.Apr,
            [5] = dto.May,
            [6] = dto.Jun,
            [7] = dto.Jul,
            [8] = dto.Aug,
            [9] = dto.Sep,
            [10] = dto.Oct,
            [11] = dto.Nov,
            [12] = dto.Dec
        };

    static decimal ConvertForDisplay(decimal value, Unit? displayUnit)
    {
        if (displayUnit is null || displayUnit.ConversionFactor <= 0)
            return value;

        var converted = (value / displayUnit.ConversionFactor) - displayUnit.Offset;
        return displayUnit.Precision >= 0
            ? Math.Round(converted, displayUnit.Precision, MidpointRounding.AwayFromZero)
            : converted;
    }
}
