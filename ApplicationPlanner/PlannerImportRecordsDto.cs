using System.Text.Json.Serialization;

namespace AmsRecords.ApplicationPlanner;

/// <summary>Explicitly reviewed completed work, not raw model output.</summary>
public sealed record PlannerImportRecordsDto(Guid FieldPubId, Guid SubmissionId, string SourceHash,
    bool ConfirmedCompletedWork, bool ConfirmedProductDetails,
    IReadOnlyList<PlannerImportRecordDto> Records,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool ConfirmedPossibleDuplicates = false);
public sealed record PlannerImportRecordDto(string SourceId, string SourceExcerpt, RecordFieldApplicationDto Application,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<PlannerImportProductSourceDto>? ProductSources = null);
public sealed record PlannerImportProductSourceDto(Guid ProductPubId, string SourceId, string SourceExcerpt);
public sealed record PlannerImportRecordsResultDto(Guid SubmissionId, IReadOnlyList<Guid> PassPubIds);
