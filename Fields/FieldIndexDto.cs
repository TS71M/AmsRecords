using AmsRecords.Units;
using AmsRecords.Weather;
using System.ComponentModel;
using static AmsRecords.RiskManagement.RiskDtos;

namespace AmsRecords.Fields;

public record FieldIndexDto
{
    [JsonPropertyName("pubId")]
    public Guid FieldPubId { get; init; }

    [JsonPropertyName("ibuPubId")]
    public Guid IbuPubId { get; init; }

    [JsonPropertyName("fieldName"), DisplayName("Name")]
    public string FieldName { get; init; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; init; }

    [JsonPropertyName("fieldTypeTxt"), DisplayName("Field Type")]
    public string FieldTypeTxt { get; init; } = string.Empty;

    [JsonPropertyName("lat")]
    public decimal? Lat { get; init; }

    [JsonPropertyName("lng")]
    public decimal? Lng { get; init; }

    [JsonPropertyName("imagePubId")]
    public Guid? AppImagePubId { get; init; }

    [JsonPropertyName("risk")]
    public RiskMiniDto? Risk { get; init; }

    [JsonPropertyName("currentObservation")]
    public WeatherCurrentMiniDto CurrentObservation { get; init; } = new(Guid.Empty, DateTime.MinValue, new UnitValueDto(), new UnitValueDto(), new UnitValueDto(), null, null, "");

    public string? ImageDataUrl { get; set; }
}
