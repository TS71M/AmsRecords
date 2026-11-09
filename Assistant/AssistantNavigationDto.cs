namespace AmsRecords.Assistant;

// Rendered, audited GET links. URLs stay in the originating browser, not the model.
public sealed record AssistantNavigationTargetDto(Guid Id, string Label, string Kind);
public sealed record AssistantNavigationSnapshotDto(Guid Id, IReadOnlyList<AssistantNavigationTargetDto> Targets);
public sealed record AssistantNavigationPatchDto(Guid SnapshotId, Guid TargetId, bool ContinueFilling = false, bool ContinueNavigation = false);
public static class AssistantNavigationPolicy
{
    public static bool Valid(AssistantNavigationSnapshotDto? snapshot) => snapshot is null
        || snapshot.Id != Guid.Empty && snapshot.Targets is { Count: > 0 and <= 40 }
        && snapshot.Targets.All(t => t is not null && t.Id != Guid.Empty && !string.IsNullOrWhiteSpace(t.Label)
            && t.Label.Length <= 160 && t.Kind is "page" or "form" or "menu")
        && snapshot.Targets.Select(t => t.Id).Distinct().Count() == snapshot.Targets.Count;
}
