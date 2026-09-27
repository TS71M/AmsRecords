namespace AmsRecords.ApplicationPlanner;

public sealed record CorrectFieldApplicationPassDto(Guid FieldPubId, Guid ExecutionPassPubId,
    Guid SubmissionId, IReadOnlyList<Guid> ExpectedExecutionPubIds,
    [param: System.ComponentModel.DataAnnotations.MaxLength(1000)] string Reason,
    bool Void = false, DateOnly? ExecutedLocalDate = null, decimal? ActualTreatedAreaM2 = null,
    decimal? ActualCarrierVolumeLitres = null, bool ClearCarrierVolume = false,
    IReadOnlyList<CorrectApplicationPassProductDto>? Products = null);
