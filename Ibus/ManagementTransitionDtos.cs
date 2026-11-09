using Lib.Enums;

namespace AmsRecords.Ibus;

public static class ManagementTransitionDtos
{
    public sealed record ReviewPage<T>(IReadOnlyList<T> Items, int TotalCount, int Page);
    public sealed record Routing(Guid FieldPubId, ProcurementMode Mode, Guid? HubPubId,
        bool AllowDirectOrdering, bool Consolidate);
    public sealed record CopyRequest(Guid ProductPubId, Guid DestinationIbuPubId, string TargetName, bool IncludePrice);
    public sealed record Request(Guid OperationId, Guid? LeavingManagerPubId, Guid? JoiningManagerPubId,
        DateTime EffectiveAtUtc, IReadOnlyList<Routing> Fields, IReadOnlyList<CopyRequest> Products,
        bool RetainPlansAndRecords, bool NoStockTransfer, int SchemaVersion = 1);
    public sealed record Option(Guid PubId, string Name);
    public sealed record FieldOption(Guid PubId, string Name, ProcurementMode Mode, Guid? HubPubId,
        bool AllowDirectOrdering, bool Consolidate);
    public sealed record ProductOption(Guid PubId, Guid OwnerPubId, string OwnerName, string Name);
    public sealed record Setup(Guid IbuPubId, string IbuName, IReadOnlyList<FieldOption> Fields,
        IReadOnlyList<Option> Managers, IReadOnlyList<Option> CurrentManagers, IReadOnlyList<Option> Hubs,
        IReadOnlyList<ProductOption> Products);
    public sealed record Preview(string Token, IReadOnlyList<string> Warnings, bool BlockingWork,
        int LocalProductCount, IReadOnlyList<HandoverSummary> Products, string ChangeSummary = "");
    public sealed record Submit(Request Request, string PreviewToken);
    public sealed record HandoverSummary(Guid PubId, Guid SourceIbuPubId, Guid DestinationIbuPubId,
        string ProductName, string TargetName, string SnapshotJson, string SnapshotHash, bool IncludePrice,
        bool SourceApproved, bool DestinationApproved, bool Rejected, Guid? CreatedProductPubId,
        Guid Version, string? Reason, DateTime CapturedAtUtc = default, string SourceName = "", string DestinationName = "");
    public sealed record Receipt(Guid PubId, Guid Version, string State, DateTime EffectiveAtUtc,
        DateTime? AppliedAtUtc, string? Reason, IReadOnlyList<HandoverSummary> Products, string ChangeSummary = "");
    public sealed record Decision(Guid Version, bool Approve, string? Reason, string? TargetName = null);
}
