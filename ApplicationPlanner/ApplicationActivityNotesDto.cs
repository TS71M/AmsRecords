namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationActivityNoteDto(Guid PubId, DateTime RecordedAtUtc, string Type,
    string Reason, string? SuggestedAction, string? Severity, decimal? CostDelta);
public sealed record ApplicationActivityNotesDto(Guid PlanPubId, string Kind, int Page, int PageSize,
    int TotalCount, IReadOnlyList<ApplicationActivityNoteDto> Rows);
