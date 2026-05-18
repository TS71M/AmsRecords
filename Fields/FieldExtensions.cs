namespace AmsRecords.Fields;

public static class FieldDtoExtensions
{
    public static FieldDto ToDto(this Field field)
        => new()
        {
            PubId = field.PubId,
            FieldName = field.FieldName,
            FieldTypeTxt = field.FieldTypeTxt,
            Active = field.Active,
            Elevation = field.Elevation,
            ElevationTxt = field.ElevationTxt,
            NumberOfHoles = field.NumberOfHoles,
            YearOfConstruction = field.YearOfConstruction,
            YearOfIrrigationSystem = field.YearOfIrrigationSystem,
            BacHeaAroGre = field.BacHeaAroGre,
            IrrigationSystem = field.IrrigationSystem,
            WaterSources = field.WaterSources,
            WeatherStation = field.WeatherStation,
            Tolerance = field.Tolerance,
            SurfaceUniPubId = field.SurfaceUni?.PubId,
            SelSurUniTxt = field.TotalSurfaceUnit,
            CutUniLenPubId = field.CutUniLen?.PubId,
            CutUniLenTxt = field.CuttingsUnit,
            GreenSpeedUniLenPubId = field.GreenSpeedUniLen?.PubId,
            GreenSpeedUniLenTxt = field.GreenSpeedUnit,
            TempUniPubId = field.TempUni?.PubId,
            TempUniTxt = field.TemperatureUnit,
            RainUniPubId = field.RainUni?.PubId,
            RainUniTxt = field.RainUnit,
            ClippingsUniPubId = field.ClippingsUni?.PubId,
            ClippingsUniTxt = field.ClippingsUnit,
            WindUniPubId = field.WindUni?.PubId,
            WindUniTxt = field.WindSpeedUnit,
            UniSurTxt = field.SurfaceUnit,
            TotalSurfaceUniPubId = field.TotalSurfaceUni?.PubId,
            ElevationUnitPubId = field.ElevationUniLen?.PubId,
            StartInventory = field.StartInventory,
            UsePlanning = field.UsePlanning,
            EnterWeatherManually = field.EnterWeatherManually,
            DollarSpotMedium = field.DollarSpotMedium,
            DollarSpotHigh = field.DollarSpotHigh,
            AddressPubId = field.Address?.PubId,
            AddressTxt = field.FieldAddress,
            ContactPubId = field.Contact?.PubId,
            ArchitectPubId = field.Architect?.PubId,
            DoGPubId = field.DoG?.PubId,
            AppImagePubId = field.AppImagePubId,
            StdWorkDay = field.StdWorkDay,
            CoordinatePubId = field.CoordinatePubId,
            Lat = field.Coordinate?.Latitude,
            Lng = field.Coordinate?.Longitude,
            CommonStartTime = field.CommonStartTime,
            IbuPubId = field.Ibu.PubId,
            FieldTypePubId = field.FieldType?.PubId,
            JurisdictionName = field.Jurisdiction?.Name ?? string.Empty,
            JurisdictionPubId = field.Jurisdiction?.PubId,
            ClimateZonePubId = field.ClimateZone?.PubId,
            ClimateZoneCode = field.ClimateZone?.ZoneCode,
            ClimateZoneName = field.ClimateZone?.ZoneName,
            PrimaryRiskAreaPubId = field.PrimaryRiskArea?.PubId,
            PrimaryRiskAreaName = field.PrimaryRiskArea?.AreaName,
            TotalSurface = field.TotalSurface,
            TotalSurfaceTxt = field.TotalSurfaceTxt,
        };

    public static FieldMapDto ToMapDto(this Field field)
        => new(
            field.PubId,
            field.FieldName,
            field.FieldType?.FieldTypeName ?? string.Empty,
            field.FieldType?.MarkerColor,
            field.FieldType?.MarkerIconUrl,
            field.Coordinate?.Latitude,
            field.Coordinate?.Longitude,
            field.Active
            );

    public static Field ToEntity(
        this FieldCreateDto dto,
        FieldType fieldType,
        Ibu ibu,
        Address? address,
        Coordinate? coordinate,
        ContactDetail? contactDetail,
        Architect? architect,
        DirectorOfGolf? directorOfGolf,
        Jurisdiction? jurisdiction,
        ClimateZone? climateZone)
        => new()
        {
            FieldName = dto.FieldName,
            FieldTypeId = fieldType?.FieldTypeId,
            JurisdictionId = jurisdiction?.JurisdictionId,
            ClimateZoneId = climateZone?.ClimateZoneId,
            Elevation = dto.Elevation,
            NumberOfHoles = dto.NumberOfHoles,
            YearOfConstruction = dto.YearOfConstruction,
            YearOfIrrigationSystem = dto.YearOfIrrigationSystem,
            BacHeaAroGre = dto.BacHeaAroGre,
            IrrigationSystem = dto.IrrigationSystem,
            WaterSources = dto.WaterSources,
            WeatherStation = dto.WeatherStation,
            Tolerance = dto.Tolerance,
            SurfaceUniId = null,
            CutUniLenId = null,
            GreenSpeedUniLenId = null,
            TempUniId = null,
            RainUniId = null,
            ClippingsUniId = null,
            WindUniId = null,
            TotalSurface = dto.TotalSurface,
            TotalSurfaceUniId = null,
            ElevationUniLenId = null,
            StartInventory = dto.StartInventory,
            UsePlanning = dto.UsePlanning,
            EnterWeatherManually = dto.EnterWeatherManually,
            DollarSpotMedium = dto.DollarSpotMedium,
            DollarSpotHigh = dto.DollarSpotHigh,
            StdWorkDay = dto.StdWorkDay,
            Coordinate = coordinate,
            CoordinateId = coordinate?.CoordinatesId,
            CommonStartTime = dto.CommonStartTime,
            Ibu = ibu,
            IbuId = ibu.IbuId,
            Address = address,
            AddressId = address?.AddressId,
            Contact = contactDetail,
            ContactId = contactDetail?.ContactId,
            Architect = architect,
            ArchitectId = architect?.ArchitectId,
            DoG = directorOfGolf,
            DoGId = directorOfGolf?.DirectorOfGolfId,
        };

    public static void UpdateEntity(
        this Field field,
        FieldUpdateDto dto,
        FieldType fieldType,
        Address? address,
        Coordinate? coordinate,
        ContactDetail? contactDetail,
        Architect? architect,
        DirectorOfGolf? directorOfGolf,
        Jurisdiction? jurisdiction,
        Coordinate? coor,
        ClimateZone? climateZone)
    {
        field.FieldName = dto.FieldName;
        field.Active = dto.Active;
        field.JurisdictionId = jurisdiction?.JurisdictionId;
        field.ClimateZoneId = climateZone?.ClimateZoneId;
        field.Elevation = dto.Elevation;
        field.NumberOfHoles = dto.NumberOfHoles;
        field.YearOfConstruction = dto.YearOfConstruction;
        field.YearOfIrrigationSystem = dto.YearOfIrrigationSystem;
        field.BacHeaAroGre = dto.BacHeaAroGre;
        field.IrrigationSystem = dto.IrrigationSystem;
        field.WaterSources = dto.WaterSources;
        field.WeatherStation = dto.WeatherStation;
        field.Tolerance = dto.Tolerance;
        field.SurfaceUniId = null;
        field.CutUniLenId = null;
        field.GreenSpeedUniLenId = null;
        field.TempUniId = null;
        field.RainUniId = null;
        field.ClippingsUniId = null;
        field.WindUniId = null;
        field.ElevationUniLenId = null;
        field.StartInventory = dto.StartInventory;
        field.UsePlanning = dto.UsePlanning;
        field.EnterWeatherManually = dto.EnterWeatherManually;
        field.DollarSpotMedium = dto.DollarSpotMedium;
        field.DollarSpotHigh = dto.DollarSpotHigh;
        field.StdWorkDay = dto.StdWorkDay;
        field.Coordinate = coordinate;
        field.CoordinateId = coordinate?.CoordinatesId;
        field.CommonStartTime = dto.CommonStartTime;
        field.Address = address;
        field.AddressId = address?.AddressId;
        field.Contact = contactDetail;
        field.ContactId = contactDetail?.ContactId;
        field.Architect = architect;
        field.ArchitectId = architect?.ArchitectId;
        field.DoG = directorOfGolf;
        field.DoGId = directorOfGolf?.DirectorOfGolfId;
        field.FieldTypeId = fieldType?.FieldTypeId;
        field.Coordinate = coor;
        field.CoordinateId = coor?.CoordinatesId;
        field.TotalSurface = dto.TotalSurface;
        field.TotalSurfaceUniId = null;
    }

    public static FieldUpdateDto ToUpdateDto(this FieldDto dto)
        => new(
            PubId: dto.PubId,
            Active: dto.Active,
            FieldTypePubId: dto.FieldTypePubId,
            FieldName: dto.FieldName,
            JurisdictionPubId: dto.JurisdictionPubId,
            ClimateZonePubId: dto.ClimateZonePubId,
            Elevation: dto.Elevation,
            TotalSurface: dto.TotalSurface,
            NumberOfHoles: dto.NumberOfHoles,
            YearOfConstruction: dto.YearOfConstruction,
            YearOfIrrigationSystem: dto.YearOfIrrigationSystem,
            BacHeaAroGre: dto.BacHeaAroGre,
            IrrigationSystem: dto.IrrigationSystem,
            WaterSources: dto.WaterSources,
            WeatherStation: dto.WeatherStation,
            Tolerance: dto.Tolerance,
            CutUniLenPubId: dto.CutUniLenPubId,
            GreenSpeedUniLenPubId: dto.GreenSpeedUniLenPubId,
            TempUniPubId: dto.TempUniPubId,
            RainUniPubId: dto.RainUniPubId,
            ClippingsUniPubId: dto.ClippingsUniPubId,
            WindUniPubId: dto.WindUniPubId,
            SurfaceUniPubId: dto.SurfaceUniPubId,
            TotalSurfaceUniPubId: dto.TotalSurfaceUniPubId,
            ElevationUnitPubId: dto.ElevationUnitPubId,
            StartInventory: dto.StartInventory,
            UsePlanning: dto.UsePlanning,
            EnterWeatherManually: dto.EnterWeatherManually,
            DollarSpotMedium: dto.DollarSpotMedium,
            DollarSpotHigh: dto.DollarSpotHigh,
            AddressPubId: dto.AddressPubId,
            ContactPubId: dto.ContactPubId,
            ArchitectPubId: dto.ArchitectPubId,
            DoGPubId: dto.DoGPubId,
            StdWorkDay: dto.StdWorkDay,
            CoordinatePubId: dto.CoordinatePubId,
            CommonStartTime: dto.CommonStartTime,
            PrimaryRiskAreaPubId: dto.PrimaryRiskAreaPubId,
            AppImagePubId: dto.AppImagePubId
        );

    public static FieldCreateDto ToCreateDto(this FieldUpdateDto dto)
        => new(
            FieldTypePubId: dto.FieldTypePubId,
            FieldName: dto.FieldName,
            JurisdictionPubId: dto.JurisdictionPubId,
            ClimateZonePubId: dto.ClimateZonePubId,
            Elevation: dto.Elevation,
            TotalSurface: dto.TotalSurface,
            NumberOfHoles: dto.NumberOfHoles,
            YearOfConstruction: dto.YearOfConstruction,
            YearOfIrrigationSystem: dto.YearOfIrrigationSystem,
            BacHeaAroGre: dto.BacHeaAroGre,
            IrrigationSystem: dto.IrrigationSystem,
            WaterSources: dto.WaterSources,
            WeatherStation: dto.WeatherStation,
            Tolerance: dto.Tolerance,
            CutUniLenPubId: dto.CutUniLenPubId,
            GreenSpeedUniLenPubId: dto.GreenSpeedUniLenPubId,
            TempUniPubId: dto.TempUniPubId,
            RainUniPubId: dto.RainUniPubId,
            ClippingsUniPubId: dto.ClippingsUniPubId,
            WindUniPubId: dto.WindUniPubId,
            SurfaceUniPubId: dto.SurfaceUniPubId,
            TotalSurfaceUniPubId: dto.TotalSurfaceUniPubId,
            ElevationUnitPubId: dto.ElevationUnitPubId,
            StartInventory: dto.StartInventory,
            UsePlanning: dto.UsePlanning,
            EnterWeatherManually: dto.EnterWeatherManually,
            DollarSpotMedium: dto.DollarSpotMedium,
            DollarSpotHigh: dto.DollarSpotHigh,
            AddressPubId: dto.AddressPubId,
            ContactPubId: dto.ContactPubId,
            ArchitectPubId: dto.ArchitectPubId,
            DoGPubId: dto.DoGPubId,
            StdWorkDay: dto.StdWorkDay,
            CoordinatePubId: dto.CoordinatePubId,
            IbuPubId: dto.IbuPubId,
            CommonStartTime: dto.CommonStartTime,
            PrimaryRiskAreaPubId: dto.PrimaryRiskAreaPubId
        );
}
