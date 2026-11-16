using Lib.Lookups.WaterTests;
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
            && input.Items.Select((sample, index) => {
                var extracted = preview.Surfaces[index].Results;
                return sample.Results.Count >= extracted.Count
                    && sample.Results.Take(extracted.Count).Select(reading => reading.DetectedLabel)
                        .SequenceEqual(extracted.Select(reading => reading.DetectedLabel), StringComparer.Ordinal)
                    && sample.Results.Skip(extracted.Count).All(reading => reading.DetectedLabel is null && reading.AnalytePubId != Guid.Empty
                        && (reading.Value.HasValue || !string.IsNullOrWhiteSpace(reading.TextValue)));
            }).All(valid => valid) ? input : null;
    }

    public static WaterTestDocumentImportCreateDto? WaterInput(Assistant.AssistantDraftDto draft, LabReportDocumentDto source)
    {
        if (draft.WaterImport is not { } preview) return null;
        var input = draft.WaterReview ?? preview.ToReviewInput(source, draft.Id);
        return input.FieldPubId == draft.FieldPubId && input.OriginalDocumentPubId == source.PubId && input.RequestId != Guid.Empty
            && HasMatchingWaterRows(preview, input) ? input : null;
    }

    public static bool HasMatchingWaterRows(WaterTestDocumentImportPreviewDto preview, WaterTestDocumentImportCreateDto input)
        => input.Results is not null && preview.Results is not null && input.Results.Count >= preview.Results.Count
            && input.Results.Count <= WaterTestCatalog.All.Count
            && input.Results.Take(preview.Results.Count).Select(row => row.ResultCode)
                .SequenceEqual(preview.Results.Select(row => row.ResultCode), StringComparer.Ordinal)
            && input.Results.Select(row => row.ResultCode).Distinct(StringComparer.OrdinalIgnoreCase).Count() == input.Results.Count
            && input.Results.Skip(preview.Results.Count).All(row => row.DetectedLabel is null && !row.RequiresSourceReview
                && WaterTestCatalog.Find(row.ResultCode) is not null
                && !WaterTestCatalog.NeedsMappingReview(row.ResultCode, row.ReportedUnit, row.ReportedBasis));

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
