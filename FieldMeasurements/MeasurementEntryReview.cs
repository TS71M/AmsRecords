using AmsRecords.Assistant;
using Lib.Routing;
using static AmsRecords.FieldMeasurements.FieldMeasurementDtos;
using static Lib.Records.ApiResponseDtos;

namespace AmsRecords.FieldMeasurements;

// Entry-only presentation mapping. Original recording setup owns authorization,
// eligible targets and configured units; these snapshots contain no history.
public static class MeasurementEntryReview
{
    public static GreenSpeedSurfaceDto? ResolveGreenSpeedTarget(this GreenSpeedTimelineDto entry, AssistantDraftDto draft)
    {
        if (!MatchesEntry(draft, AssistantDraftKinds.GreenSpeed, entry.FieldPubId, entry.UnitPubId)
            || draft.SurfacePubId is null || draft.SurfacePubId == Guid.Empty
            || !draft.MeasuredAt.HasValue) return null;
        var matches = entry.GreenSpeedSurfaces.Where(surface => surface.SurfacePubId == draft.SurfacePubId).Take(2).ToArray();
        return matches is [var target] && target.AreaPubId == draft.AreaPubId ? target : null;
    }

    public static MeasurementRecordingAreaDto? ResolveCuttingHeightTarget(this MeasurementRecordingSetupDto entry, AssistantDraftDto draft)
    {
        if (!MatchesEntry(draft, AssistantDraftKinds.CuttingHeight, entry.FieldPubId, entry.UnitPubId)
            || draft.SurfacePubId.HasValue || !draft.MeasuredOn.HasValue) return null;
        var matches = entry.Areas.Where(area => area.AreaPubId == draft.AreaPubId).Take(2).ToArray();
        return matches is [var target] ? target : null;
    }

    static bool MatchesEntry(AssistantDraftDto draft, string kind, Guid field, Guid? unit)
        => draft.Kind == kind && draft.Id != Guid.Empty && field != Guid.Empty && draft.FieldPubId == field
            && unit.HasValue && unit != Guid.Empty && draft.UnitPubId == unit
            && draft.AreaPubId.HasValue && draft.AreaPubId != Guid.Empty && draft.Value is > 0;

    public static string RecordingPath(Guid field, string kind) => $"/{ApiControllers.FieldMeasurements}/" + (kind switch {
        AssistantDraftKinds.GreenSpeed => "green-speeds",
        AssistantDraftKinds.CuttingHeight => "cutting-heights",
        AssistantDraftKinds.ClippingVolume => "clipping-volumes",
        _ => throw new ArgumentException("Unsupported measurement kind.", nameof(kind)) }) + $"/recording-setup/{field:D}";

    public static GreenSpeedTimelineDto GreenSpeedEntry(this MeasurementRecordingSetupDto setup)
        => new(setup.FieldPubId, setup.FieldName, setup.Unit, setup.GreenSpeedSurfaces, [], setup.UnitPubId);
    public static ClippingVolumeTimelineDto ClippingEntry(this MeasurementRecordingSetupDto setup)
        => new(setup.FieldPubId, setup.FieldName, setup.Unit, setup.ClippingSurfaces ?? [], [], setup.UnitPubId);

    public static ApiResponse<GreenSpeedTimelineDto> GreenSpeedEntryResponse(this ApiResponse<MeasurementRecordingSetupDto> response, Guid field)
        => Map(response, field, GreenSpeedEntry);
    public static ApiResponse<ClippingVolumeTimelineDto> ClippingEntryResponse(this ApiResponse<MeasurementRecordingSetupDto> response, Guid field)
        => Map(response, field, ClippingEntry);
    static ApiResponse<T> Map<T>(ApiResponse<MeasurementRecordingSetupDto> response, Guid field, Func<MeasurementRecordingSetupDto, T> map)
        => response.Success && response.Data is { } data && data.FieldPubId == field && data.UnitPubId != Guid.Empty
            ? Ok(map(data)) : Fail<T>(response.Success ? 409 : response.StatusCode, response.ErrorMessage ?? "Recording setup could not be loaded.");
}
