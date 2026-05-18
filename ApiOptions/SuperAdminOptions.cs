namespace AmsRecords.ApiOptions;

public record SuperAdminOptions(
    [property: JsonPropertyName("firstName")] string SuperAdminFirstName,
    [property: JsonPropertyName("lastName")] string SuperAdminLastName,
    [property: JsonPropertyName("email")] string SuperAdminEmail,
    [property: JsonPropertyName("userName")] string SuperAdminUserName,
    [property: JsonPropertyName("tempPassword")] string SuperAdminTempPassword
    );