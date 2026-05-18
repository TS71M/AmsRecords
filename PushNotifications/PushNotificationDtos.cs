namespace AmsRecords.PushNotifications;

public static class PushNotificationDtos
{
    public sealed record RegisterPushDeviceDto(
        [property: JsonPropertyName("provider")] string? Provider,
        [property: JsonPropertyName("platform")] string? Platform,
        [property: JsonPropertyName("token")] string? Token,
        [property: JsonPropertyName("deviceId")] string? DeviceId,
        [property: JsonPropertyName("deviceName")] string? DeviceName,
        [property: JsonPropertyName("appVersion")] string? AppVersion,
        [property: JsonPropertyName("buildNumber")] string? BuildNumber
    );

    public sealed record PushDeviceDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("provider")] string Provider,
        [property: JsonPropertyName("platform")] string Platform,
        [property: JsonPropertyName("deviceName")] string? DeviceName,
        [property: JsonPropertyName("appVersion")] string? AppVersion,
        [property: JsonPropertyName("buildNumber")] string? BuildNumber,
        [property: JsonPropertyName("registeredUtc")] DateTime RegisteredUtc,
        [property: JsonPropertyName("lastSeenUtc")] DateTime LastSeenUtc,
        [property: JsonPropertyName("disabledUtc")] DateTime? DisabledUtc
    );

    public sealed record TestPushNotificationDto(
        [property: JsonPropertyName("userPubId")] Guid? UserPubId,
        [property: JsonPropertyName("title")] string? Title,
        [property: JsonPropertyName("body")] string? Body
    );

    public sealed record PushSendResultDto(
        [property: JsonPropertyName("targetDeviceCount")] int TargetDeviceCount,
        [property: JsonPropertyName("successCount")] int SuccessCount,
        [property: JsonPropertyName("failureCount")] int FailureCount
    );
}
