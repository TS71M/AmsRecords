namespace AmsRecords.ApplicationPlanner;

public sealed record PlannerImportSheetDto(string Id, string Name, bool Hidden);
public sealed record PlannerImportWorkbookDto(string SourceHash, IReadOnlyList<PlannerImportSheetDto> Sheets);
public sealed record PlannerImportPreviewDto(Guid FieldPubId, string SourceHash, string Intent,
    IReadOnlyList<PlannerImportSourceBlock> Sources, IReadOnlyList<string> SourceWarnings,
    PlannerImportExtractionDto Extraction);
