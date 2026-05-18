namespace AmsRecords.GrassSpeciesTimingProfiles;

public static class GrassSpeciesTimingProfileDtos
{
    public sealed record GrassSpeciesTimingProfileDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("grassSpeciesPubId")] Guid GrassSpeciesPubId,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("defaultName")] string DefaultName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("preferredFromGts")] decimal PreferredFromGts,
        [property: JsonPropertyName("preferredToGts")] decimal PreferredToGts,
        [property: JsonPropertyName("acceptableFromGts")] decimal AcceptableFromGts,
        [property: JsonPropertyName("acceptableToGts")] decimal AcceptableToGts,
        [property: JsonPropertyName("managementNote")] string? ManagementNote);

    public sealed record GrassSpeciesTimingProfileCreateDto(
        [property: JsonPropertyName("grassSpeciesPubId")][param: Required] Guid GrassSpeciesPubId,
        [property: JsonPropertyName("code")][param: Required, MinLength(2), MaxLength(100)] string Code,
        [property: JsonPropertyName("defaultName")][param: Required, MinLength(2), MaxLength(200)] string DefaultName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("preferredFromGts")] decimal PreferredFromGts,
        [property: JsonPropertyName("preferredToGts")] decimal PreferredToGts,
        [property: JsonPropertyName("acceptableFromGts")] decimal AcceptableFromGts,
        [property: JsonPropertyName("acceptableToGts")] decimal AcceptableToGts,
        [property: JsonPropertyName("managementNote")][param: MaxLength(1000)] string? ManagementNote);

    public sealed record GrassSpeciesTimingProfileUpdateDto(
        [property: JsonPropertyName("pubId")][param: Required] Guid PubId,
        [property: JsonPropertyName("grassSpeciesPubId")][param: Required] Guid GrassSpeciesPubId,
        [property: JsonPropertyName("code")][param: Required, MinLength(2), MaxLength(100)] string Code,
        [property: JsonPropertyName("defaultName")][param: Required, MinLength(2), MaxLength(200)] string DefaultName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("preferredFromGts")] decimal PreferredFromGts,
        [property: JsonPropertyName("preferredToGts")] decimal PreferredToGts,
        [property: JsonPropertyName("acceptableFromGts")] decimal AcceptableFromGts,
        [property: JsonPropertyName("acceptableToGts")] decimal AcceptableToGts,
        [property: JsonPropertyName("managementNote")][param: MaxLength(1000)] string? ManagementNote);
}