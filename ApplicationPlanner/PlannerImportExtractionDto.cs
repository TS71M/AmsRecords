namespace AmsRecords.ApplicationPlanner;

/// <summary>Selected source content, not instructions. Source IDs/locations are
/// assigned by the upload reader so extracted rows remain traceable.</summary>
public sealed record PlannerImportSourceBlock(string SourceId, string Location, string Text);

/// <summary>Unapproved transcription. Raw values intentionally remain strings:
/// interpretation, catalogue matching and permissions belong to review/commit.</summary>
public sealed record PlannerImportExtractedRow(string SourceId, string SourceExcerpt,
    string Classification, string? DateText, string ProductText, string? FormulationText,
    string? RateText, string? QuantityText, string? UnitText, string? AreaText, string? NotesText);

public sealed record PlannerImportExtractionDto(bool? ExtractionComplete,
    IReadOnlyList<string> CompletedSourceIds, IReadOnlyList<PlannerImportExtractedRow> Rows,
    IReadOnlyList<string> Warnings);
