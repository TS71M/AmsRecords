namespace AmsRecords.ApplicationPlanner;

// Human-entered provenance, never an AI-observed quotation. Stored in the
// existing immutable import evidence, not a second application ledger.
public sealed record PlannerImportManualEntryDto(string Note);
