using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;

namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationPlannerNutrientsDto(Guid RevisionPubId,
    ApplicationPlanZoneSummaryDto Zone, ApplicationZonePerformanceDto? Performance);
