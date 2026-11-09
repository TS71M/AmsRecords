using static AmsRecords.GrassTypes.GrassTypeDtos;

namespace AmsRecords.GrassTypes;

public static class GrassTypeExtensions
{

    public static GrassTypeUpdateDto ToUpdateDto(this GrassTypeDto dto)
        => new(
            dto.PubId,
            dto.GrassTypeName,
            dto.Pathway,
            dto.Description,
            dto.OptimalTemperatur,
            dto.Variation
            );

    public static GrassTypeCreateDto ToCreateDto(this GrassTypeDto dto)
       => new(
           dto.GrassTypeName,
           dto.Pathway,
           dto.Description,
           dto.OptimalTemperatur,
           dto.Variation
           );
}