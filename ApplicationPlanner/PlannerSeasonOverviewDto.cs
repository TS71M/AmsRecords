using Lib.Planning;

namespace AmsRecords.ApplicationPlanner;

// Omit ZonePubId for a revision-wide product overview. Existing zone reads stay compatible.
public sealed record PlannerSeasonScopeDto(Guid FieldPubId, int Year, Guid PlanPubId, Guid RevisionPubId,
    Guid? ZonePubId = null, Guid? ProductSnapshotPubId = null, bool IncludeAreaDetails = true);
public sealed record PlannerSeasonOverviewDto(PlannerSeasonScopeDto Scope, bool IsEditable,
    IReadOnlyList<PlannerSeasonProductDto> Products, int UnassignedApplicationCount,
    ApplicationPlanSeasonSummary.Result? Summary = null,
    IReadOnlyList<PlannerSeasonNutrientBudgetDto>? NutrientBudgets = null,
    IReadOnlyList<Guid>? ReferenceSurfacePubIds = null,
    IReadOnlyList<PlannerSeasonProductOptionDto>? AvailableProducts = null,
    IReadOnlyList<PlannerSeasonAreaOptionDto>? Areas = null);
public sealed record PlannerSeasonProductOptionDto(Guid ProductSnapshotPubId, string ProductName);
public sealed record PlannerSeasonAreaOptionDto(Guid ZonePubId, string Name, decimal AreaM2);
public sealed record PlannerSeasonProductAreaDto(Guid ZonePubId, string Name, decimal AreaM2,
    PlannerSeasonProductDto Allocation, IReadOnlyList<Guid>? SurfacePubIds = null);
public sealed record PlannerSeasonNutrientBudgetDto(ApplicationPlanProgrammeBalance.Result Balance, string Source);
public sealed record PlannerSeasonProductDto(Guid ProductSnapshotPubId, string ProductName,
    FertilizerProductQuantityUnit? Unit, string Status, decimal? AnnualQuantity, decimal? Allocated,
    decimal? Remaining, IReadOnlyList<ApplicationPlanMonthIntent> Months,
    ApplicationPlanAllocationIntentPolicy.Review? OriginalSchedule,
    IReadOnlyList<PlannerSeasonProductAreaDto>? Areas = null);
