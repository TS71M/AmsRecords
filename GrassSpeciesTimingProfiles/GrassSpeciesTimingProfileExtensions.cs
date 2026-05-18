using static AmsRecords.GrassSpeciesTimingProfiles.GrassSpeciesTimingProfileDtos;

namespace AmsRecords.GrassSpeciesTimingProfiles;

public static class GrassSpeciesTimingProfileExtensions
{
    public static GrassSpeciesTimingProfileDto ToDto(this GrassSpeciesTimingProfile entity)
        => new(
            entity.PubId,
            entity.GrassSpecies.PubId,
            entity.Code,
            entity.DefaultName,
            entity.Active,
            entity.SortOrder,
            entity.PreferredFromGts,
            entity.PreferredToGts,
            entity.AcceptableFromGts,
            entity.AcceptableToGts,
            entity.ManagementNote);

    public static GrassSpeciesTimingProfile ToEntity(this GrassSpeciesTimingProfileCreateDto dto, int grassSpeciesId)
        => new()
        {
            GrassSpeciesId = grassSpeciesId,
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

    public static GrassSpeciesTimingProfile UpdateEntity(
        this GrassSpeciesTimingProfile entity,
        GrassSpeciesTimingProfileUpdateDto dto,
        int grassSpeciesId)
    {
        entity.GrassSpeciesId = grassSpeciesId;
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