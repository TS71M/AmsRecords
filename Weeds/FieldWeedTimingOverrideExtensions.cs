using static AmsRecords.Weeds.FieldWeedTimingOverrideDtos;

namespace AmsRecords.Weeds;

public static class FieldWeedTimingOverrideExtensions
{
    public static FieldWeedTimingOverrideDto ToDto(this FieldWeedTimingOverride entity)
        => new(
            entity.PubId,
            entity.Field?.PubId ?? Guid.Empty,
            entity.Weed?.PubId ?? Guid.Empty,
            entity.Weed?.Code ?? string.Empty,
            entity.Weed?.DefaultName ?? string.Empty,
            entity.PreferredFromGts,
            entity.PreferredToGts,
            entity.AcceptableFromGts,
            entity.AcceptableToGts,
            entity.Note,
            entity.Active
        );

    public static FieldWeedTimingOverride ToEntity(
        this FieldWeedTimingOverrideCreateDto dto,
        int fieldId,
        int weedId)
        => new()
        {
            FieldId = fieldId,
            WeedId = weedId,
            PreferredFromGts = dto.PreferredFromGts,
            PreferredToGts = dto.PreferredToGts,
            AcceptableFromGts = dto.AcceptableFromGts,
            AcceptableToGts = dto.AcceptableToGts,
            Note = dto.Note?.Trim(),
            Active = dto.Active
        };

    public static FieldWeedTimingOverrideUpdateDto ToUpdateDto(this FieldWeedTimingOverride entity)
        => new(
            entity.PubId,
            entity.Field?.PubId ?? Guid.Empty,
            entity.Weed?.PubId ?? Guid.Empty,
            entity.PreferredFromGts,
            entity.PreferredToGts,
            entity.AcceptableFromGts,
            entity.AcceptableToGts,
            entity.Note,
            entity.Active
        );

    public static void UpdateEntity(
        this FieldWeedTimingOverride entity,
        FieldWeedTimingOverrideUpdateDto dto,
        int fieldId,
        int weedId)
    {
        entity.FieldId = fieldId;
        entity.WeedId = weedId;
        entity.PreferredFromGts = dto.PreferredFromGts;
        entity.PreferredToGts = dto.PreferredToGts;
        entity.AcceptableFromGts = dto.AcceptableFromGts;
        entity.AcceptableToGts = dto.AcceptableToGts;
        entity.Note = dto.Note?.Trim();
        entity.Active = dto.Active;
    }
}