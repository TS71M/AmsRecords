namespace AmsRecords.LoginLogs;

public static class LoginLogDtos
{
    public sealed record LoginLogDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid? IbuPubId,
        [property: JsonPropertyName("ibuName")] string? IbuName,
        [property: JsonPropertyName("userPubId")] Guid? UserPubId,
        [property: JsonPropertyName("userDisplayName")] string? UserDisplayName,
        [property: JsonPropertyName("occuredUtc")] DateTime OccuredUtc,
        [property: JsonPropertyName("succeeded")] bool Succeeded,
        [property: JsonPropertyName("failureCode")] string? FailureCode,
        [property: JsonPropertyName("ipAddress")] string? IpAddress,
        [property: JsonPropertyName("userAgent")] string? UserAgent
    );

    public sealed record LoginLogQueryDto(
        [property: JsonPropertyName("ibuPubId")] Guid? IbuPubId,
        [property: JsonPropertyName("userPubId")] Guid? UserPubId,
        [property: JsonPropertyName("fromUtc")] DateTime? FromUtc,
        [property: JsonPropertyName("toUtc")] DateTime? ToUtc,
        [property: JsonPropertyName("succeeded")] bool? Succeeded,
        [property: JsonPropertyName("userName")] string? UserName = null,
        [property: JsonPropertyName("take")][Range(1, 5000)] int Take = 200,
        [property: JsonPropertyName("skip")][Range(0, int.MaxValue)] int Skip = 0
    );

    public sealed record LoginLogPurgeDto(
        [property: JsonPropertyName("keepDays")][Range(1, 3650)] int KeepDays = 180
    );

    public sealed record PurgeResultDto(
        [property: JsonPropertyName("deleted")] int Deleted,
        [property: JsonPropertyName("cutoffUtc")] DateTime CutoffUtc
    );
}
