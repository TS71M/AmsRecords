namespace AmsRecords.ApiOptions;

public record PublicMailOptions(
    string SmtpHost,
    int SmtpPort,
    bool EnableSsl,
    string UserName,
    string Password,
    string FromAddress,
    string FromName,
    string AccessRequestInboxEmail,
    string? ProxyToken,
    string? AllowedSourceBaseUrl)
{
    public bool HasPassword => !string.IsNullOrWhiteSpace(Password);
}
