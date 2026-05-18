using static AmsRecords.Weeds.WeedDtos;

namespace AmsRecords.Weeds;

public static class WeedExtensions
{
    public static WeedDto ToDto(this Weed weed)
        => new(
            PubId: weed.PubId,
            Code: weed.Code,
            DefaultName: weed.DefaultName,
            Active: weed.Active,
            SortOrder: weed.SortOrder,
            PreferredFromGts: weed.PreferredFromGts,
            PreferredToGts: weed.PreferredToGts,
            AcceptableFromGts: weed.AcceptableFromGts,
            AcceptableToGts: weed.AcceptableToGts,
            ManagementNote: weed.ManagementNote);

    public static Weed ToEntity(this WeedCreateDto dto)
        => new()
        {
            Code = dto.Code.Trim(),
            DefaultName = dto.DefaultName.Trim(),
            Active = dto.Active,
            SortOrder = dto.SortOrder,
            PreferredFromGts = dto.PreferredFromGts,
            PreferredToGts = dto.PreferredToGts,
            AcceptableFromGts = dto.AcceptableFromGts,
            AcceptableToGts = dto.AcceptableToGts,
            ManagementNote = string.IsNullOrWhiteSpace(dto.ManagementNote) ? null : dto.ManagementNote.Trim()
        };

    public static Weed UpdateEntity(this Weed entity, WeedUpdateDto dto)
    {
        entity.Code = dto.Code.Trim();
        entity.DefaultName = dto.DefaultName.Trim();
        entity.Active = dto.Active;
        entity.SortOrder = dto.SortOrder;
        entity.PreferredFromGts = dto.PreferredFromGts;
        entity.PreferredToGts = dto.PreferredToGts;
        entity.AcceptableFromGts = dto.AcceptableFromGts;
        entity.AcceptableToGts = dto.AcceptableToGts;
        entity.ManagementNote = string.IsNullOrWhiteSpace(dto.ManagementNote) ? null : dto.ManagementNote.Trim();

        return entity;
    }
}