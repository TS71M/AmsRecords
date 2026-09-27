namespace AmsRecords.ApplicationPlanner;

public sealed record RecordApplicationPassExecutionDto(Guid AnnualPlanPubId, Guid RevisionPubId,
    Guid PlannedPassPubId, Guid SubmissionId, DateOnly ExecutedLocalDate,
    decimal ActualTreatedAreaM2, decimal? ActualCarrierVolumeLitres, Guid? MachineryPubId,
    IReadOnlyList<RecordApplicationPassProductDto> Products);

public sealed record RecordApplicationPassProductDto(Guid PlannedApplicationPubId, Guid? ActualProductPubId,
    decimal ActualRate, decimal ActualCost, bool CompletesPlannedApplication,
    [param: System.ComponentModel.DataAnnotations.MaxLength(2000)] string Notes,
    decimal? ActualProductQuantity = null, string? ActualProductQuantityUnit = null);
