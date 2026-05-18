namespace AmsRecords.Fields;

public record FieldMapDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("FieldTypeName")] string FieldTypeName,
        [property: JsonPropertyName("markerColor")] string? MarkerColor,
        [property: JsonPropertyName("markerIconUrl")] string? MarkerIconUrl,
        [property: JsonPropertyName("lat")] decimal? Lat,
        [property: JsonPropertyName("lng")] decimal? Lng,
        [property: JsonPropertyName("active")] bool Active
        );