namespace AmsRecords.Weeds;

public static class FieldWeedTimingOverrideDtos
{
    public sealed record FieldWeedTimingOverrideDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("weedPubId")] Guid WeedPubId,
        [property: JsonPropertyName("weedCode")] string WeedCode,
        [property: JsonPropertyName("weedName")] string WeedName,
        [property: JsonPropertyName("preferredFromGts")] decimal? PreferredFromGts,
        [property: JsonPropertyName("preferredToGts")] decimal? PreferredToGts,
        [property: JsonPropertyName("acceptableFromGts")] decimal? AcceptableFromGts,
        [property: JsonPropertyName("acceptableToGts")] decimal? AcceptableToGts,
        [property: JsonPropertyName("note")] string? Note,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record FieldWeedTimingOverrideCreateDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("weedPubId")] Guid WeedPubId,
        [property: JsonPropertyName("preferredFromGts")] decimal? PreferredFromGts,
        [property: JsonPropertyName("preferredToGts")] decimal? PreferredToGts,
        [property: JsonPropertyName("acceptableFromGts")] decimal? AcceptableFromGts,
        [property: JsonPropertyName("acceptableToGts")] decimal? AcceptableToGts,
        [property: JsonPropertyName("note")] string? Note,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record FieldWeedTimingOverrideUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("weedPubId")] Guid WeedPubId,
        [property: JsonPropertyName("preferredFromGts")] decimal? PreferredFromGts,
        [property: JsonPropertyName("preferredToGts")] decimal? PreferredToGts,
        [property: JsonPropertyName("acceptableFromGts")] decimal? AcceptableFromGts,
        [property: JsonPropertyName("acceptableToGts")] decimal? AcceptableToGts,
        [property: JsonPropertyName("note")] string? Note,
        [property: JsonPropertyName("active")] bool Active
    );
}