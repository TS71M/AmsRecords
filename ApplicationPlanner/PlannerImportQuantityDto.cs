using Lib.Planning;

namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerImportQuantityDto(Guid FieldPubId, int Year, Guid PlanPubId, Guid RevisionPubId,
    Guid ZonePubId, Guid ProductSnapshotPubId, string ExpectedEditToken,
    decimal TreatedAreaM2, decimal Quantity, FertilizerProductQuantityUnit QuantityUnit);

public sealed record PlannerImportQuantityResultDto(Guid ProductSnapshotPubId, FertilizerApplicationRateUnit RateUnit,
    decimal Rate, decimal RequestedQuantity, decimal ScheduledQuantity,
    FertilizerProductQuantityUnit QuantityUnit, decimal QuantityDifference, string EditToken);
