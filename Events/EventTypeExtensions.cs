using static AmsRecords.Events.EventTypeDtos;

namespace AmsRecords.Events;

public static class EventTypeExtensions
{
    public static EventTypeDto ToDto(this EventType eventType)
            => new(
                eventType.PubId,
                eventType.EventTypeName,
                eventType.EventTypeDes,
                eventType.IsActive,
                eventType.User?.Name?.FullName ?? "-"
                );
}