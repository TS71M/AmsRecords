namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerPlanOption(Guid PubId, string PlanName);
public sealed record PlannerRevisionOption(Guid PubId, int RevisionNumber, string Status);
public sealed record PlannerZoneOption(Guid PubId, string ZoneName, decimal TreatedAreaM2);
public sealed record ApplicationPlannerSeasonContextDto(Guid FieldPubId, string FieldName, int Year,
    Guid? PlanPubId, Guid? RevisionPubId, IReadOnlyList<PlannerPlanOption> Plans,
    IReadOnlyList<PlannerRevisionOption> Revisions, IReadOnlyList<PlannerZoneOption> Zones,
    int CapturedAreaCount, int UsableZoneCount, int UsableProductCount, int TargetCount);
