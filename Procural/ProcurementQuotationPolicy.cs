using Lib.Enums;

namespace AmsRecords.Procural;

public static class ProcurementQuotationPolicy
{
    public static bool AllowsEnquiries(PurchaseRequisitionStatus status)
        => status is PurchaseRequisitionStatus.Approved or PurchaseRequisitionStatus.PriceRequested or PurchaseRequisitionStatus.ReadyToOrder;
}
