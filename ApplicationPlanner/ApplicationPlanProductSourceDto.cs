namespace AmsRecords.ApplicationPlanner;

// Product contains current catalogue values but historical inventory. When the
// unit changed, that inventory still belongs to the old frozen unit: consumers
// must request a reviewed replacement rather than apply the number unchanged.
public sealed record ApplicationPlanProductSourceDto(Guid ProductSnapshotPubId, DateTime CheckedAtUtc,
    bool Found, bool InventoryUnitChanged, ApplicationPlannerDtos.UpdateApplicationPlanProductSnapshotDto? Product);
