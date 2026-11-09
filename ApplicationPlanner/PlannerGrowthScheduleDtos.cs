using Lib.Planning;
using System.Text.Json.Serialization;

namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerGrowthScheduleRequestDto(PlannerAllocationScopeDto Scope, string ExpectedCalendarToken,
    DateOnly StartDate, DateOnly EndDate, decimal NitrogenAtFullGrowthGramsPerM2,
    decimal CutoffPercent, decimal TreatedAreaM2, int IntervalDays = 14,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool AdjustExisting = false);
public sealed record PlannerGrowthScheduleEntryDto(DateOnly Date, int IsoWeek, decimal GrowthPotentialPercent,
    decimal RequestedNitrogenGramsPerM2, decimal DeliveredNitrogenGramsPerM2, decimal Rate,
    decimal ProductQuantity, bool BelowCutoff);
public sealed record PlannerGrowthSchedulePreviewDto(PlannerGrowthScheduleRequestDto Request,
    string ProductName, FertilizerProductQuantityUnit Unit, FertilizerApplicationRateUnit RateUnit,
    PlannerAllocationGrowthGuidanceDto GrowthGuidance, bool AddsProductToZone,
    IReadOnlyList<PlannerGrowthScheduleEntryDto> Entries, string PreviewToken = "", int OtherPlannedProducts = 0,
    ApplicationPlanProductGuidance.Analysis? NitrogenAnalysis = null, decimal? DensityKgPerLitre = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<PlannerGrowthScheduleReplacedEntryDto>? ReplacedEntries = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] GrowthScheduleProgramme? PreviousProgramme = null);
public sealed record PlannerGrowthScheduleReplacedEntryDto(Guid PlanItemPubId, DateOnly Date, decimal Rate, decimal ProductQuantity);
public sealed record CommitPlannerGrowthScheduleDto(PlannerGrowthScheduleRequestDto Request, string ExpectedPreviewToken, Guid OperationId);
