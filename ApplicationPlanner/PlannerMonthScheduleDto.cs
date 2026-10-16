using Lib.Planning;

namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerMonthScheduleRequestDto(PlannerAllocationScopeDto Scope, string ExpectedAllocationToken,
    int Month, int FirstDay, int IntervalDays, decimal TreatedAreaM2);
public sealed record PlannerMonthScheduleEntryDto(DateOnly Date, int IsoWeek, decimal RequestedQuantity,
    decimal Rate, decimal ScheduledQuantity, decimal QuantityDifference);
public sealed record PlannerMonthSchedulePreviewDto(PlannerMonthScheduleRequestDto Request, decimal MonthlyTarget,
    decimal ExistingIntendedQuantity, decimal ProposedQuantity, decimal RemainingQuantity,
    FertilizerProductQuantityUnit Unit, FertilizerApplicationRateUnit RateUnit,
    IReadOnlyList<PlannerMonthScheduleEntryDto> Entries, string PreviewToken = "");
public sealed record CommitPlannerMonthScheduleDto(PlannerMonthScheduleRequestDto Request, string ExpectedPreviewToken, Guid OperationId);
