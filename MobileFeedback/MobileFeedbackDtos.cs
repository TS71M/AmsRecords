namespace AmsRecords.MobileFeedback;

using Lib.Enums;

public static class MobileFeedbackDtos
{
    public sealed record MobileBugReportSubmitDto(
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("category")] string? Category,
        [property: JsonPropertyName("severity")] string? Severity,
        [property: JsonPropertyName("screenshotBase64")] string? ScreenshotBase64,
        [property: JsonPropertyName("screenshotContentType")] string? ScreenshotContentType,
        [property: JsonPropertyName("screenshotFileName")] string? ScreenshotFileName,
        [property: JsonPropertyName("pageName")] string? PageName,
        [property: JsonPropertyName("appVersion")] string? AppVersion,
        [property: JsonPropertyName("appBuild")] string? AppBuild,
        [property: JsonPropertyName("platform")] string? Platform,
        [property: JsonPropertyName("deviceModel")] string? DeviceModel,
        [property: JsonPropertyName("manufacturer")] string? Manufacturer,
        [property: JsonPropertyName("osVersion")] string? OsVersion,
        [property: JsonPropertyName("idiom")] string? Idiom,
        [property: JsonPropertyName("occurredAtUtc")] DateTime OccurredAtUtc);

    public sealed record MobileBugReportSubmittedDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("submittedAtUtc")] DateTime SubmittedAtUtc);

    public sealed record MobileBugReportStatusUpdateDto(
        [property: JsonPropertyName("status")] MobileBugReportStatus Status);

    public sealed record MobileBugReportStatusCountDto(
        [property: JsonPropertyName("status")] MobileBugReportStatus Status,
        [property: JsonPropertyName("count")] int Count);

    public sealed record MobileBugReportAdminDto(
        [property: JsonPropertyName("bugNumber")] int BugNumber,
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("submittedUtc")] DateTime SubmittedUtc,
        [property: JsonPropertyName("status")] MobileBugReportStatus Status,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("category")] string? Category,
        [property: JsonPropertyName("severity")] string? Severity,
        [property: JsonPropertyName("pageName")] string? PageName,
        [property: JsonPropertyName("appVersion")] string? AppVersion,
        [property: JsonPropertyName("appBuild")] string? AppBuild,
        [property: JsonPropertyName("platform")] string? Platform,
        [property: JsonPropertyName("deviceModel")] string? DeviceModel,
        [property: JsonPropertyName("manufacturer")] string? Manufacturer,
        [property: JsonPropertyName("osVersion")] string? OsVersion,
        [property: JsonPropertyName("idiom")] string? Idiom,
        [property: JsonPropertyName("screenshotFileName")] string? ScreenshotFileName,
        [property: JsonPropertyName("screenshotContentType")] string? ScreenshotContentType,
        [property: JsonPropertyName("screenshotImagePubId")] Guid? ScreenshotImagePubId,
        [property: JsonPropertyName("emailSent")] bool EmailSent,
        [property: JsonPropertyName("emailSentUtc")] DateTime? EmailSentUtc,
        [property: JsonPropertyName("emailError")] string? EmailError,
        [property: JsonPropertyName("automationNotes")] string? AutomationNotes,
        [property: JsonPropertyName("ibuPubId")] Guid? IbuPubId,
        [property: JsonPropertyName("ibuName")] string? IbuName,
        [property: JsonPropertyName("userPubId")] Guid? UserPubId,
        [property: JsonPropertyName("userName")] string? UserName,
        [property: JsonPropertyName("userEmail")] string? UserEmail);
}
