using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;

namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationPlannerMaterialsDto(Guid PlanPubId, Guid RevisionPubId,
    string MaterialEditToken, IReadOnlyList<AnnualProductPlanLineDto> MaterialPlanLines);
