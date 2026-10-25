using Lib.Planning;

namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerSeasonScopeDto(Guid FieldPubId, int Year, Guid PlanPubId, Guid RevisionPubId, Guid ZonePubId);
public sealed record PlannerSeasonOverviewDto(PlannerSeasonScopeDto Scope, bool IsEditable,
    IReadOnlyList<PlannerSeasonProductDto> Products, int UnassignedApplicationCount,
    ApplicationPlanSeasonSummary.Result? Summary = null,
    IReadOnlyList<PlannerSeasonNutrientBudgetDto>? NutrientBudgets = null,
    IReadOnlyList<Guid>? ReferenceSurfacePubIds = null);
public sealed record PlannerSeasonNutrientBudgetDto(ApplicationPlanProgrammeBalance.Result Balance, string Source);
public sealed record PlannerSeasonProductDto(Guid ProductSnapshotPubId, string ProductName,
    FertilizerProductQuantityUnit? Unit, string Status, decimal? AnnualQuantity, decimal? Allocated,
    decimal? Remaining, IReadOnlyList<ApplicationPlanMonthIntent> Months,
    ApplicationPlanAllocationIntentPolicy.Review? OriginalSchedule);
