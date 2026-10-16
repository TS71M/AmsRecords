using AmsRecords.Weather;

namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationPlanningEvidenceViewDto(Guid RevisionPubId, string Status, ApplicationPlanningEvidenceDto? Evidence);

public sealed record RefreshPlanningEvidenceDto(Guid FieldPubId, Guid PlanPubId, Guid RevisionPubId,
    bool PreviewOnly = true, string? ExpectedToken = null);
public sealed record PlanningEvidenceRefreshResultDto(Guid RevisionPubId,
    ApplicationPlanningEvidenceDto? Before, ApplicationPlanningEvidenceDto After,
    IReadOnlyList<string> ChangedSections, string ExpectedToken, bool Applied);

// Persisted versioned contract: never rebuild an older capture from current data.
public sealed record ApplicationPlanningEvidenceDto(
    int Version, DateTime CapturedAtUtc, Guid FieldPubId,
    PlanningLocationEvidenceDto Location,
    GrowthPotentialDtos.GrowthPotentialProfileDto? GrowthPotential,
    IReadOnlyList<PlanningAreaEvidenceDto> Areas,
    PlanningClimateEvidenceDto? ClimateNormal = null);

public sealed record PlanningClimateEvidenceDto(Guid SourcePubId, string SourceType,
    short? FromYear, short? ToYear, DateTime UpdatedAtUtc, IReadOnlyList<PlanningClimateMonthDto> Months);
public sealed record PlanningClimateMonthDto(Guid SourcePubId, byte Month, decimal MeanTemperatureC);

public sealed record PlanningLocationEvidenceDto(
    Guid? CoordinatePubId, decimal? Latitude, decimal? Longitude, decimal? Elevation,
    string? ClimateZone, string? Jurisdiction);

public sealed record PlanningAreaEvidenceDto(
    Guid AreaPubId, string Name, Guid? CompositionPubId, DateTime? CompositionValidFromUtc,
    IReadOnlyList<PlanningGrassEvidenceDto> Grass,
    IReadOnlyList<PlanningSoilEvidenceDto> SoilTests);

public sealed record PlanningGrassEvidenceDto(Guid? SpeciesPubId, string Name, decimal Percent);

public sealed record PlanningSoilEvidenceDto(
    Guid SurfacePubId, Guid TestPubId, DateTime SampleDate, string? Method,
    IReadOnlyList<PlanningSoilValueDto> Values, string SurfaceName = "",
    IReadOnlyList<AmsRecords.SoilTests.SoilGuidelineDtos.SoilEffectiveAnalyteGuidelineDto>? Guidelines = null);

public sealed record PlanningSoilValueDto(Guid ChemicalPubId, string Name, decimal Value, string? Unit,
    string? ChemicalFormula = null, bool? IsElemental = null);
