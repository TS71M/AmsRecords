namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationPlanSourceSurfaceDto(Guid PubId, string Name, decimal AreaM2);
public sealed record ApplicationPlanAreaSourceDto(Guid AreaSnapshotPubId, DateTime CheckedAtUtc,
    bool Found, string? AreaName, bool Active, bool Planning,
    IReadOnlyList<ApplicationPlanSourceSurfaceDto> Surfaces,
    ApplicationPlannerDtos.UpdateApplicationPlanAreaSnapshotDto? ProposedArea = null);
