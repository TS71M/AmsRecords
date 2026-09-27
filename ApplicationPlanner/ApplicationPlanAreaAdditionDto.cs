namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationPlanAreaCandidateDto(Guid PubId, string Name);
public sealed record ApplicationPlanAreaCandidatesDto(int Page, bool HasMore, IReadOnlyList<ApplicationPlanAreaCandidateDto> Rows);
public sealed record AddApplicationPlanAreaDto(Guid PlanPubId, Guid RevisionPubId, Guid SourceAreaPubId,
    bool PreviewOnly = true, string? ExpectedToken = null);
public sealed record ApplicationPlanAreaAdditionDto(string ExpectedToken, bool PreviewOnly, string Name,
    decimal AreaM2, IReadOnlyList<ApplicationPlanSourceSurfaceDto> Surfaces);
