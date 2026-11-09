using AmsRecords.Units;
using static AmsRecords.ClimateNormals.ClimateNormalDtos;

namespace AmsRecords.ClimateNormals;

public static class ClimateNormalExtensions
{

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
}
