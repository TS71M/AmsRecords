using Lib.Planning;

namespace AmsRecords.ApplicationPlanner;

// Read-only source evidence. A source row is never a newly executed application.
public sealed record PlannerActualCopySourceDto(Guid FieldPubId, int Year, string Scope,
    Guid? PlanPubId, string SourceToken, IReadOnlyList<PlannerActualCopyRowDto> Rows);

public sealed record PlannerActualCopyRowDto(Guid ExecutionPubId, Guid? PassPubId, DateOnly Date,
    Guid? PlanPubId, string ProductName, Guid? SourcePlanProductSnapshotPubId,
    string ZoneName, decimal TreatedAreaM2, decimal Quantity,
    FertilizerProductQuantityUnit? QuantityUnit, decimal Rate, FertilizerApplicationRateUnit? RateUnit,
    decimal? DensityKgPerL, IReadOnlyList<PlannerActualCopyNutrientDto> Nutrients,
    bool? AnalysisComplete = null);

public sealed record PlannerActualCopyNutrientDto(string Code, decimal AnalysisAmount,
    FertilizerNutrientAnalysisBasis AnalysisBasis, decimal TotalGrams);
