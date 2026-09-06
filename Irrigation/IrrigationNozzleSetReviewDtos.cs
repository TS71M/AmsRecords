using static AmsRecords.Irrigation.IrrigationCatalogImportDtos;

namespace AmsRecords.Irrigation;

public sealed record NozzleSetReviewSource(Guid? CandidatePubId, Guid? BatchPubId,
    string DocumentTitle, string Revision, int SourcePage, string Status, string Relationship,
    IReadOnlyList<IrrigationCatalogReconciliationDifferenceDto> Differences);

public sealed record NozzleSetReviewItem(Guid CandidatePubId, Guid BatchPubId, string CandidateType,
    string Summary, int SourcePage, string DocumentTitle, string Action, string PayloadJson);

public sealed record NozzleSetReviewPackage(Guid CandidatePubId, Guid BatchPubId, string Status,
    IrrigationCatalogNozzleSetCandidate Set, string Recommendation, bool CanAccept, string ReviewToken,
    IReadOnlyList<string> Issues, IReadOnlyList<NozzleSetReviewSource> Sources,
    IReadOnlyList<NozzleSetReviewItem> Items, IReadOnlyList<string> ExistingComponents,
    IReadOnlyList<string> CompatibleModels, IReadOnlyList<string> Warnings);

public sealed record NozzleSetPackageApproval(
    [property: Required, MaxLength(64)] string ReviewToken,
    [property: MaxLength(2000)] string? ReviewerNotes);

public sealed record NozzleSetPackageResult(Guid CandidatePubId, int AcceptedCount, bool AlreadyAccepted);

public static class IrrigationNozzleSetRoles
{
    // Mechanical requirements belong to the documented set, not to its spray outlets.
    public static bool IsMechanicalRequirement(string? role) => role?.Trim().ToUpperInvariant()
        is "STATOR" or "RESTRICTOR" or "ADAPTER" or "ADAPTOR";
}
