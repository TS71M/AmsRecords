using Lib.Enums;

namespace AmsRecords.Procural;

public static class ProcurementOrderCompletionPolicy
{
    public static bool CanReconcileInventory(ProcurementPurchaseOrderStatus status, ProcurementInventoryHandoffStatus handoff, bool hasReceipts)
        => hasReceipts
            && status is ProcurementPurchaseOrderStatus.PartiallyReceived or ProcurementPurchaseOrderStatus.FullyReceived or ProcurementPurchaseOrderStatus.Closed
            && handoff is ProcurementInventoryHandoffStatus.Pending or ProcurementInventoryHandoffStatus.Failed;

    public static bool CanClose(ProcurementPurchaseOrderStatus status, ProcurementInventoryHandoffStatus handoff)
        => status == ProcurementPurchaseOrderStatus.FullyReceived
            && handoff is ProcurementInventoryHandoffStatus.NotRequired or ProcurementInventoryHandoffStatus.Completed;
}
