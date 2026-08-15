namespace AmsRecords.Procural;

public sealed record ProcurementPermissionDefinition(string Key, string Title, string Description);

public static class ProcurementPermissionCatalog
{
    public const string ModuleKey = "procural";
    public const string CreateRequest = "create-request";
    public const string ReviewRequest = "review-request";
    public const string ApproveRequest = "approve-request";
    public const string ManageRfq = "manage-rfq";
    public const string PlaceOrder = "place-order";
    public const string ReceiveOrder = "receive-order";
    public const string ManageMasterData = "manage-master-data";
    public const string Configure = "configure";

    public static IReadOnlyList<ProcurementPermissionDefinition> All { get; } =
    [
        new(CreateRequest, "Create requests", "Create, edit, submit, and withdraw own purchase requests."),
        new(ReviewRequest, "Review requests", "Review requests, request more information, and assign procurement managers."),
        new(ApproveRequest, "Approve requests", "Approve or reject purchase requests."),
        new(ManageRfq, "Manage supplier enquiries", "Request, enter, compare, and select supplier quotations."),
        new(PlaceOrder, "Place orders", "Create, place, amend, or cancel supplier purchase orders."),
        new(ReceiveOrder, "Receive orders", "Record deliveries, discrepancies, and close completed purchase orders."),
        new(ManageMasterData, "Manage procurement master data", "Manage suppliers, products, catalogues, and supplier terms."),
        new(Configure, "Configure procurement", "Manage procurement hubs, routing modes, policies, and defaults.")
    ];

    public static bool IsCapability(string? key)
        => !string.IsNullOrWhiteSpace(key) &&
           All.Any(x => string.Equals(x.Key, key.Trim(), StringComparison.OrdinalIgnoreCase));
}
