namespace AmsRecords.Events;

public class EventTypeDtos
{
    public record EventTypeDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("eventTypeName")] string EventTypeName,
        [property: JsonPropertyName("eventTypeDes")][MaxLength(250)] string EventTypeDes,
        [property: JsonPropertyName("isActive")] bool IsActive,
        [property: JsonPropertyName("createdBy")] string CreatedBy
        );

    public record EventTypeCreateDto(
        [property: JsonPropertyName("eventTypeName")] string EventTypeName,
        [property: JsonPropertyName("eventTypeDes")][MaxLength(250)] string EventTypeDes
        );

    public record EventTypeUpdateDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("eventTypeName")] string EventTypeName,
        [property: JsonPropertyName("eventTypeDes")][MaxLength(250)] string EventTypeDes,
        [property: JsonPropertyName("isActive")] bool IsActive
        );
}