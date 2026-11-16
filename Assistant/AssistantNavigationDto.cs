namespace AmsRecords.Assistant;

// Rendered, audited GET links. URLs stay in the originating browser, not the model.
public sealed record AssistantNavigationTargetDto(Guid Id, string Label, string Kind,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    bool TdrAnalysisFilters = false,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    bool NextPendingProduct = false,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    bool PlannerFrozenInputs = false,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    string? PlannerCaptureProductName = null,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    string? ModuleFieldAccessKey = null,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    bool FieldApplicationCorrection = false,
    [property: System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    bool FieldApplicationHistoryDay = false)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public AssistantPlanningHubTargetDto? PlanningHub { get; init; }

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public int? PlanningHubYear { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public Guid? PlanningHubFilterFieldId { get; init; }

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public bool PlannerMlsn { get; init; }

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public Guid? SoilStandardId { get; init; }
}

public sealed record AssistantPlanningHubTargetDto(int Year, Guid FieldId, Guid? PlanId);
public sealed record AssistantNavigationSnapshotDto(Guid Id, IReadOnlyList<AssistantNavigationTargetDto> Targets);
public sealed record AssistantNavigationPatchDto(Guid SnapshotId, Guid TargetId, bool ContinueFilling = false, bool ContinueNavigation = false)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public Guid? MlsnZoneId { get; init; }

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public string? MlsnResumeQuestion { get; init; }
}
public static class AssistantNavigationPolicy
{
    public static bool Valid(AssistantNavigationSnapshotDto? snapshot) => snapshot is null
        || snapshot.Id != Guid.Empty && snapshot.Targets is { Count: > 0 and <= 40 }
        && snapshot.Targets.All(t => t is not null && t.Id != Guid.Empty && !string.IsNullOrWhiteSpace(t.Label)
            && t.Label.Length <= 160 && t.Kind is "page" or "form" or "menu"
            && (t.PlanningHub is null || t.PlanningHub is { Year: >= 1900 and <= 2100 } hub
                && hub.FieldId != Guid.Empty && hub.PlanId != Guid.Empty && t.Kind == (hub.PlanId.HasValue ? "page" : "form"))
            && (t.PlanningHubYear is null || t.PlanningHubYear is >= 1900 and <= 2100 && t.PlanningHub is null && t.Kind == "menu")
            && (t.PlanningHubFilterFieldId is null || t.PlanningHubYear.HasValue && t.PlanningHubFilterFieldId != Guid.Empty)
            && (!(t.TdrAnalysisFilters || t.NextPendingProduct) || t.Kind == "form")
            && (!t.PlannerFrozenInputs || t.Kind == "page")
            && (!t.PlannerMlsn || t.Kind == "form" && !t.TdrAnalysisFilters && !t.NextPendingProduct && t.PlanningHub is null)
            && (t.SoilStandardId is null || t.SoilStandardId != Guid.Empty && t.Kind == "form")
            && (!t.FieldApplicationCorrection || t.Kind == "page" && !t.PlannerFrozenInputs && t.ModuleFieldAccessKey is null)
            && (!t.FieldApplicationHistoryDay || t.Kind == "menu" && !t.PlannerFrozenInputs && t.ModuleFieldAccessKey is null)
            && (t.ModuleFieldAccessKey is null || t.Kind == "page" && !t.PlannerFrozenInputs
                && AmsRecords.Ibus.ModuleMenuCatalog.Modules.Any(module => module.Key == t.ModuleFieldAccessKey))
            && (t.PlannerCaptureProductName is null || t.PlannerFrozenInputs
                && t.PlannerCaptureProductName.Length <= 250 && !string.IsNullOrWhiteSpace(t.PlannerCaptureProductName)))
        && snapshot.Targets.Count(t => t.NextPendingProduct) <= 1
        && snapshot.Targets.Count(t => t.PlannerFrozenInputs) <= 1
        && snapshot.Targets.Count(t => t.PlannerMlsn) <= 1
        && snapshot.Targets.Count(t => t.FieldApplicationCorrection) <= 1
        && snapshot.Targets.Count(t => t.FieldApplicationHistoryDay) <= 1
        && snapshot.Targets.Count(t => t.PlanningHubYear.HasValue) <= 1
        && snapshot.Targets.Where(t => t.ModuleFieldAccessKey is not null).Select(t => t.ModuleFieldAccessKey).Distinct().Count()
            == snapshot.Targets.Count(t => t.ModuleFieldAccessKey is not null)
        && snapshot.Targets.Select(t => t.Id).Distinct().Count() == snapshot.Targets.Count;
}
