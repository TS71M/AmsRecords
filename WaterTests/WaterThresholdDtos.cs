namespace AmsRecords.WaterTests;

public sealed record WaterThresholdRowDto(string ResultCode, decimal? MinValue, decimal? OptValue, decimal? MaxValue);
public sealed record WaterThresholdSettingsDto(Guid OwnerPubId, bool IbuScope, bool CanEdit, List<WaterThresholdRowDto> Rows, Guid? IbuPubId = null, string? OwnerName = null,
    List<EffectiveWaterThresholdDto>? InheritedRows = null);
public sealed record WaterThresholdSaveDto(Guid OwnerPubId, bool IbuScope, List<WaterThresholdRowDto> Rows);
public sealed record EffectiveWaterThresholdDto(string ResultCode, decimal? MinValue, decimal? OptValue,
    decimal? MaxValue, string UnitShort, string? Basis, string Source, Guid OwnerPubId,
    string? SourceReference = null, string? SourceUrl = null, string? Guidance = null, bool MaximumExclusive = false);
