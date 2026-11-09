namespace AmsRecords;

public sealed record LabReportDocumentDto(Guid PubId, string FileName, string MimeType, int ByteLength)
{
    public IReadOnlyList<LabReportExistingTestDto> ExistingTests { get; init; } = [];
    public bool HasSavedResults { get; init; }
}

public sealed record LabReportExistingTestDto(Guid PubId, string Label, DateTime Date, Guid? SurfacePubId = null);

public sealed record LabReportDocumentListItemDto(Guid PubId, string FileName, string MimeType, int ByteLength,
    string ModuleKey, DateTime UploadedUtc, bool HasSavedResults);
public sealed record LabReportDocumentListDto(IReadOnlyList<LabReportDocumentListItemDto> Documents, bool HasMore, int NextOffset);
