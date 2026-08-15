using Lib.Enums;

namespace AmsRecords.Procural;

public sealed record ProcurementTransitionRule(
    PurchaseRequisitionStatus From,
    PurchaseRequisitionStatus To,
    string Capability,
    bool ReasonRequired = false);

public static class PurchaseRequisitionWorkflowPolicy
{
    public static IReadOnlyList<ProcurementTransitionRule> Rules { get; } =
    [
        new(PurchaseRequisitionStatus.Draft, PurchaseRequisitionStatus.Submitted, ProcurementPermissionCatalog.CreateRequest),
        new(PurchaseRequisitionStatus.Draft, PurchaseRequisitionStatus.Cancelled, ProcurementPermissionCatalog.CreateRequest, true),
        new(PurchaseRequisitionStatus.Submitted, PurchaseRequisitionStatus.UnderReview, ProcurementPermissionCatalog.ReviewRequest),
        new(PurchaseRequisitionStatus.Submitted, PurchaseRequisitionStatus.NeedsInformation, ProcurementPermissionCatalog.ReviewRequest, true),
        new(PurchaseRequisitionStatus.Submitted, PurchaseRequisitionStatus.Approved, ProcurementPermissionCatalog.ApproveRequest),
        new(PurchaseRequisitionStatus.Submitted, PurchaseRequisitionStatus.Rejected, ProcurementPermissionCatalog.ApproveRequest, true),
        new(PurchaseRequisitionStatus.Submitted, PurchaseRequisitionStatus.Cancelled, ProcurementPermissionCatalog.CreateRequest, true),
        new(PurchaseRequisitionStatus.NeedsInformation, PurchaseRequisitionStatus.Draft, ProcurementPermissionCatalog.CreateRequest),
        new(PurchaseRequisitionStatus.NeedsInformation, PurchaseRequisitionStatus.Cancelled, ProcurementPermissionCatalog.CreateRequest, true),
        new(PurchaseRequisitionStatus.UnderReview, PurchaseRequisitionStatus.NeedsInformation, ProcurementPermissionCatalog.ReviewRequest, true),
        new(PurchaseRequisitionStatus.UnderReview, PurchaseRequisitionStatus.Approved, ProcurementPermissionCatalog.ApproveRequest),
        new(PurchaseRequisitionStatus.UnderReview, PurchaseRequisitionStatus.Rejected, ProcurementPermissionCatalog.ApproveRequest, true),
        new(PurchaseRequisitionStatus.UnderReview, PurchaseRequisitionStatus.Cancelled, ProcurementPermissionCatalog.ApproveRequest, true),
        new(PurchaseRequisitionStatus.Approved, PurchaseRequisitionStatus.PriceRequested, ProcurementPermissionCatalog.ManageRfq),
        new(PurchaseRequisitionStatus.Approved, PurchaseRequisitionStatus.ReadyToOrder, ProcurementPermissionCatalog.PlaceOrder),
        new(PurchaseRequisitionStatus.Approved, PurchaseRequisitionStatus.Cancelled, ProcurementPermissionCatalog.ApproveRequest, true),
        new(PurchaseRequisitionStatus.PriceRequested, PurchaseRequisitionStatus.ReadyToOrder, ProcurementPermissionCatalog.ManageRfq),
        new(PurchaseRequisitionStatus.PriceRequested, PurchaseRequisitionStatus.Cancelled, ProcurementPermissionCatalog.ManageRfq, true),
        new(PurchaseRequisitionStatus.ReadyToOrder, PurchaseRequisitionStatus.Cancelled, ProcurementPermissionCatalog.PlaceOrder, true),
        new(PurchaseRequisitionStatus.Fulfilled, PurchaseRequisitionStatus.Closed, ProcurementPermissionCatalog.ReceiveOrder),
        new(PurchaseRequisitionStatus.Rejected, PurchaseRequisitionStatus.Draft, ProcurementPermissionCatalog.CreateRequest)
    ];

    public static ProcurementTransitionRule? Find(PurchaseRequisitionStatus from, PurchaseRequisitionStatus to)
        => Rules.FirstOrDefault(x => x.From == from && x.To == to);

    public static IReadOnlyList<PurchaseRequisitionStatus> AllowedTargets(PurchaseRequisitionStatus status)
        => Rules.Where(x => x.From == status).Select(x => x.To).Distinct().ToList();

    public static bool IsEditable(PurchaseRequisitionStatus status)
        => status is PurchaseRequisitionStatus.Draft or PurchaseRequisitionStatus.NeedsInformation;
}
