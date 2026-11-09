using Lib.Planning;
using System.Text.Json.Serialization;

namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerNitrogenAlternativeProductDto(Guid ProductSnapshotPubId, decimal NitrogenSharePercent);
public sealed record PlannerNitrogenAlternativeOptionsDto(Guid ZonePubId, IReadOnlyList<Guid> SourceProductSnapshotPubIds,
    IReadOnlyList<PlannerNitrogenAlternativeProductDto> Products, DateOnly StartDate, DateOnly EndDate,
    int IntervalDays, decimal CutoffPercent = 40, string NitrogenBasis = "Annual",
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool FindCheaper = false);
public sealed record PlannerNitrogenAlternativeProductPreviewDto(Guid ProductSnapshotPubId, string ProductName,
    FertilizerApplicationRateUnit RateUnit, FertilizerProductQuantityUnit Unit, decimal Quantity,
    ApplicationPlanProductGuidance.Analysis NitrogenAnalysis);
public sealed record PlannerNitrogenAlternativeNutrientDto(string Code, decimal? SourceGramsPerM2, decimal? ProposedGramsPerM2);
public sealed record PlannerNitrogenAlternativePreviewDto(PlannerNitrogenAlternativeOptionsDto Options,
    decimal TreatedAreaM2, NitrogenAlternativeSchedule.Result Schedule,
    IReadOnlyList<Guid> ReplacedItemPubIds, IReadOnlyList<PlannerNitrogenAlternativeProductPreviewDto> Products,
    IReadOnlyList<PlannerNitrogenAlternativeNutrientDto> Nutrients, PlannerAllocationGrowthGuidanceDto GrowthGuidance,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] PlannerNitrogenAlternativeCostDto? CostComparison = null);
public sealed record PlannerNitrogenAlternativeCostDto(StockFirstNitrogenCostSearch.Result Result, IReadOnlyDictionary<Guid, string> ProductNames);
