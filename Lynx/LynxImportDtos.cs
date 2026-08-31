namespace AmsRecords.Lynx;

public static class LynxImportDtos
{
    public static class ReconciliationStatuses
    {
        public const string Added = "added";
        public const string Changed = "changed";
        public const string Missing = "missing";
        public const string Unchanged = "unchanged";
    }

    public sealed record LynxImportSummaryDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("originalFileName")] string OriginalFileName,
        [property: JsonPropertyName("fileSha256")] string FileSha256,
        [property: JsonPropertyName("archiveVersion")] string? ArchiveVersion,
        [property: JsonPropertyName("importedUtc")] DateTime ImportedUtc,
        [property: JsonPropertyName("configuredStationCount")] int ConfiguredStationCount,
        [property: JsonPropertyName("mappedStationCount")] int MappedStationCount,
        [property: JsonPropertyName("unmappedStationCount")] int UnmappedStationCount,
        [property: JsonPropertyName("mapPointCount")] int MapPointCount,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("warningMessage")] string? WarningMessage,
        [property: JsonPropertyName("modelInventory")] IReadOnlyList<LynxModelInventoryDto> ModelInventory,
        [property: JsonPropertyName("tagGroups")] IReadOnlyList<LynxTagGroupInventoryDto> TagGroups);

    public sealed record LynxModelInventoryDto(
        [property: JsonPropertyName("sprinklerModel")] string SprinklerModel,
        [property: JsonPropertyName("count")] int Count);

    public sealed record LynxTagGroupInventoryDto(
        [property: JsonPropertyName("tagGroup")] string TagGroup,
        [property: JsonPropertyName("count")] int Count);

    public sealed record LynxImportStationDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("stationTag")] string StationTag,
        [property: JsonPropertyName("hardwareDescriptor")] string? HardwareDescriptor,
        [property: JsonPropertyName("satelliteNumber")] int SatelliteNumber,
        [property: JsonPropertyName("stationNumber")] int StationNumber,
        [property: JsonPropertyName("sprinklerModel")] string? SprinklerModel,
        [property: JsonPropertyName("nozzleNumber")] string? NozzleNumber,
        [property: JsonPropertyName("mapPointCount")] int MapPointCount,
        [property: JsonPropertyName("mapped")] bool Mapped,
        [property: JsonPropertyName("mapPoints")] IReadOnlyList<LynxMapPointDto> MapPoints);

    public sealed record LynxMapPointDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("pointNumber")] int PointNumber,
        [property: JsonPropertyName("x")] double X,
        [property: JsonPropertyName("y")] double Y,
        [property: JsonPropertyName("onMap")] bool OnMap);

    public sealed record LynxSynchronizationSystemOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record LynxSynchronizationAreaOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record LynxTagGroupMappingPreviewDto(
        [property: JsonPropertyName("tagGroup")] string TagGroup,
        [property: JsonPropertyName("count")] int Count,
        [property: JsonPropertyName("irrigationAreaPubId")] Guid? IrrigationAreaPubId,
        [property: JsonPropertyName("irrigationAreaName")] string IrrigationAreaName);

    public sealed record LynxHeadReconciliationDto(
        [property: JsonPropertyName("sourceReference")] string SourceReference,
        [property: JsonPropertyName("stationTag")] string StationTag,
        [property: JsonPropertyName("tagGroup")] string TagGroup,
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("irrigationHeadPubId")] Guid? IrrigationHeadPubId,
        [property: JsonPropertyName("currentHardwareAddress")] string CurrentHardwareAddress,
        [property: JsonPropertyName("sourceHardwareAddress")] string SourceHardwareAddress,
        [property: JsonPropertyName("currentMapX")] double? CurrentMapX,
        [property: JsonPropertyName("currentMapY")] double? CurrentMapY,
        [property: JsonPropertyName("sourceMapX")] double? SourceMapX,
        [property: JsonPropertyName("sourceMapY")] double? SourceMapY,
        [property: JsonPropertyName("currentSprinklerModel")] string CurrentSprinklerModel,
        [property: JsonPropertyName("sourceSprinklerModel")] string SourceSprinklerModel,
        [property: JsonPropertyName("currentNozzle")] string CurrentNozzle,
        [property: JsonPropertyName("sourceNozzle")] string SourceNozzle,
        [property: JsonPropertyName("changes")] IReadOnlyList<string> Changes,
        [property: JsonPropertyName("mapPointNumber")] int? MapPointNumber = null,
        [property: JsonPropertyName("stationMapPointCount")] int StationMapPointCount = 0);

    public sealed record LynxSynchronizationPreviewDto(
        [property: JsonPropertyName("importPubId")] Guid ImportPubId,
        [property: JsonPropertyName("irrigationSystemPubId")] Guid? IrrigationSystemPubId,
        [property: JsonPropertyName("irrigationSystemName")] string IrrigationSystemName,
        [property: JsonPropertyName("previouslySynchronized")] bool PreviouslySynchronized,
        [property: JsonPropertyName("approvalToken")] string ApprovalToken,
        [property: JsonPropertyName("addedCount")] int AddedCount,
        [property: JsonPropertyName("changedCount")] int ChangedCount,
        [property: JsonPropertyName("missingCount")] int MissingCount,
        [property: JsonPropertyName("unchangedCount")] int UnchangedCount,
        [property: JsonPropertyName("systems")] IReadOnlyList<LynxSynchronizationSystemOptionDto> Systems,
        [property: JsonPropertyName("areas")] IReadOnlyList<LynxSynchronizationAreaOptionDto> Areas,
        [property: JsonPropertyName("tagGroups")] IReadOnlyList<LynxTagGroupMappingPreviewDto> TagGroups,
        [property: JsonPropertyName("heads")] IReadOnlyList<LynxHeadReconciliationDto> Heads);

    public sealed record LynxTagGroupMappingApprovalDto(
        [property: JsonPropertyName("tagGroup")]
        [param: Required, StringLength(80, MinimumLength = 1)] string TagGroup,
        [property: JsonPropertyName("irrigationAreaPubId")] Guid? IrrigationAreaPubId);

    public sealed record LynxSynchronizationApprovalDto(
        [property: JsonPropertyName("irrigationSystemPubId")] Guid IrrigationSystemPubId,
        [property: JsonPropertyName("approvalToken")]
        [param: Required, StringLength(64, MinimumLength = 64)] string ApprovalToken,
        [property: JsonPropertyName("deactivateMissingHeads")] bool DeactivateMissingHeads,
        [property: JsonPropertyName("tagGroupMappings")] IReadOnlyList<LynxTagGroupMappingApprovalDto> TagGroupMappings);

    public sealed record LynxSynchronizationResultDto(
        [property: JsonPropertyName("importPubId")] Guid ImportPubId,
        [property: JsonPropertyName("irrigationSystemPubId")] Guid IrrigationSystemPubId,
        [property: JsonPropertyName("addedCount")] int AddedCount,
        [property: JsonPropertyName("changedCount")] int ChangedCount,
        [property: JsonPropertyName("missingCount")] int MissingCount,
        [property: JsonPropertyName("unchangedCount")] int UnchangedCount,
        [property: JsonPropertyName("deactivatedMissingCount")] int DeactivatedMissingCount);

    public sealed record LynxAreaInitializationResultDto(
        [property: JsonPropertyName("createdAreaCount")] int CreatedAreaCount,
        [property: JsonPropertyName("reusedAreaCount")] int ReusedAreaCount,
        [property: JsonPropertyName("mappedTagGroupCount")] int MappedTagGroupCount);
}
