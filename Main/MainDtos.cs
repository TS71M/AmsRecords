namespace AmsRecords.Main;

public static class MainDtos
{
    public record MainDto(
        [property: JsonPropertyName("fieldId")] Guid FieldId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("appImagePubId")] Guid? AppImagePubId
    )
    {
        public MainDto() : this(Guid.Empty, string.Empty, true, null) { }

        public string? AppImageUrl { get; set; }
    };
}