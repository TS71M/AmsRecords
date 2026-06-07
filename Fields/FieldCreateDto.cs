using System.ComponentModel;
using static AmsRecords.AppImages.AppImageDtos;

namespace AmsRecords.Fields;

public sealed record FieldCreateDto(
    [property: JsonPropertyName("selFieldTypePubId")]
    [property: DisplayName("Field Type")]
    Guid? FieldTypePubId,

    [property: JsonPropertyName("fieldName")]
    [property: DisplayName("Name")]
    string FieldName,

    [property: JsonPropertyName("jurisdictionPubId")]
    Guid? JurisdictionPubId,

    [property: JsonPropertyName("climateZonePubId")]
    [property: DisplayName("Climate Zone")]
    Guid? ClimateZonePubId,

    [property: JsonPropertyName("elevation")]
    [property: DisplayName("Elevation")]
    [param: Range(0, 10000, ErrorMessage = "Elevation must be between 0 and 10000.")]
    decimal Elevation,

    [property: JsonPropertyName("totalSurface")]
    [property: DisplayName("Total surface")]
    decimal TotalSurface,

    [property: JsonPropertyName("numberOfHoles")]
    [property: DisplayName("Number of Holes")]
    int NumberOfHoles,

    [property: JsonPropertyName("yearOfConstruction")]
    [property: DisplayName("Year of Construction")]
    [param: Range(1900, 2100, ErrorMessage = "Year of construction must be between 1900 and 2100.")]
    int? YearOfConstruction,

    [property: JsonPropertyName("yearOfIrrigationSystem")]
    [property: DisplayName("Year of Irrigation System")]
    [param: Range(1900, 2100, ErrorMessage = "Year of irrigation system must be between 1900 and 2100.")]
    int? YearOfIrrigationSystem,

    [property: JsonPropertyName("bacHeaAroGre")]
    [property: DisplayName("Back heads around Greens")]
    bool BacHeaAroGre,

    [property: JsonPropertyName("irrigationSystem")]
    [property: DisplayName("Irrigation System")]
    string IrrigationSystem,

    [property: JsonPropertyName("waterSources")]
    [property: DisplayName("Water Sources")]
    [param: Range(0, 100, ErrorMessage = "Water sources must be between 0 and 100.")]
    int WaterSources,

    [property: JsonPropertyName("weatherStation")]
    [property: DisplayName("Weather Station")]
    bool WeatherStation,

    [property: JsonPropertyName("tolerance")]
    [param: Range(0, 100, ErrorMessage = "Tolerance must be between 0 and 100%.")]
    int Tolerance,

    [property: JsonPropertyName("cutUniLenPubId")]
    [property: DisplayName("Cutting Height unit")]
    Guid? CutUniLenPubId,

    [property: JsonPropertyName("greenSpeedUniLenPubId")]
    [property: DisplayName("Green Speed unit")]
    Guid? GreenSpeedUniLenPubId,

    [property: JsonPropertyName("tempUniPubId")]
    [property: DisplayName("Temperature unit")]
    Guid? TempUniPubId,

    [property: JsonPropertyName("rainUniPubId")]
    [property: DisplayName("Precipitation unit")]
    Guid? RainUniPubId,

    [property: JsonPropertyName("clippingsUniPubId")]
    [property: DisplayName("Clippings unit")]
    Guid? ClippingsUniPubId,

    [property: JsonPropertyName("windUniPubId")]
    [property: DisplayName("Wind unit")]
    Guid? WindUniPubId,

    [property: JsonPropertyName("uniSurPubId")]
    [property: DisplayName("Surface unit")]
    Guid? SurfaceUniPubId,

    [property: JsonPropertyName("totalSurfaceUniPubId")]
    [property: DisplayName("Total surface unit")]
    Guid? TotalSurfaceUniPubId,

    [property: JsonPropertyName("elevationUnitPubId")]
    [property: DisplayName("Elevation unit")]
    Guid? ElevationUnitPubId,

    [property: JsonPropertyName("startInventory")]
    [property: DisplayName("Start Inventory")]
    bool StartInventory,

    [property: JsonPropertyName("usePlanning")]
    [property: DisplayName("Use Planning")]
    bool UsePlanning,

    [property: JsonPropertyName("enterWeatherManually")]
    [property: DisplayName("Enter Weather Data manually")]
    bool EnterWeatherManually,

    [property: JsonPropertyName("dollarSpotMedium")]
    [property: DisplayName("Dollar spot threshold medium")]
    [param: Range(0, 100, ErrorMessage = "Dollar spot threshold medium must be between 0 and 100.")]
    int DollarSpotMedium,

    [property: JsonPropertyName("dollarSpotHigh")]
    [property: DisplayName("Dollar spot threshold high")]
    [param: Range(0, 100, ErrorMessage = "Dollar spot threshold high must be between 0 and 100.")]
    int DollarSpotHigh,

    [property: JsonPropertyName("addressId")]
    Guid? AddressPubId,

    [property: JsonPropertyName("contactId")]
    Guid? ContactPubId,

    [property: JsonPropertyName("architectId")]
    Guid? ArchitectPubId,

    [property: JsonPropertyName("doGId")]
    Guid? DoGPubId,

    [property: JsonPropertyName("stdWorkDay")]
    [property: DisplayName("Standard work day duration")]
    [param: Range(1, 24, ErrorMessage = "Standard work day duration must be between 1 and 24 hours.")]
    int StdWorkDay=9,

    [property: JsonPropertyName("coordinatePubId")]
    Guid? CoordinatePubId=default,

    [property: JsonPropertyName("ibuPubId")]
    Guid IbuPubId=default,

    [property: JsonPropertyName("commonStartTime")]
    [property: DisplayName("Common start time")]
    TimeSpan? CommonStartTime=default,

    [property: JsonPropertyName("primaryRiskAreaPubId")]
    [property: DisplayName("Primary Risk Area")]
    Guid? PrimaryRiskAreaPubId=default,

    [property: JsonPropertyName("growthPotentialAreaPubId")]
    [property: DisplayName("Growth Potential Area")]
    Guid? GrowthPotentialAreaPubId=default
)
{
    public AppImageCreateDto? ImageCreateDto { get; set; } = null;
}

public sealed record FieldAiSeedDto(
    [property: JsonPropertyName("ibuPubId")]
    Guid IbuPubId,

    [property: JsonPropertyName("fieldName")]
    string FieldName,

    [property: JsonPropertyName("fieldWebsite")]
    string? FieldWebsite,

    [property: JsonPropertyName("fieldTypePubId")]
    Guid? FieldTypePubId,

    [property: JsonPropertyName("countryPubId")]
    Guid? CountryPubId,

    [property: JsonPropertyName("jurisdictionPubId")]
    Guid? JurisdictionPubId
);
