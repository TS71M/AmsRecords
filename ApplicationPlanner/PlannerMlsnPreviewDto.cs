using Lib.Planning;
using static AmsRecords.SoilTests.SoilGuidelineDtos;

namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerMlsnPreviewRequest(Guid FieldPubId, Guid PlanPubId, Guid RevisionPubId, Guid ZonePubId,
    Guid SoilTestPubId, Guid ChemicalPubId, decimal? ExpectedAnnualUseGramsPerM2,
    decimal? RootzoneDepthCm, decimal? BulkDensityGramsPerCm3);

public sealed record ApplyPlannerMlsnTargetDto(PlannerMlsnPreviewRequest Request, string ExpectedPreviewToken,
    string ExpectedTargetsToken, Guid NutrientPubId, bool ConfirmSampleRepresentsZone);

// A sampled-surface budget, not evidence that one sample represents a whole zone.
public sealed record PlannerMlsnPreviewDto(PlannerMlsnPreviewRequest Request, int PlanningYear,
    DateTime EvidenceCapturedAtUtc, DateTime SampleDate, Guid SurfacePubId, string SurfaceName,
    string Element, decimal SoilTestMgPerKg, SoilEffectiveAnalyteGuidelineDto Guideline,
    MlsnNutrientBudgetPolicy.Result Budget, string PreviewToken);
