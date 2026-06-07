using System.ComponentModel;
using static AmsRecords.AppImages.AppImageDtos;

namespace AmsRecords.Fields;

public sealed record FieldUpdateDto(
    [property: JsonPropertyName("pubId")]
    Guid PubId = default,

    [property: JsonPropertyName("active")]
    bool Active = true,

    [property: JsonPropertyName("selFieldTypePubId")]
    [property: DisplayName("Field Type")]
    Guid? FieldTypePubId = null,

    [property: JsonPropertyName("fieldName")]
    [property: DisplayName("Name")]
    string FieldName = "",

    [property: JsonPropertyName("jurisdictionPubId")]
    Guid? JurisdictionPubId = null,

    [property: JsonPropertyName("climateZonePubId")]
    [property: DisplayName("Climate Zone")]
    Guid? ClimateZonePubId = null,

    [property: JsonPropertyName("elevation")]
    [property: DisplayName("Elevation")]
    [param: Range(0, 10000, ErrorMessage = "Elevation must be between 0 and 10000.")]
    decimal Elevation = 0,

    [property: JsonPropertyName("totalSurface")]
    [property: DisplayName("Total surface")]
    decimal TotalSurface = 0,

    [property: JsonPropertyName("numberOfHoles")]
    [property: DisplayName("Number of Holes")]
    int NumberOfHoles = 18,

    [property: JsonPropertyName("yearOfConstruction")]
    [property: DisplayName("Year of Construction")]
    [param: Range(1900, 2100, ErrorMessage = "Year of construction must be between 1900 and 2100.")]
    int? YearOfConstruction = 1980,

    [property: JsonPropertyName("yearOfIrrigationSystem")]
    [property: DisplayName("Year of Irrigation System")]
    [param: Range(1900, 2100, ErrorMessage = "Year of irrigation system must be between 1900 and 2100.")]
    int? YearOfIrrigationSystem = 1980,

    [property: JsonPropertyName("bacHeaAroGre")]
    [property: DisplayName("Back heads around Greens")]
    bool BacHeaAroGre = false,

    [property: JsonPropertyName("irrigationSystem")]
    [property: DisplayName("Irrigation System")]
    string IrrigationSystem = "",

    [property: JsonPropertyName("waterSources")]
    [property: DisplayName("Water Sources")]
    [param: Range(0, 100, ErrorMessage = "Water sources must be between 0 and 100.")]
    int WaterSources = 1,

    [property: JsonPropertyName("weatherStation")]
    [property: DisplayName("Weather Station")]
    bool WeatherStation = false,

    [property: JsonPropertyName("tolerance")]
    [param: Range(0, 100, ErrorMessage = "Tolerance must be between 0 and 100%.")]
    int Tolerance = 0,

    [property: JsonPropertyName("cutUniLenPubId")]
    [property: DisplayName("Cutting Height unit")]
    Guid? CutUniLenPubId = null,

    [property: JsonPropertyName("greenSpeedUniLenPubId")]
    [property: DisplayName("Green Speed unit")]
    Guid? GreenSpeedUniLenPubId = null,

    [property: JsonPropertyName("tempUniPubId")]
    [property: DisplayName("Temperature unit")]
    Guid? TempUniPubId = null,

    [property: JsonPropertyName("rainUniPubId")]
    [property: DisplayName("Precipitation unit")]
    Guid? RainUniPubId = null,

    [property: JsonPropertyName("clippingsUniPubId")]
    [property: DisplayName("Clippings unit")]
    Guid? ClippingsUniPubId = null,

    [property: JsonPropertyName("windUniPubId")]
    [property: DisplayName("Wind unit")]
    Guid? WindUniPubId = null,

    [property: JsonPropertyName("uniSurPubId")]
    [property: DisplayName("Surface unit")]
    Guid? SurfaceUniPubId = null,

    [property: JsonPropertyName("totalSurfaceUniPubId")]
    [property: DisplayName("Total surface unit")]
    Guid? TotalSurfaceUniPubId = null,

    [property: JsonPropertyName("elevationUnitPubId")]
    [property: DisplayName("Elevation unit")]
    Guid? ElevationUnitPubId = null,

    [property: JsonPropertyName("startInventory")]
    [property: DisplayName("Start Inventory")]
    bool StartInventory = false,

    [property: JsonPropertyName("usePlanning")]
    [property: DisplayName("Use Planning")]
    bool UsePlanning = true,

    [property: JsonPropertyName("enterWeatherManually")]
    [property: DisplayName("Enter Weather Data manually")]
    bool EnterWeatherManually = false,

    [property: JsonPropertyName("dollarSpotMedium")]
    [property: DisplayName("Dollar spot threshold medium")]
    [param: Range(0, 100, ErrorMessage = "Dollar spot threshold medium must be between 0 and 100.")]
    int DollarSpotMedium = 20,

    [property: JsonPropertyName("dollarSpotHigh")]
    [property: DisplayName("Dollar spot threshold high")]
    [param: Range(0, 100, ErrorMessage = "Dollar spot threshold high must be between 0 and 100.")]
    int DollarSpotHigh = 30,

    [property: JsonPropertyName("addressId")]
    Guid? AddressPubId = null,

    [property: JsonPropertyName("contactId")]
    Guid? ContactPubId = null,

    [property: JsonPropertyName("architectId")]
    Guid? ArchitectPubId = null,

    [property: JsonPropertyName("doGId")]
    Guid? DoGPubId = null,

    [property: JsonPropertyName("stdWorkDay")]
    [property: DisplayName("Standard work day duration")]
    [param: Range(1, 24, ErrorMessage = "Standard work day duration must be between 1 and 24 hours.")]
    int StdWorkDay = 8,

    [property: JsonPropertyName("coordinatePubId")]
    Guid? CoordinatePubId = null,

    [property: JsonPropertyName("commonStartTime")]
    [property: DisplayName("Common start time")]
    TimeSpan? CommonStartTime = null,

    [property: JsonPropertyName("primaryRiskAreaPubId")]
    [property: DisplayName("Primary Risk Area")]
    Guid? PrimaryRiskAreaPubId = null,

    [property: JsonPropertyName("growthPotentialAreaPubId")]
    [property: DisplayName("Growth Potential Area")]
    Guid? GrowthPotentialAreaPubId = null,

    [property: JsonPropertyName("appImagePubId")]
    Guid? AppImagePubId = null
)
{
    public Guid IbuPubId { get; set; } = default;
    [JsonIgnore]
    public Guid? CountryPubId { get; set; } = null;
    [JsonIgnore]
    public string? FieldWebsite { get; set; } = null;
    public AppImageCreateDto? ImageCreateDto { get; set; } = null;
    public string? ImageDataUrl { get; set; } = null;

};
