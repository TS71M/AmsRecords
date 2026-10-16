using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;
using System.Text.Json.Serialization;

namespace AmsRecords.ApplicationPlanner;

/// <summary>Human-reviewed additions to a selected draft, never execution records.
/// Rates and areas have already been explicitly interpreted in the frozen plan's units.</summary>
public sealed record PlannerImportPlanDto(
    SaveApplicationPlanGridDto Grid,
    string SourceHash,
    bool ConfirmedPlanningIntent,
    bool ConfirmedProductDetails,
    IReadOnlyList<PlannerImportPlanSourceDto> Sources,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool ConfirmedPossibleDuplicates = false);

public sealed record PlannerImportPlanSourceDto(Guid ClientEntryKey, string SourceId, string SourceExcerpt);
