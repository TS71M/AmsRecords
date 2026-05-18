namespace AmsRecords.Units;

public class UnitTypeDtos
{
    public record UnitTypeDto(
      [property: JsonPropertyName("pubId")] Guid PubId,
      [property: JsonPropertyName("unitTypeName")] string UnitTypeName
      );

    public record UnitTypeCreateDto(
        [property: JsonPropertyName("unitTypeName")] string UnitTypeName
       );

    public record UnitTypeUpdateDto(
     [property: JsonPropertyName("pubId")] Guid PubId,
     [property: JsonPropertyName("unitTypeName")] string UnitTypeName
     );
}
