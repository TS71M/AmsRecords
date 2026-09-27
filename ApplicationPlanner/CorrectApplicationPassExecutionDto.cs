namespace AmsRecords.ApplicationPlanner;

public sealed record CorrectApplicationPassExecutionDto(Guid AnnualPlanPubId, Guid ExecutionPassPubId,
    Guid SubmissionId, IReadOnlyList<Guid> ExpectedExecutionPubIds,
    [param: System.ComponentModel.DataAnnotations.MaxLength(1000)] string Reason,
    bool Void = false, DateOnly? ExecutedLocalDate = null, decimal? ActualTreatedAreaM2 = null,
    decimal? ActualCarrierVolumeLitres = null, bool ClearCarrierVolume = false,
    IReadOnlyList<CorrectApplicationPassProductDto>? Products = null);

public sealed record CorrectApplicationPassProductDto(Guid ExecutionPubId, decimal? ActualRate = null,
    decimal? ActualCost = null, bool? CompletesPlannedApplication = null,
    [param: System.ComponentModel.DataAnnotations.MaxLength(2000)] string? Notes = null,
    decimal? ActualProductQuantity = null, string? ActualProductQuantityUnit = null);

public sealed record ApplicationExecutionPassDetailsDto(Guid PubId, Guid AnnualPlanPubId, Guid RevisionPubId,
    Guid SourcePlanPassPubId, DateOnly ExecutedLocalDate, decimal TreatedAreaM2,
    decimal? ActualCarrierVolumeLitres, IReadOnlyList<ApplicationExecutionPassProductDto> Products);

public sealed record ApplicationExecutionPassProductDto(Guid ExecutionPubId, string ProductName, string ZoneName,
    string MachineryName, decimal ActualRate, string? RateUnit, decimal ProductQuantity, string? QuantityUnit,
    decimal ActualCost, bool CompletesPlannedApplication, string Notes);
