using AmsRecords.Units;

namespace AmsRecords.Weather;

public sealed record WeatherCurrentMiniDto(
    [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
    [property: JsonPropertyName("observedAtUtc")] DateTime ObservedAtUtc,
    [property: JsonPropertyName("temp")] UnitValueDto Temp,
    [property: JsonPropertyName("dewPoint")] UnitValueDto DewPoint,
    [property: JsonPropertyName("relativeHumidity")] UnitValueDto RelativeHumidity,
    [property: JsonPropertyName("weatherMain")] string? WeatherMain,
    [property: JsonPropertyName("weatherCode")] int? WeatherCode,
    [property: JsonPropertyName("iconKey")] string IconKey)
{
    [JsonPropertyName("visualizationTemp")]
    public UnitValueDto VisualizationTemp { get; init; } = new();

    [JsonPropertyName("visualizationDewPoint")]
    public UnitValueDto VisualizationDewPoint { get; init; } = new();

    [JsonPropertyName("tempComparableValue")]
    public decimal? TempComparableValue { get; init; }

    [JsonPropertyName("dewPointComparableValue")]
    public decimal? DewPointComparableValue { get; init; }

    public WeatherCurrentMiniDto() : this(Guid.Empty, DateTime.MinValue, new UnitValueDto(), new UnitValueDto(), new UnitValueDto(), null, null, string.Empty) { }
}
