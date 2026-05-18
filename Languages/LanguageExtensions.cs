using static AmsRecords.Languages.LanguageDtos;

namespace AmsRecords.Languages;

public static class LanguageExtensions
{
    public static LanguageDto ToDto(this Language entity)
        => new(
            PubId: entity.PubId,
            LanguageCode: entity.LanguageCode,
            CultureCode: entity.CultureCode,
            LanguageName: entity.LanguageName,
            Active: entity.Active
        );

    public static LanguageMiniDto ToMiniDto(this Language entity)
        => new(
            PubId: entity.PubId,
            LanguageName: entity.LanguageName
        );

    public static Language ToEntity(this LanguageCreateDto dto)
        => new()
        {
            LanguageCode = dto.LanguageCode.Trim(),
            CultureCode = dto.CultureCode.Trim(),
            LanguageName = dto.LanguageName.Trim(),
            Active = dto.Active
        };

    public static void UpdateFromDto(this Language entity, LanguageUpdateDto dto)
    {
        entity.LanguageCode = dto.LanguageCode.Trim();
        entity.CultureCode = dto.CultureCode.Trim();
        entity.LanguageName = dto.LanguageName.Trim();
        entity.Active = dto.Active;
    }
}