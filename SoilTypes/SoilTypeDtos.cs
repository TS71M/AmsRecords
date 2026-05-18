namespace AmsRecords.SoilTypes;

public static class SoilTypeDtos
{
    public record SoilTypeDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("soilTypeName")][MaxLength(100)] string Name,
        [property: JsonPropertyName("soilTypeActive")] bool IsActive
    );

    public record SoilTypeCreateDto(
        [property: JsonPropertyName("soilTypeName")][MaxLength(100)] string Name
    );

    public record SoilTypeUpdateDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("soilTypeName")][MaxLength(100)] string Name,
        [property: JsonPropertyName("soilTypeActive")] bool IsActive
    );
}