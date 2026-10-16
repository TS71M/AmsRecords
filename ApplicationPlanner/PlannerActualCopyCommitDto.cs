namespace AmsRecords.ApplicationPlanner;

// The destination is an explicitly measured, frozen area, not a live surface
// selection inferred from historical coverage or a matching zone name.
public sealed record PlannerActualCopyCommitDto(PlannerActualCopyPreviewRequest Request,
    string ExpectedPreviewToken, Guid OperationId, string PlanName, string ZoneName,
    bool ConfirmedFrozenInputs = false);
public sealed record PlannerActualCopyCommitResultDto(Guid PlanPubId, Guid RevisionPubId, Guid ZonePubId);
