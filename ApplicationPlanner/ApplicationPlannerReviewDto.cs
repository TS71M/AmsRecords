using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;

namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerReviewRevisionDto(Guid PubId, int RevisionNumber, string Status, DateTime? ApprovedAtUtc);
public sealed record ApplicationPlannerReviewDto(Guid PlanPubId, Guid RevisionPubId, string PlanName,
    IReadOnlyList<PlannerReviewRevisionDto> Revisions, ApplicationPlanPerformanceDto Performance);
