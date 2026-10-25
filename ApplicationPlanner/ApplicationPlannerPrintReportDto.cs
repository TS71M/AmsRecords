using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;

namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationPlannerPrintRequestDto(Guid FieldPubId, int? Year = null,
    Guid? PlanPubId = null, Guid? RevisionPubId = null, Guid? ZonePubId = null,
    bool IncludeDetails = false, bool IncludeComparison = false,
    bool IncludeEvidence = false, bool IncludeMaterials = false, bool IncludeGrowth = false);

// A single response owns all optional sections. GeneratedAtUtc describes this
// report, not the capture date of its frozen evidence or the execution dates.
public sealed record ApplicationPlannerPrintReportDto(ApplicationPlanCalendarDto Calendar, DateTime GeneratedAtUtc,
    ApplicationPlannerPrintDetailsDto? Details = null,
    ApplicationPlanningEvidenceViewDto? Evidence = null,
    ApplicationPlannerMaterialsDto? Materials = null, PlannerAllocationGrowthGuidanceDto? GrowthGuidance = null);
