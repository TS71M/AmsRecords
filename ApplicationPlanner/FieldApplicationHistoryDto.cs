using Lib.Planning;
using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;

namespace AmsRecords.ApplicationPlanner;

/// <summary>Calendar-year actuals, including records with no planning context.
/// Null cost/analysis status means unavailable, not zero/complete.</summary>
public sealed record FieldApplicationHistoryDto(Guid FieldPubId, int Year, int Page,
    int PageSize, int TotalCount, IReadOnlyList<FieldApplicationHistoryRowDto> Rows,
    decimal RecordedCost = 0, IReadOnlyList<ApplicationExecutionNutrientTotalDto>? RecordedNutrients = null);

public sealed record FieldApplicationHistoryRowDto(
    Guid ExecutionPubId, Guid? PassPubId, DateOnly Date,
    Guid? PlanPubId, string? PlanName, Guid? RevisionPubId, int? RevisionNumber,
    string ProductName, string ZoneName, decimal TreatedAreaM2,
    decimal Rate, FertilizerApplicationRateUnit? RateUnit,
    decimal Quantity, FertilizerProductQuantityUnit? QuantityUnit,
    decimal? Cost, bool? AnalysisComplete, string MachineName, decimal? CarrierVolumeLitres,
    string Notes, bool IsCurrent, bool IsVoided, Guid? ReplacesExecutionPubId,
    Guid? CorrectionPubId, string CorrectionReason,
    IReadOnlyList<ApplicationExecutionNutrientTotalDto> Nutrients,
    int? PlanYear = null, Guid? PlannedApplicationPubId = null);
