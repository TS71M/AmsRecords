namespace AmsRecords.Coordinates;

public record CoordinateDto(
    [property: JsonPropertyName("pubId")] Guid PubId,
    [property: JsonPropertyName("latitude")] decimal Latitude,
    [property: JsonPropertyName("longitude")] decimal Longitude
    );

public record CoordinateCreateDto(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("addressOwnerType")][Required] ParentEntityType OwnerType,
    [property: JsonPropertyName("ownerPubId")][Required] Guid OwnerPubId
    );

public record CoordinateUpdateDto(
    [property: JsonPropertyName("pubId")] Guid PubId,
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("ownerType")] ParentEntityType? OwnerType = null,
    [property: JsonPropertyName("ownerPubId")] Guid? OwnerPubId = null
    );
