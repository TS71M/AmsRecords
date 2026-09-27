namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationPlanProductCandidateDto(Guid PubId, string Name, string Category, string InventoryUnit);
public sealed record ApplicationPlanProductCandidatesDto(int Page, bool HasMore, IReadOnlyList<ApplicationPlanProductCandidateDto> Rows);
public sealed record AddApplicationPlanProductDto(Guid PlanPubId, Guid RevisionPubId, Guid SourceProductPubId,
    decimal? InventoryQuantity, bool PreviewOnly = true, string? ExpectedToken = null);
public sealed record ApplicationPlanProductAdditionDto(string ExpectedToken, bool PreviewOnly,
    ApplicationPlannerDtos.UpdateApplicationPlanProductSnapshotDto Product);
