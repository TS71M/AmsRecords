namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationRevisionDiffValueDto(string Field, string? Before, string? After);
public sealed record ApplicationRevisionDiffRowDto(Guid LogicalApplicationPubId, string Change,
    string? BeforeProduct, string? AfterProduct, IReadOnlyList<ApplicationRevisionDiffValueDto> Differences);
public sealed record ApplicationRevisionDiffDto(Guid BaselineRevisionPubId, Guid ComparedRevisionPubId,
    int Year, bool OriginalIntention, int Page, int TotalDifferences, int Added, int Removed, int Changed,
    int Unchanged, bool HasUnavailableHistory, IReadOnlyList<ApplicationRevisionDiffRowDto> Rows);
