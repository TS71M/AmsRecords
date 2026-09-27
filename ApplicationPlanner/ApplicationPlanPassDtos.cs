namespace AmsRecords.ApplicationPlanner;

public sealed record SaveApplicationPlanPassDto(Guid AnnualPlanPubId, Guid RevisionPubId, Guid ZonePubId,
    Guid? PassPubId, Guid? ExpectedToken,
    [param: System.ComponentModel.DataAnnotations.MaxLength(120)] string Name,
    DateOnly PlannedLocalDate, decimal TreatedAreaM2, decimal CarrierVolumeLitresPerHa,
    Guid? MachineryPubId, IReadOnlyList<Guid> ItemPubIds, bool Dissolve = false);

public sealed record ApplicationPlanPassDto(Guid PubId, Guid ConcurrencyToken, string Name,
    DateOnly PlannedLocalDate, decimal TreatedAreaM2, decimal CarrierVolumeLitresPerHa,
    Guid? MachineryPubId, IReadOnlyList<Guid> ItemPubIds,
    string MachineryName = "", decimal? TotalCarrierVolumeLitres = null,
    int? FullTankLoads = null, decimal? PartialTankVolumeLitres = null, int? TotalTankLoads = null);
