namespace AmsRecords.Languages;

public static class LanguageDtos
{
    public sealed record LanguageDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("languageCode")] string LanguageCode,
        [property: JsonPropertyName("cultureCode")] string CultureCode,
        [property: JsonPropertyName("languageName")] string LanguageName,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record LanguageCreateDto(
        [property: JsonPropertyName("languageCode")][Required, MaxLength(10)] string LanguageCode,
        [property: JsonPropertyName("cultureCode")][Required, MaxLength(20)] string CultureCode,
        [property: JsonPropertyName("languageName")][Required, MaxLength(100)] string LanguageName,
        [property: JsonPropertyName("active")] bool Active
    )
    {
        public LanguageCreateDto() : this(string.Empty, string.Empty, string.Empty, true)
        {
        }
    }

    public sealed record LanguageUpdateDto(
        [property: JsonPropertyName("publicId")][Required] Guid PubId,
        [property: JsonPropertyName("languageCode")][Required, MaxLength(10)] string LanguageCode,
        [property: JsonPropertyName("cultureCode")][Required, MaxLength(20)] string CultureCode,
        [property: JsonPropertyName("languageName")][Required, MaxLength(100)] string LanguageName,
        [property: JsonPropertyName("active")] bool Active
    )
    {
        public LanguageUpdateDto() : this(Guid.Empty, string.Empty, string.Empty, string.Empty, true)
        {
        }
    }

    public sealed record LanguageMiniDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("languageName")] string LanguageName
    );
}