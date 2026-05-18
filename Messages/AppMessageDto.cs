namespace AmsRecords.Messages;

public sealed record AppMessageDto(
[property: JsonPropertyName("code")] string Code,
[property: JsonPropertyName("severity")] string? Severity,
[property: JsonPropertyName("args")] Dictionary<string, string>? Args
);
 