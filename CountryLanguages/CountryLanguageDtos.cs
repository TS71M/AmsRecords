namespace AmsRecords.CountryLanguages;

public static class CountryLanguageDtos
{
    public sealed record CountryLanguageDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("countryPubId")] Guid CountryPubId,
        [property: JsonPropertyName("countryCode")] string CountryCode,
        [property: JsonPropertyName("countryName")] string CountryName,
        [property: JsonPropertyName("languagePubId")] Guid LanguagePubId,
        [property: JsonPropertyName("languageCode")] string LanguageCode,
        [property: JsonPropertyName("cultureCode")] string CultureCode,
        [property: JsonPropertyName("languageName")] string LanguageName,
        [property: JsonPropertyName("isDefault")] bool IsDefault,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("sortOrder")] int SortOrder
    );

    public sealed record CountryLanguageCreateDto(
        [property: JsonPropertyName("countryPubId")][Required] Guid CountryPubId,
        [property: JsonPropertyName("languagePubId")][Required] Guid LanguagePubId,
        [property: JsonPropertyName("isDefault")] bool IsDefault,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("sortOrder")] int SortOrder
    )
    {
        public CountryLanguageCreateDto() : this(Guid.Empty, Guid.Empty, false, true, 0)
        {
        }
    }

    public sealed record CountryLanguageUpdateDto(
        [property: JsonPropertyName("publicId")][Required] Guid PubId,
        [property: JsonPropertyName("countryPubId")][Required] Guid CountryPubId,
        [property: JsonPropertyName("languagePubId")][Required] Guid LanguagePubId,
        [property: JsonPropertyName("isDefault")] bool IsDefault,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("sortOrder")] int SortOrder
    )
    {
        public CountryLanguageUpdateDto() : this(Guid.Empty, Guid.Empty, Guid.Empty, false, true, 0)
        {
        }
    }

    public sealed record CountryLanguageMiniDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("countryPubId")] Guid CountryPubId,
        [property: JsonPropertyName("countryName")] string CountryName,
        [property: JsonPropertyName("languagePubId")] Guid LanguagePubId,
        [property: JsonPropertyName("languageName")] string LanguageName,
        [property: JsonPropertyName("isDefault")] bool IsDefault
    );
}