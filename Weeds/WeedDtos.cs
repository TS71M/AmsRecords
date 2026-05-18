namespace AmsRecords.Weeds;

public static class WeedDtos
{
    public sealed record WeedDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("defaultName")] string DefaultName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("preferredFromGts")] decimal PreferredFromGts,
        [property: JsonPropertyName("preferredToGts")] decimal PreferredToGts,
        [property: JsonPropertyName("acceptableFromGts")] decimal AcceptableFromGts,
        [property: JsonPropertyName("acceptableToGts")] decimal AcceptableToGts,
        [property: JsonPropertyName("managementNote")] string? ManagementNote);

    public sealed record WeedCreateDto(
        [property: JsonPropertyName("code")]
        [Required, MinLength(2), MaxLength(100)]
        string Code,

        [property: JsonPropertyName("defaultName")]
        [Required, MinLength(2), MaxLength(200)]
        string DefaultName,

        [property: JsonPropertyName("active")]
        bool Active,

        [property: JsonPropertyName("sortOrder")]
        int SortOrder,

        [property: JsonPropertyName("preferredFromGts")]
        decimal PreferredFromGts,

        [property: JsonPropertyName("preferredToGts")]
        decimal PreferredToGts,

        [property: JsonPropertyName("acceptableFromGts")]
        decimal AcceptableFromGts,

        [property: JsonPropertyName("acceptableToGts")]
        decimal AcceptableToGts,

        [property: JsonPropertyName("managementNote")]
        [MaxLength(1000)]
        string? ManagementNote);

    public sealed record WeedUpdateDto(
        [property: JsonPropertyName("pubId")]
        [Required]
        Guid PubId,

        [property: JsonPropertyName("code")]
        [Required, MinLength(2), MaxLength(100)]
        string Code,

        [property: JsonPropertyName("defaultName")]
        [Required, MinLength(2), MaxLength(200)]
        string DefaultName,

        [property: JsonPropertyName("active")]
        bool Active,

        [property: JsonPropertyName("sortOrder")]
        int SortOrder,

        [property: JsonPropertyName("preferredFromGts")]
        decimal PreferredFromGts,

        [property: JsonPropertyName("preferredToGts")]
        decimal PreferredToGts,

        [property: JsonPropertyName("acceptableFromGts")]
        decimal AcceptableFromGts,

        [property: JsonPropertyName("acceptableToGts")]
        decimal AcceptableToGts,

        [property: JsonPropertyName("managementNote")]
        [MaxLength(1000)]
        string? ManagementNote);
}