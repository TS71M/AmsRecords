using Lib.Planning;

namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerAllocationScopeDto(Guid FieldPubId, int Year, Guid PlanPubId,
    Guid RevisionPubId, Guid ZonePubId, Guid ProductSnapshotPubId);
public sealed record SavePlannerAllocationDto(PlannerAllocationScopeDto Scope, string ExpectedToken,
    Guid OperationId, ApplicationPlanAllocationIntent Intent, bool PreviewOnly = false);
public sealed record PlannerAllocationDto(PlannerAllocationScopeDto Scope, ApplicationPlanAllocationIntent? Intent,
    FertilizerProductQuantityUnit Unit, string EditToken, PlannerAllocationGrowthGuidanceDto? GrowthGuidance = null,
    PlannerAllocationProductGuidanceDto? ProductGuidance = null);

public sealed record PlannerAllocationProductGuidanceDto(decimal AreaM2, decimal? DensityKgPerLitre,
    IReadOnlyList<ApplicationPlanProductGuidance.Analysis> Analysis, PlannerProgrammeBalanceDto? Programme = null);

public sealed record PlannerProgrammeBalanceDto(string SourceToken, IReadOnlyList<ApplicationPlanProgrammeBalance.Result> Nutrients);

public sealed record PlannerAllocationGrowthGuidanceDto(DateTime CapturedAtUtc, string Profile,
    string? AreaName, int Year, IReadOnlyList<PlannerAllocationGrowthMonthDto> Months);
public sealed record PlannerAllocationGrowthMonthDto(int Month, decimal TemperatureC, decimal GrowthPotentialPercent, decimal Weight);
