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
}
