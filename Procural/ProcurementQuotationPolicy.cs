using Lib.Enums;

namespace AmsRecords.Procural;

public static class ProcurementQuotationPolicy
{
    public static bool AllowsEnquiries(PurchaseRequisitionStatus status)
        => status is PurchaseRequisitionStatus.Approved or PurchaseRequisitionStatus.PriceRequested or PurchaseRequisitionStatus.ReadyToOrder;

    public static bool CanSelectResponse(ProcurementQuoteStatus status, IEnumerable<decimal?> unitPrices)
        => status == ProcurementQuoteStatus.Received && unitPrices.All(price => price.HasValue);
}
