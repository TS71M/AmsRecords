using Lib.Enums;

namespace AmsRecords.Procural;

public static class ProcurementEventExtensions
{
    public static PurchaseRequisitionEvent AddProcurementEvent(
        this PurchaseRequisition requisition,
        User? actor,
        ProcurementEventType eventType,
        PurchaseRequisitionStatus? from,
        PurchaseRequisitionStatus? to,
        bool isAutomatic,
        string? note,
        DateTime? createdUtc = null)
    {
        var procurementEvent = new PurchaseRequisitionEvent
        {
            PurchaseRequisition = requisition,
            ActorUserId = actor?.Id,
            ActorUser = actor,
            EventType = eventType,
            FromStatus = from,
            ToStatus = to,
            IsAutomatic = isAutomatic,
            CreatedDt = createdUtc ?? DateTime.UtcNow,
            Note = (note ?? string.Empty).Trim()
        };
        requisition.Events.Add(procurementEvent);
        return procurementEvent;
    }
}
