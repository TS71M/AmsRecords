using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;

namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationActivityProductOption(Guid PubId, string ProductName, string RateUnitLabel);
public sealed record ApplicationPlannerActivityDto(Guid PlanPubId, Guid RevisionPubId,
    ApplicationPlannerStatsDto Stats, IReadOnlyList<ApplicationPlanItemSummaryDto> UpcomingApplications,
    IReadOnlyList<ApplicationActivityProductOption> ProductSnapshots, int Page, int PageSize, int TotalCount);
