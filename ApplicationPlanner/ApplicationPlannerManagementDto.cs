using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;

namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerManagementPlanDto(Guid PubId, string PlanName, string PlanPurpose, int Year,
    int LatestRevisionNumber, string LatestRevisionStatus);
public sealed record ApplicationPlannerManagementDto(Guid FieldPubId, int Year,
    IReadOnlyList<PlannerManagementPlanDto> AvailablePlans, PlannerManagementPlanDto? AnnualPlan,
    IReadOnlyList<ApplicationPlanRevisionSummaryDto> SelectedPlanRevisions,
    IReadOnlyList<ApplicationPlanZoneSummaryDto> Zones, IReadOnlyList<ApplicationPlanAreaSnapshotDto> AreaSnapshots,
    IReadOnlyList<ApplicationPlanProductSnapshotDto> ProductSnapshots, string? ZoneInputsToken = null)
{
    public bool HasAnnualPlan => AnnualPlan is not null;
    public Guid? SelectedPlanPubId => AnnualPlan?.PubId;
}
