namespace AmsRecords.Chemicals;

public static class ProtectedDefaultsDtos
{
    public sealed record ApplyProtectedDefaultsDto(
        [property: JsonPropertyName("validFromUtc")] DateTimeOffset? ValidFromUtc,
        [property: JsonPropertyName("validToUtc")] DateTimeOffset? ValidToUtc,
        [property: JsonPropertyName("reason")] string? Reason
    );
}
