namespace AmsRecords.Irrigation;

public static class IrrigationDigitalTwinDtos
{
    public sealed record IrrigationSystemDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("manufacturer")] string Manufacturer,
        [property: JsonPropertyName("controlSystem")] string ControlSystem,
        [property: JsonPropertyName("sourceSystem")] string SourceSystem,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("controllerCount")] int ControllerCount,
        [property: JsonPropertyName("headCount")] int HeadCount,
        [property: JsonPropertyName("areaCount")] int AreaCount);

    public sealed record IrrigationControllerDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("irrigationSystemPubId")] Guid IrrigationSystemPubId,
        [property: JsonPropertyName("irrigationSystemName")] string IrrigationSystemName,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("controllerNumber")] int? ControllerNumber,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record IrrigationHeadDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("irrigationSystemPubId")] Guid IrrigationSystemPubId,
        [property: JsonPropertyName("irrigationSystemName")] string IrrigationSystemName,
        [property: JsonPropertyName("irrigationControllerPubId")] Guid? IrrigationControllerPubId,
        [property: JsonPropertyName("irrigationControllerName")] string IrrigationControllerName,
        [property: JsonPropertyName("sprinklerModelPubId")] Guid? SprinklerModelPubId,
        [property: JsonPropertyName("sprinklerModelName")] string SprinklerModelName,
        [property: JsonPropertyName("sprinklerNozzlePubId")] Guid? SprinklerNozzlePubId,
        [property: JsonPropertyName("sprinklerNozzleName")] string SprinklerNozzleName,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("hardwareAddress")] string HardwareAddress,
        [property: JsonPropertyName("mapX")] double? MapX,
        [property: JsonPropertyName("mapY")] double? MapY,
        [property: JsonPropertyName("elevationM")] decimal? ElevationM,
        [property: JsonPropertyName("arcDegrees")] decimal? ArcDegrees,
        [property: JsonPropertyName("orientationDegrees")] decimal? OrientationDegrees,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record IrrigationAreaHeadDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("headPubId")] Guid HeadPubId,
        [property: JsonPropertyName("headName")] string HeadName,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record IrrigationAreaBoundaryPointDto(
        [property: JsonPropertyName("sequence")] int Sequence,
        [property: JsonPropertyName("x")] double X,
        [property: JsonPropertyName("y")] double Y);

    public sealed record IrrigationAreaBoundaryDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("irrigationAreaPubId")] Guid IrrigationAreaPubId,
        [property: JsonPropertyName("areaM2")] double AreaM2,
        [property: JsonPropertyName("perimeterM")] double PerimeterM,
        [property: JsonPropertyName("minX")] double MinX,
        [property: JsonPropertyName("minY")] double MinY,
        [property: JsonPropertyName("maxX")] double MaxX,
        [property: JsonPropertyName("maxY")] double MaxY,
        [property: JsonPropertyName("centroidX")] double CentroidX,
        [property: JsonPropertyName("centroidY")] double CentroidY,
        [property: JsonPropertyName("points")] IReadOnlyList<IrrigationAreaBoundaryPointDto> Points);

    public sealed record IrrigationAreaBoundarySaveDto(
        [property: JsonPropertyName("points")] IReadOnlyList<IrrigationAreaBoundaryPointDto> Points);

    public sealed record IrrigationAreaDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("irrigationSystemPubId")] Guid IrrigationSystemPubId,
        [property: JsonPropertyName("irrigationSystemName")] string IrrigationSystemName,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("areaTypeCode")] string AreaTypeCode,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("heads")] IReadOnlyList<IrrigationAreaHeadDto> Heads);

    public sealed record IrrigationSourceReferenceDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("irrigationSystemPubId")] Guid IrrigationSystemPubId,
        [property: JsonPropertyName("irrigationControllerPubId")] Guid? IrrigationControllerPubId,
        [property: JsonPropertyName("irrigationHeadPubId")] Guid? IrrigationHeadPubId,
        [property: JsonPropertyName("sprinklerModelPubId")] Guid? SprinklerModelPubId,
        [property: JsonPropertyName("sprinklerNozzlePubId")] Guid? SprinklerNozzlePubId,
        [property: JsonPropertyName("irrigationAreaPubId")] Guid? IrrigationAreaPubId,
        [property: JsonPropertyName("sourceType")] string SourceType,
        [property: JsonPropertyName("sourceEntityType")] string SourceEntityType,
        [property: JsonPropertyName("sourceReference")] string SourceReference,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record IrrigationControllerOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("irrigationSystemPubId")] Guid IrrigationSystemPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("controllerNumber")] int? ControllerNumber);

    public sealed record IrrigationSystemOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name);

    public sealed record IrrigationSprinklerModelOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("displayName")] string DisplayName);

    public sealed record IrrigationSprinklerNozzleOptionSelectionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("sprinklerModelPubId")] Guid SprinklerModelPubId,
        [property: JsonPropertyName("displayName")] string DisplayName);

    public sealed record IrrigationFieldOptionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name);

    public sealed record IrrigationHeadOptionsDto(
        [property: JsonPropertyName("systems")] IReadOnlyList<IrrigationSystemOptionDto> Systems,
        [property: JsonPropertyName("controllers")] IReadOnlyList<IrrigationControllerOptionDto> Controllers,
        [property: JsonPropertyName("sprinklerModels")] IReadOnlyList<IrrigationSprinklerModelOptionDto> SprinklerModels,
        [property: JsonPropertyName("sprinklerNozzles")] IReadOnlyList<IrrigationSprinklerNozzleOptionSelectionDto> SprinklerNozzles);

    public sealed record IrrigationAreaOptionsDto(
        [property: JsonPropertyName("systems")] IReadOnlyList<IrrigationSystemOptionDto> Systems,
        [property: JsonPropertyName("fields")] IReadOnlyList<IrrigationFieldOptionDto> Fields,
        [property: JsonPropertyName("heads")] IReadOnlyList<IrrigationHeadDto> Heads);

    public sealed record IrrigationAreaHeadSelectionDto(
        [property: JsonPropertyName("headPubId")] Guid HeadPubId,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary);

    public sealed record IrrigationAreaHeadsSaveDto(
        [property: JsonPropertyName("heads")] IReadOnlyList<IrrigationAreaHeadSelectionDto> Heads);

    public sealed record IrrigationSystemSaveDto
    {
        [JsonPropertyName("ibuPubId")]
        public Guid IbuPubId { get; set; }

        [JsonPropertyName("name"), Required, StringLength(160, MinimumLength = 1)]
        public string Name { get; set; } = "";

        [JsonPropertyName("description"), StringLength(2000)]
        public string Description { get; set; } = "";

        [JsonPropertyName("manufacturer"), StringLength(120)]
        public string Manufacturer { get; set; } = "";

        [JsonPropertyName("controlSystem"), StringLength(160)]
        public string ControlSystem { get; set; } = "";

        [JsonPropertyName("sourceSystem"), StringLength(120)]
        public string SourceSystem { get; set; } = "";

        [JsonPropertyName("active")]
        public bool Active { get; set; } = true;
    }

    public sealed record IrrigationControllerSaveDto
    {
        [JsonPropertyName("irrigationSystemPubId")]
        public Guid IrrigationSystemPubId { get; set; }

        [JsonPropertyName("name"), Required, StringLength(160, MinimumLength = 1)]
        public string Name { get; set; } = "";

        [JsonPropertyName("controllerNumber"), Range(0, int.MaxValue)]
        public int? ControllerNumber { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; } = true;
    }

    public sealed record IrrigationHeadSaveDto
    {
        [JsonPropertyName("irrigationSystemPubId")]
        public Guid IrrigationSystemPubId { get; set; }

        [JsonPropertyName("irrigationControllerPubId")]
        public Guid? IrrigationControllerPubId { get; set; }

        [JsonPropertyName("sprinklerModelPubId")]
        public Guid? SprinklerModelPubId { get; set; }

        [JsonPropertyName("sprinklerNozzlePubId")]
        public Guid? SprinklerNozzlePubId { get; set; }

        [JsonPropertyName("name"), Required, StringLength(160, MinimumLength = 1)]
        public string Name { get; set; } = "";

        [JsonPropertyName("hardwareAddress"), StringLength(200)]
        public string HardwareAddress { get; set; } = "";

        [JsonPropertyName("mapX")]
        public double? MapX { get; set; }

        [JsonPropertyName("mapY")]
        public double? MapY { get; set; }

        [JsonPropertyName("elevationM"), Range(typeof(decimal), "-1000", "10000")]
        public decimal? ElevationM { get; set; }

        [JsonPropertyName("arcDegrees"), Range(typeof(decimal), "0", "360")]
        public decimal? ArcDegrees { get; set; }

        [JsonPropertyName("orientationDegrees"), Range(typeof(decimal), "0", "360")]
        public decimal? OrientationDegrees { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; } = true;
    }

    public sealed record IrrigationAreaSaveDto
    {
        [JsonPropertyName("irrigationSystemPubId")]
        public Guid IrrigationSystemPubId { get; set; }

        [JsonPropertyName("fieldPubId")]
        public Guid? FieldPubId { get; set; }

        [JsonPropertyName("name"), Required, StringLength(160, MinimumLength = 1)]
        public string Name { get; set; } = "";

        [JsonPropertyName("areaTypeCode"), StringLength(80)]
        public string AreaTypeCode { get; set; } = "";

        [JsonPropertyName("active")]
        public bool Active { get; set; } = true;
    }

    public sealed record IrrigationSourceReferenceSaveDto
    {
        [JsonPropertyName("irrigationSystemPubId")]
        public Guid IrrigationSystemPubId { get; set; }

        [JsonPropertyName("irrigationControllerPubId")]
        public Guid? IrrigationControllerPubId { get; set; }

        [JsonPropertyName("irrigationHeadPubId")]
        public Guid? IrrigationHeadPubId { get; set; }

        [JsonPropertyName("sprinklerModelPubId")]
        public Guid? SprinklerModelPubId { get; set; }

        [JsonPropertyName("sprinklerNozzlePubId")]
        public Guid? SprinklerNozzlePubId { get; set; }

        [JsonPropertyName("irrigationAreaPubId")]
        public Guid? IrrigationAreaPubId { get; set; }

        [JsonPropertyName("sourceType"), Required, StringLength(80, MinimumLength = 1)]
        public string SourceType { get; set; } = "";

        [JsonPropertyName("sourceEntityType"), Required, StringLength(80, MinimumLength = 1)]
        public string SourceEntityType { get; set; } = "";

        [JsonPropertyName("sourceReference"), Required, StringLength(300, MinimumLength = 1)]
        public string SourceReference { get; set; } = "";

        [JsonPropertyName("active")]
        public bool Active { get; set; } = true;
    }
}
