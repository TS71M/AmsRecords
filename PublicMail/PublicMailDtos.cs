namespace AmsRecords.PublicMail;

public static class PublicMailDtos
{
    public sealed record RequestAccessDto(
        [property: Required, StringLength(120), JsonPropertyName("name")] string Name,
        [property: Required, EmailAddress, StringLength(200), JsonPropertyName("email")] string Email,
        [property: Required, StringLength(160), JsonPropertyName("organization")] string Organization,
        [property: StringLength(120), JsonPropertyName("role")] string Role,
        [property: Required, StringLength(120), JsonPropertyName("interest")] string Interest,
        [property: StringLength(1200), JsonPropertyName("message")] string Message
    );

    public sealed record RequestAccessTestDto(
        [property: Required, EmailAddress, StringLength(200), JsonPropertyName("recipientEmail")] string RecipientEmail
    );
}
