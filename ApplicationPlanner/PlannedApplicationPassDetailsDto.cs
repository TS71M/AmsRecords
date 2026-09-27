namespace AmsRecords.ApplicationPlanner;

public sealed record PlannedApplicationPassDetailsDto(Guid PubId, Guid AnnualPlanPubId, Guid RevisionPubId,
    string Name, Guid ZonePubId, string ZoneName, decimal TreatedAreaM2, DateOnly PlannedLocalDate,
    decimal CarrierVolumeLitresPerHa, Guid? MachineryPubId, IReadOnlyList<PlannedApplicationPassProductDto> Products);

public sealed record PlannedApplicationPassProductDto(Guid PlannedApplicationPubId, Guid? ProductSnapshotPubId,
    string ProductName, decimal Rate, string? RateUnit, decimal TreatedAreaM2,
    DateOnly ScheduledLocalDate, string ScheduleStatus, bool IsComplete);
