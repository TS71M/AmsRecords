namespace AmsRecords.Ibus;

public static class IbuModuleSettingDtos
{
    public sealed record IbuModulePreferenceDto(
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("isEnabled")] bool IsEnabled,
        [property: JsonPropertyName("restrictToSelectedFields")] bool RestrictToSelectedFields = false);

    public sealed record IbuModulePreferencesDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("modules")] List<IbuModulePreferenceDto> Modules,
        [property: JsonPropertyName("canEdit")] bool CanEdit);

    public sealed record SaveIbuModulePreferencesDto(
        [property: JsonPropertyName("modules")] List<IbuModulePreferenceDto> Modules);

    public sealed record SaveIbuModulePreferenceDto(
        [property: JsonPropertyName("isEnabled")] bool IsEnabled);

    public sealed record IbuModuleFieldItemDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("isSelected")] bool IsSelected);

    [method: JsonConstructor]
    public sealed record IbuModuleFieldPreferencesDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("moduleKey")] string ModuleKey,
        [property: JsonPropertyName("isEnabled")] bool IsEnabled,
        [property: JsonPropertyName("restrictToSelectedFields")] bool RestrictToSelectedFields,
        [property: JsonPropertyName("fields")] IReadOnlyList<IbuModuleFieldItemDto> Fields,
        [property: JsonPropertyName("canEdit")] bool CanEdit,
        [property: JsonPropertyName("reviewStamp"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? ReviewStamp = null)
    {
        [JsonPropertyName("hasUnlistedSelections"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool HasUnlistedSelections { get; init; }

        public IbuModuleFieldPreferencesDto(Guid IbuPubId, string ModuleKey, bool IsEnabled,
            bool RestrictToSelectedFields, IReadOnlyList<IbuModuleFieldItemDto> Fields, bool CanEdit)
            : this(IbuPubId, ModuleKey, IsEnabled, RestrictToSelectedFields, Fields, CanEdit, null)
        {
        }

        public void Deconstruct(out Guid IbuPubId, out string ModuleKey, out bool IsEnabled,
            out bool RestrictToSelectedFields, out IReadOnlyList<IbuModuleFieldItemDto> Fields, out bool CanEdit)
            => (IbuPubId, ModuleKey, IsEnabled, RestrictToSelectedFields, Fields, CanEdit) =
                (this.IbuPubId, this.ModuleKey, this.IsEnabled, this.RestrictToSelectedFields, this.Fields, this.CanEdit);
    }

    [method: JsonConstructor]
    public sealed record SaveIbuModuleFieldPreferencesDto(
        [property: JsonPropertyName("restrictToSelectedFields")] bool RestrictToSelectedFields,
        [property: JsonPropertyName("fieldPubIds")] IReadOnlyList<Guid> FieldPubIds,
        [property: JsonPropertyName("expectedReviewStamp"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? ExpectedReviewStamp = null)
    {
        [JsonPropertyName("reviewedRemovalPubIds"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IReadOnlyList<Guid>? ReviewedRemovalPubIds { get; init; }

        public SaveIbuModuleFieldPreferencesDto(bool RestrictToSelectedFields, IReadOnlyList<Guid> FieldPubIds)
            : this(RestrictToSelectedFields, FieldPubIds, null)
        {
        }

        public void Deconstruct(out bool RestrictToSelectedFields, out IReadOnlyList<Guid> FieldPubIds)
            => (RestrictToSelectedFields, FieldPubIds) = (this.RestrictToSelectedFields, this.FieldPubIds);
    }
}
