namespace AmsRecords.Units;

public class UnitDtos
{
    public record UnitDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("unitShort")][MaxLength(10)] string UnitShort,
        [property: JsonPropertyName("unitName")][MaxLength(50)] string UnitName,
        [property: JsonPropertyName("isBase")] bool IsBase,
        [property: JsonPropertyName("conversionFactor")] decimal ConversionFactor,
        [property: JsonPropertyName("offSet")] decimal OffSet,
        [property: JsonPropertyName("precision")] int Precision,
        [property: JsonPropertyName("unitTypePubId")] Guid UnitTypePubId,
        [property: JsonPropertyName("unitTypeName")] string? UnitTypeName
        );

    public record UnitMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("unitName")][MaxLength(50)] string UnitName
        );

    public record UnitCreateDto(
        [property: JsonPropertyName("unitShort")][MaxLength(10)] string UnitShort,
        [property: JsonPropertyName("unitName")][MaxLength(50)] string UnitName,
        [property: JsonPropertyName("isBase")] bool IsBase,
        [property: JsonPropertyName("conversionFactor")] decimal ConversionFactor,
        [property: JsonPropertyName("offSet")] decimal OffSet,
        [property: JsonPropertyName("precision")] int Precision,
        [property: JsonPropertyName("unitTypePubId")] Guid UnitTypePubId
       );

    public record UnitUpdateDto(
       [property: JsonPropertyName("pubId")] Guid PubId,
       [property: JsonPropertyName("unitShort")][MaxLength(10)] string UnitShort,
       [property: JsonPropertyName("unitName")][MaxLength(50)] string UnitName,
       [property: JsonPropertyName("isBase")] bool IsBase,
       [property: JsonPropertyName("conversionFactor")] decimal ConversionFactor,
       [property: JsonPropertyName("offSet")] decimal OffSet,
       [property: JsonPropertyName("precision")] int Precision,
       [property: JsonPropertyName("unitTypePubId")] Guid UnitTypePubId,
       [property: JsonPropertyName("unitTypeName")] string? UnitTypeName
       );
}
