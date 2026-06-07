namespace AmsRecords.Authentication;

public static class AuthenticationDtos
{
    public sealed record RegisterDto(
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("businessUnitName")] string? BusinessUnitName,
        [property: JsonPropertyName("workspaceKind")] WorkspaceKinds.WorkspaceKind WorkspaceKind,
        [property: JsonPropertyName("primaryFieldName")] string? PrimaryFieldName,
        [property: JsonPropertyName("recaptchaToken")] string? RecaptchaToken,
        [property: JsonPropertyName("requestExistingDataClaim")] bool RequestExistingDataClaim = false
    );

    public enum RegisterOutcome
    {
        Registered = 1,
        ClaimAvailable = 2,
        ClaimRequested = 3,
        DuplicateExists = 4
    }

    public sealed record RegisterResultDto(
        [property: JsonPropertyName("outcome")] RegisterOutcome Outcome,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("userPubId")] Guid? UserPubId,
        [property: JsonPropertyName("claimRequestPubId")] Guid? ClaimRequestPubId,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("businessUnitName")] string BusinessUnitName,
        [property: JsonPropertyName("message")] string Message,
        [property: JsonPropertyName("emailConfirmationSent")] bool EmailConfirmationSent,
        [property: JsonPropertyName("requiresPasswordSetup")] bool RequiresPasswordSetup
    );

    public sealed record LoginDto(
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("password")] string? Password
    );

    public sealed record ConfirmEmailDto(
       [property: JsonPropertyName("userId")] int UserId,
       [property: JsonPropertyName("code")] string Code
   );

    public enum PasswordResetEmailJobState
    {
        Queued = 1,
        RetryScheduled = 2,
        Processing = 3,
        Sent = 4,
        Failed = 5,
        Cancelled = 6
    }

    public enum PasswordResetQueueRequestOutcome
    {
        Queued = 1,
        AlreadyQueued = 2
    }

    public sealed record PasswordResetEmailJobStatusDto(
        [property: JsonPropertyName("jobPubId")] Guid JobPubId,
        [property: JsonPropertyName("state")] PasswordResetEmailJobState State,
        [property: JsonPropertyName("requestedUtc")] DateTime RequestedUtc,
        [property: JsonPropertyName("expiresUtc")] DateTime ExpiresUtc,
        [property: JsonPropertyName("nextAttemptUtc")] DateTime? NextAttemptUtc,
        [property: JsonPropertyName("lastAttemptUtc")] DateTime? LastAttemptUtc,
        [property: JsonPropertyName("sentUtc")] DateTime? SentUtc,
        [property: JsonPropertyName("failedUtc")] DateTime? FailedUtc,
        [property: JsonPropertyName("cancelledUtc")] DateTime? CancelledUtc,
        [property: JsonPropertyName("attemptCount")] int AttemptCount,
        [property: JsonPropertyName("canCancel")] bool CanCancel,
        [property: JsonPropertyName("lastError")] string? LastError
    )
    {
        [JsonPropertyName("isActive")]
        public bool IsActive => State is PasswordResetEmailJobState.Queued
            or PasswordResetEmailJobState.RetryScheduled
            or PasswordResetEmailJobState.Processing;
    }

    public sealed record PasswordResetQueueResultDto(
        [property: JsonPropertyName("outcome")] PasswordResetQueueRequestOutcome Outcome,
        [property: JsonPropertyName("job")] PasswordResetEmailJobStatusDto Job
    );

    public sealed record PasswordResetLinkDto(
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("url")] string Url
    );

    public sealed record PasswordResetLinkPayloadDto(
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("env")] string? Env
    );

    public sealed record PasswordResetWithTokenDto(
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("password")] string Password
    );
}
