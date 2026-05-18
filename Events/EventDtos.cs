namespace AmsRecords.Events;

public static class EventDtos
{
    public record EventDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("eventDate")] DateTime EventDate,
        [property: JsonPropertyName("eventName")] string EventName,
        [property: JsonPropertyName("eventDescription")] string EventDescription,
        [property: JsonPropertyName("pubEventTypeId")] Guid PubEventTypeId,
        [property: JsonPropertyName("eventTypeName")] string EventTypeName,
        [property: JsonPropertyName("createdBy")] string CreatedBy
        );

    public record EventUpdateDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("eventDate")] DateTime EventDate,
        [property: JsonPropertyName("eventName")] string EventName,
        [property: JsonPropertyName("eventDescription")] string EventDescription,
        [property: JsonPropertyName("pubEventTypeId")] Guid PubEventTypeId,
        [property: JsonPropertyName("updatedAt")] DateTime UpdatedAt
        );

    public record EventCreateDto(
        [property: JsonPropertyName("eventDate")] DateTime EventDate,
        [property: JsonPropertyName("eventName")] string EventName,
        [property: JsonPropertyName("pubEventTypeId")] Guid PubEventTypeId,
        [property: JsonPropertyName("eventDescription")] string EventDescription,
        [property: JsonPropertyName("createdById")] string CreatedById
        );
}