using System.ComponentModel;
using static AmsRecords.AppImages.AppImageDtos;

namespace AmsRecords.Fields;

public sealed record FieldDto
{
    [JsonPropertyName("selFieldTypePubId")]
    [DisplayName("Field Type")]
    public Guid? FieldTypePubId { get; init; }

    [JsonPropertyName("fieldName")]
    [DisplayName("Name")]
    public string FieldName { get; init; } = string.Empty;

    [JsonPropertyName("jurisdictionPubId")]
    public Guid? JurisdictionPubId { get; init; }

    [JsonPropertyName("climateZonePubId")]
    [DisplayName("Climate Zone")]
    public Guid? ClimateZonePubId { get; init; }

    [JsonPropertyName("elevation")]
    [DisplayName("Elevation")]
    [Range(0, 10000, ErrorMessage = "Elevation must be between 0 and 10000.")]
    public decimal Elevation { get; init; }

    [JsonPropertyName("totalSurface")]
    [DisplayName("Total surface")]
    public decimal TotalSurface { get; init; }

    [JsonPropertyName("numberOfHoles")]
    [DisplayName("Number of Holes")]
    public int NumberOfHoles { get; init; } = 18;

    [JsonPropertyName("yearOfConstruction")]
    [DisplayName("Year of Construction")]
    [Range(1900, 2100, ErrorMessage = "Year of construction must be between 1900 and 2100.")]
    public int? YearOfConstruction { get; init; }

    [JsonPropertyName("yearOfIrrigationSystem")]
    [DisplayName("Year of Irrigation System")]
    [Range(1900, 2100, ErrorMessage = "Year of irrigation system must be between 1900 and 2100.")]
    public int? YearOfIrrigationSystem { get; init; }

    [JsonPropertyName("bacHeaAroGre")]
    [DisplayName("Back heads around Greens")]
    public bool BacHeaAroGre { get; init; }

    [JsonPropertyName("irrigationSystem")]
    [DisplayName("Irrigation System")]
    public string IrrigationSystem { get; init; } = string.Empty;

    [JsonPropertyName("waterSources")]
    [DisplayName("Water Sources")]
    [Range(0, 100, ErrorMessage = "Water sources must be between 0 and 100.")]
    public int WaterSources { get; init; } = 1;

    [JsonPropertyName("weatherStation")]
    [DisplayName("Weather Station")]
    public bool WeatherStation { get; init; }

    [JsonPropertyName("tolerance")]
    [Range(0, 100, ErrorMessage = "Tolerance must be between 0 and 100%.")]
    public int Tolerance { get; init; }

    [JsonPropertyName("cutUniLenPubId")]
    [DisplayName("Cutting Height unit")]
    public Guid? CutUniLenPubId { get; init; }

    [JsonPropertyName("greenSpeedUniLenPubId")]
    [DisplayName("Green Speed unit")]
    public Guid? GreenSpeedUniLenPubId { get; init; }

    [JsonPropertyName("tempUniPubId")]
    [DisplayName("Temperature unit")]
    public Guid? TempUniPubId { get; init; }

    [JsonPropertyName("rainUniPubId")]
    [DisplayName("Precipitation unit")]
    public Guid? RainUniPubId { get; init; }

    [JsonPropertyName("clippingsUniPubId")]
    [DisplayName("Clippings unit")]
    public Guid? ClippingsUniPubId { get; init; }

    [JsonPropertyName("windUniPubId")]
    [DisplayName("Wind unit")]
    public Guid? WindUniPubId { get; init; }

    [JsonPropertyName("uniSurPubId")]
    [DisplayName("Surface unit")]
    public Guid? SurfaceUniPubId { get; init; }

    [JsonPropertyName("totalSurfaceUniPubId")]
    [DisplayName("Total surface unit")]
    public Guid? TotalSurfaceUniPubId { get; init; }

    [JsonPropertyName("elevationUnitPubId")]
    [DisplayName("Elevation unit")]
    public Guid? ElevationUnitPubId { get; init; }

    [JsonPropertyName("startInventory")]
    [DisplayName("Start Inventory")]
    public bool StartInventory { get; init; }

    [JsonPropertyName("usePlanning")]
    [DisplayName("Use Planning")]
    public bool UsePlanning { get; init; }

    [JsonPropertyName("enterWeatherManually")]
    [DisplayName("Enter Weather Data manually")]
    public bool EnterWeatherManually { get; init; }

    [JsonPropertyName("dollarSpotMedium")]
    [DisplayName("Dollar spot threshold medium")]
    [Range(0, 100, ErrorMessage = "Dollar spot threshold medium must be between 0 and 100.")]
    public int DollarSpotMedium { get; init; } = 20;

    [JsonPropertyName("dollarSpotHigh")]
    [DisplayName("Dollar spot threshold high")]
    [Range(0, 100, ErrorMessage = "Dollar spot threshold high must be between 0 and 100.")]
    public int DollarSpotHigh { get; init; } = 30;

    [JsonPropertyName("addressId")]
    public Guid? AddressPubId { get; init; }

    [JsonPropertyName("contactId")]
    public Guid? ContactPubId { get; init; }

    [JsonPropertyName("architectId")]
    public Guid? ArchitectPubId { get; init; }

    [JsonPropertyName("doGId")]
    public Guid? DoGPubId { get; init; }

    [JsonPropertyName("stdWorkDay")]
    [DisplayName("Standard work day duration")]
    [Range(1, 24, ErrorMessage = "Standard work day duration must be between 1 and 24 hours.")]
    public int StdWorkDay { get; init; } = 8;

    [JsonPropertyName("coordinatePubId")]
    public Guid? CoordinatePubId { get; init; }

    [JsonPropertyName("lat")]
    public decimal? Lat { get; init; }

    [JsonPropertyName("lng")]
    public decimal? Lng { get; init; }

    [JsonPropertyName("ibuPubId")]
    public Guid IbuPubId { get; init; }

    [JsonPropertyName("commonStartTime")]
    [DisplayName("Common start time")]
    public TimeSpan? CommonStartTime { get; init; }

    public AppImageCreateDto? ImageCreateDto { get; init; }

    // --- extra fields for FieldDto ---
    [JsonPropertyName("pubId")]
    public Guid PubId { get; init; }

    [JsonPropertyName("jurisdiction")]
    [DisplayName("Jurisdiction")]
    public string JurisdictionName { get; init; } = string.Empty;

    [JsonPropertyName("climateZoneCode")]
    public string? ClimateZoneCode { get; init; }

    [JsonPropertyName("climateZoneName")]
    [DisplayName("Climate Zone")]
    public string? ClimateZoneName { get; init; }

    [JsonPropertyName("primaryRiskAreaPubId")]
    [DisplayName("Primary Risk Area")]
    public Guid? PrimaryRiskAreaPubId { get; init; }

    [JsonPropertyName("primaryRiskAreaName")]
    public string? PrimaryRiskAreaName { get; init; }

    [JsonPropertyName("active")]
    public bool Active { get; init; }

    [JsonPropertyName("fieldTypeTxt")]
    [DisplayName("Field Type")]
    public string FieldTypeTxt { get; init; } = string.Empty;

    [JsonPropertyName("elevationTxt")]
    public string ElevationTxt { get; init; } = string.Empty;

    [JsonPropertyName("totalSurfaceTxt")]
    public string TotalSurfaceTxt { get; init; } = string.Empty;

    [JsonPropertyName("selSurUniTxt")]
    [DisplayName("Surface unit")]
    public string? SelSurUniTxt { get; init; }

    [JsonPropertyName("totalSurfaceUniTxt")]
    [DisplayName("Total surface unit")]
    public string? TotalSurfaceUniTxt { get; init; }

    [JsonPropertyName("elevationUnitTxt")]
    [DisplayName("Elevation unit")]
    public string? ElevationUnitTxt { get; init; }

    [JsonPropertyName("cutUniLenTxt")]
    [DisplayName("Cutting Height unit")]
    public string? CutUniLenTxt { get; init; }

    [JsonPropertyName("greenSpeedUniLenTxt")]
    [DisplayName("Green Speed unit")]
    public string? GreenSpeedUniLenTxt { get; init; }

    [JsonPropertyName("tempUniTxt")]
    [DisplayName("Temperature unit")]
    public string? TempUniTxt { get; init; }

    [JsonPropertyName("rainUniTxt")]
    [DisplayName("Precipitation unit")]
    public string? RainUniTxt { get; init; }

    [JsonPropertyName("clippingsUniTxt")]
    [DisplayName("Clippings unit")]
    public string? ClippingsUniTxt { get; init; }

    [JsonPropertyName("windUniTxt")]
    [DisplayName("Wind unit")]
    public string? WindUniTxt { get; init; }

    [JsonPropertyName("uniSurTxt")]
    [DisplayName("Surface unit")]
    public string? UniSurTxt { get; init; }

    [JsonPropertyName("addressTxt")]
    public string? AddressTxt { get; init; }

    [JsonPropertyName("imagePubId")]
    public Guid? AppImagePubId { get; init; }

    // UI-only
    public string? AppImageUrl { get; set; }
}
