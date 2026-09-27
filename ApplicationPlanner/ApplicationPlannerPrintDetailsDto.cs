using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;

namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationPlannerPrintProductDto(Guid PubId, IReadOnlyList<ApplicationPlanProductNutrientSnapshotDto> Nutrients);
public sealed record ApplicationPlannerPrintDetailsDto(Guid RevisionPubId, Guid ZonePubId,
    IReadOnlyList<ApplicationPlannerPrintProductDto> Products, ApplicationZonePerformanceDto? Performance);
