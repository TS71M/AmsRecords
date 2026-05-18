using static AmsRecords.Events.EventDtos;

namespace AmsRecords.Events;

public static class EventExtensions
{
    public static EventDto ToDto(this Event eventX)
           => new(
               eventX.PubId,
               eventX.EventDate,
               eventX.EventName,
               eventX.EventDescription,
               eventX.EventType.PubId,
               eventX.EventType.EventTypeName,
               eventX.User.Name?.FullName ?? "-"
               );
}