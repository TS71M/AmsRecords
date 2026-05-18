namespace AmsRecords.Units;

public record UnitValueDto(
    [property: JsonPropertyName("value")] decimal? Value,
    [property: JsonPropertyName("text")] string Text,
    [property: JsonPropertyName("unitShort")] string UnitShort
)
{
    public UnitValueDto() : this(null, string.Empty, string.Empty) { }
};