namespace AmsRecords.UserPageVisits;

public static class UserPageVisitDtos
{
    public sealed record UserPageVisitCreateDto(
        [property: JsonPropertyName("app")] string App,
        [property: JsonPropertyName("path")] string Path,
        [property: JsonPropertyName("pageTitle")] string? PageTitle,
        [property: JsonPropertyName("method")] string Method,
        [property: JsonPropertyName("ipAddress")] string? IpAddress,
        [property: JsonPropertyName("userAgent")] string? UserAgent
    );

    public sealed record UserPageVisitDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid? IbuPubId,
        [property: JsonPropertyName("ibuName")] string? IbuName,
        [property: JsonPropertyName("userPubId")] Guid UserPubId,
        [property: JsonPropertyName("userDisplayName")] string? UserDisplayName,
        [property: JsonPropertyName("userName")] string? UserName,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("app")] string App,
        [property: JsonPropertyName("path")] string Path,
        [property: JsonPropertyName("pageTitle")] string? PageTitle,
        [property: JsonPropertyName("method")] string Method,
        [property: JsonPropertyName("ipAddress")] string? IpAddress,
        [property: JsonPropertyName("userAgent")] string? UserAgent,
        [property: JsonPropertyName("visitedUtc")] DateTime VisitedUtc
    );

    public sealed record UserPageVisitQueryDto(
        [property: JsonPropertyName("ibuPubId")] Guid? IbuPubId,
        [property: JsonPropertyName("userPubId")] Guid? UserPubId,
        [property: JsonPropertyName("userName")] string? UserName,
        [property: JsonPropertyName("app")] string? App,
        [property: JsonPropertyName("path")] string? Path,
        [property: JsonPropertyName("fromUtc")] DateTime? FromUtc,
        [property: JsonPropertyName("toUtc")] DateTime? ToUtc,
        [property: JsonPropertyName("take")][Range(1, 5000)] int Take = 200,
        [property: JsonPropertyName("skip")][Range(0, int.MaxValue)] int Skip = 0
    );

    public sealed record UserUsageSummaryDto(
        [property: JsonPropertyName("userPubId")] Guid UserPubId,
        [property: JsonPropertyName("userDisplayName")] string? UserDisplayName,
        [property: JsonPropertyName("userName")] string? UserName,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("ibuPubId")] Guid? IbuPubId,
        [property: JsonPropertyName("ibuName")] string? IbuName,
        [property: JsonPropertyName("lastLoginUtc")] DateTime? LastLoginUtc,
        [property: JsonPropertyName("lastVisitUtc")] DateTime? LastVisitUtc,
        [property: JsonPropertyName("loginCount")] int LoginCount,
        [property: JsonPropertyName("visitCount")] int VisitCount,
        [property: JsonPropertyName("distinctPageCount")] int DistinctPageCount
    );

    public sealed record UserPageUsageDto(
        [property: JsonPropertyName("app")] string App,
        [property: JsonPropertyName("path")] string Path,
        [property: JsonPropertyName("pageTitle")] string? PageTitle,
        [property: JsonPropertyName("visitCount")] int VisitCount,
        [property: JsonPropertyName("lastVisitedUtc")] DateTime LastVisitedUtc
    );

    public sealed record UserUsageAnalysisDto(
        [property: JsonPropertyName("users")] IReadOnlyList<UserUsageSummaryDto> Users,
        [property: JsonPropertyName("pages")] IReadOnlyList<UserPageUsageDto> Pages,
        [property: JsonPropertyName("recentVisits")] IReadOnlyList<UserPageVisitDto> RecentVisits,
        [property: JsonPropertyName("totalVisits")] int TotalVisits
    );

    public sealed record UserPageVisitPurgeDto(
        [property: JsonPropertyName("keepDays")][Range(1, 3650)] int KeepDays = 365
    );

    public sealed record UserPageVisitPurgeResultDto(
        [property: JsonPropertyName("deleted")] int Deleted,
        [property: JsonPropertyName("cutoffUtc")] DateTime CutoffUtc
    );
}
