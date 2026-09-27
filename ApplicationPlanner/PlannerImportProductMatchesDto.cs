namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerImportProductMatchRequest(Guid FieldPubId, IReadOnlyList<string> ProductNames);
/// <summary>Name matches are review candidates, never confirmed formulation matches.</summary>
public sealed record PlannerImportProductMatchDto(string SourceName,
    IReadOnlyList<FieldApplicationCandidateDto> Candidates, bool MoreCandidates);
public sealed record PlannerImportProductMatchesDto(Guid FieldPubId,
    IReadOnlyList<PlannerImportProductMatchDto> Products, bool SearchIncomplete);
