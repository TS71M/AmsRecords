using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;

namespace AmsRecords.ApplicationPlanner;

/// <summary>Human-reviewed additions to a selected draft, never execution records.
/// Rates and areas have already been explicitly interpreted in the frozen plan's units.</summary>
public sealed record PlannerImportPlanDto(
    SaveApplicationPlanGridDto Grid,
    string SourceHash,
    bool ConfirmedPlanningIntent,
    bool ConfirmedProductDetails,
    IReadOnlyList<PlannerImportPlanSourceDto> Sources);

public sealed record PlannerImportPlanSourceDto(Guid ClientEntryKey, string SourceId, string SourceExcerpt);
