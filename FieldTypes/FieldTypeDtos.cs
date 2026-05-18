namespace AmsRecords.FieldTypes;

public class FieldTypeDtos
{
    public record FieldTypeDto(
        [property: JsonPropertyName("fieldTypeId")] Guid PubId,
        [property: JsonPropertyName("fieldTypeName")] string FieldTypeName,
        [property: JsonPropertyName("markerColor")] string? MarkerColor,
        [property: JsonPropertyName("markerIconUrl")] string? MarkerIconUrl
        );

    public record FieldTypeMiniDto(
        [property: JsonPropertyName("fieldTypeId")] Guid PubId,
        [property: JsonPropertyName("fieldTypeName")] string FieldTypeName
        );

    public record FieldTypeCreateDto(
        [property: JsonPropertyName("fieldTypeName")] string FieldTypeName,
        [property: JsonPropertyName("markerColor")] string? MarkerColor,
        [property: JsonPropertyName("markerIconUrl")] string? MarkerIconUrl
        );

    public record FieldTypeUpdateDto(
        [property: JsonPropertyName("fieldTypeId")] Guid PubId,
        [property: JsonPropertyName("fieldTypeName")] string FieldTypeName,
        [property: JsonPropertyName("markerColor")] string? MarkerColor,
        [property: JsonPropertyName("markerIconUrl")] string? MarkerIconUrl
        );
}