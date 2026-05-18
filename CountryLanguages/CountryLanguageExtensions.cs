using static AmsRecords.CountryLanguages.CountryLanguageDtos;

namespace AmsRecords.CountryLanguages;

public static class CountryLanguageExtensions
{
    public static CountryLanguageDto ToDto(this CountryLanguage entity)
        => new(
            PubId: entity.PubId,
            CountryPubId: entity.Country?.PubId ?? Guid.Empty,
            CountryCode: entity.Country?.CountryCode ?? string.Empty,
            CountryName: entity.Country?.CountryName ?? string.Empty,
            LanguagePubId: entity.Language?.PubId ?? Guid.Empty,
            LanguageCode: entity.Language?.LanguageCode ?? string.Empty,
            CultureCode: entity.Language?.CultureCode ?? string.Empty,
            LanguageName: entity.Language?.LanguageName ?? string.Empty,
            IsDefault: entity.IsDefault,
            Active: entity.Active,
            SortOrder: entity.SortOrder
        );

    public static CountryLanguageMiniDto ToMiniDto(this CountryLanguage entity)
        => new(
            PubId: entity.PubId,
            CountryPubId: entity.Country?.PubId ?? Guid.Empty,
            CountryName: entity.Country?.CountryName ?? string.Empty,
            LanguagePubId: entity.Language?.PubId ?? Guid.Empty,
            LanguageName: entity.Language?.LanguageName ?? string.Empty,
            IsDefault: entity.IsDefault
        );

    public static CountryLanguage ToEntity(this CountryLanguageCreateDto dto, int countryId, int languageId)
        => new()
        {
            CountryId = countryId,
            LanguageId = languageId,
            IsDefault = dto.IsDefault,
            Active = dto.Active,
            SortOrder = dto.SortOrder
        };

    public static void UpdateFromDto(this CountryLanguage entity, CountryLanguageUpdateDto dto, int countryId, int languageId)
    {
        entity.CountryId = countryId;
        entity.LanguageId = languageId;
        entity.IsDefault = dto.IsDefault;
        entity.Active = dto.Active;
        entity.SortOrder = dto.SortOrder;
    }
}