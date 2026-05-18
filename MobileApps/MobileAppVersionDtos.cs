namespace AmsRecords.MobileApps;

public static class MobileAppVersionDtos
{
    public const string PlatformHeaderName = "X-Mobile-Platform";
    public const string VersionHeaderName = "X-Mobile-Version";
    public const string BuildHeaderName = "X-Mobile-Build";

    public static string NormalizePlatform(string? platform)
        => string.Equals(platform, "android", StringComparison.OrdinalIgnoreCase)
            ? "android"
            : "ios";

    public sealed record MobileAppVersionPolicyDto(
        [property: JsonPropertyName("platform")] string Platform,
        [property: JsonPropertyName("minimumBuild")] int MinimumBuild,
        [property: JsonPropertyName("minimumDisplayVersion")] string MinimumDisplayVersion,
        [property: JsonPropertyName("latestBuild")] int LatestBuild,
        [property: JsonPropertyName("latestDisplayVersion")] string LatestDisplayVersion,
        [property: JsonPropertyName("updateUrl")] string? UpdateUrl,
        [property: JsonPropertyName("updateMessage")] string? UpdateMessage,
        [property: JsonPropertyName("isUpdateRequired")] bool IsUpdateRequired
    );
}
