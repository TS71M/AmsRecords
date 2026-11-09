using static AmsRecords.SoilTests.SoilTestDtos;
using static AmsRecords.WaterTests.WaterTestDtos;

namespace AmsRecords;

// Shared presentation mapping, not extraction, conversion or save authorization.
public static class LabImportReview
{
    public static SoilTestDocumentImportCreateDto? SoilInput(Assistant.AssistantDraftDto draft, LabReportDocumentDto source)
    {
        if (draft.SoilImport is not { } preview) return null;
        var input = draft.SoilReview ?? preview.ToReviewInput(source);
        return input.FieldPubId == draft.FieldPubId && input.OriginalDocumentPubId == source.PubId
            && input.Items.Count == preview.Surfaces.Count
            && input.Items.Select((s, i) => s.Results.Count == preview.Surfaces[i].Results.Count).All(m => m) ? input : null;
    }

    public static WaterTestDocumentImportCreateDto? WaterInput(Assistant.AssistantDraftDto draft, LabReportDocumentDto source)
    {
        if (draft.WaterImport is not { } preview) return null;
        var input = draft.WaterReview ?? preview.ToReviewInput(source, draft.Id);
        return input.FieldPubId == draft.FieldPubId && input.OriginalDocumentPubId == source.PubId && input.RequestId != Guid.Empty
            && input.Results.Select(r => r.ResultCode).SequenceEqual(preview.Results.Select(r => r.ResultCode), StringComparer.Ordinal) ? input : null;
    }
    public static bool MatchesSource(LabReportDocumentDto? expected, LabReportDocumentDto? current)
        => expected is { PubId: var id } && id != Guid.Empty && current is not null
            && id == current.PubId && expected.ByteLength == current.ByteLength && expected.MimeType == current.MimeType;

    public static SoilTestDocumentImportCreateDto ToReviewInput(this SoilTestDocumentImportPreviewDto preview,
        LabReportDocumentDto? source = null)
    {
        source ??= preview.OriginalDocument;
        return new(preview.FieldPubId, preview.ReportedFieldName, preview.SoilTestDateUtc ?? default,
            preview.MethodCode, preview.MethodRaw,
            [.. preview.Surfaces.Select(sample => new SoilTestDocumentImportCreateItemDto(
                sample.MatchedSurfacePubId ?? Guid.Empty, sample.MatchedSurfaceName ?? sample.RawSurfaceLabel,
                sample.RawSurfaceLabel,
                sample.MatchedSurfacePubId is { } surface && surface != Guid.Empty
                    && !(source?.ExistingTests.Any(saved => saved.SurfacePubId == surface && saved.Date.Date == preview.SoilTestDateUtc?.Date) ?? false),
                [.. sample.Results.Select(r => new SoilTestDocumentImportCreateResultInputDto(r.AnalytePubId, r.Value,
                    r.UnitPubId, r.DetectedLabel, r.TextValue))], sample.SampleNotes))]) { OriginalDocumentPubId = source?.PubId };
    }

    public static WaterTestDocumentImportCreateDto ToReviewInput(this WaterTestDocumentImportPreviewDto preview,
        LabReportDocumentDto? source = null, Guid requestId = default)
        => new(preview.FieldPubId, preview.ReportedFieldName, preview.Title, preview.ReportDateUtc ?? default,
            preview.SampleReceivedDateUtc, preview.SampleTakenDateUtc, preview.ReportNumber, preview.AnalysisNumber,
            preview.ProjectName, preview.CustomerNumber, preview.SamplerName, false,
            [.. preview.Results.Select(r => new WaterTestDocumentImportResultInputDto(r.ResultCode, r.Value, r.TextValue,
                r.DetectedLabel, r.ReportedUnit, r.ReportedBasis, RequiresSourceReview: r.NeedsReview))]) {
                OriginalDocumentPubId = (source ?? preview.OriginalDocument)?.PubId, RequestId = requestId };
}
