using Lib.Enums;

namespace AmsRecords.Machinery;

public static class MachineryDtos
{
    public sealed record MachineryListDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("categoryName")] string CategoryName,
        [property: JsonPropertyName("manufacturerName")] string ManufacturerName,
        [property: JsonPropertyName("modelName")] string ModelName,
        [property: JsonPropertyName("registeredIbuPubId")] Guid RegisteredIbuPubId,
        [property: JsonPropertyName("registeredIbuName")] string RegisteredIbuName,
        [property: JsonPropertyName("stationedIbuPubId")] Guid StationedIbuPubId,
        [property: JsonPropertyName("stationedIbuName")] string StationedIbuName,
        [property: JsonPropertyName("stationedFieldPubId")] Guid? StationedFieldPubId,
        [property: JsonPropertyName("stationedFieldName")] string StationedFieldName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("blocked")] bool Blocked,
        [property: JsonPropertyName("broken")] bool Broken,
        [property: JsonPropertyName("applicationCapability")] MachineryApplicationCapability ApplicationCapability,
        [property: JsonPropertyName("isDefaultApplicationMachine")] bool IsDefaultApplicationMachine,
        [property: JsonPropertyName("workingWidth")] decimal? WorkingWidth,
        [property: JsonPropertyName("workingWidthUnit")] string WorkingWidthUnit,
        [property: JsonPropertyName("capacity")] decimal? Capacity,
        [property: JsonPropertyName("capacityUnit")] string CapacityUnit,
        [property: JsonPropertyName("defaultApplicationRate")] decimal? DefaultApplicationRate,
        [property: JsonPropertyName("defaultApplicationRateUnit")] string DefaultApplicationRateUnit,
        [property: JsonPropertyName("defaultApplicationAreaUnit")] string DefaultApplicationAreaUnit,
        [property: JsonPropertyName("lastCalibratedOn")] DateOnly? LastCalibratedOn,
        [property: JsonPropertyName("calibrationDueOn")] DateOnly? CalibrationDueOn);

    public sealed record MachineryFormDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("registeredIbuName")] string RegisteredIbuName,
        [property: JsonPropertyName("stationedIbuPubId")] Guid StationedIbuPubId,
        [property: JsonPropertyName("stationedIbuName")] string StationedIbuName,
        [property: JsonPropertyName("stationedFieldPubId")] Guid? StationedFieldPubId,
        [property: JsonPropertyName("stationedFieldName")] string StationedFieldName,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("categoryName")] string CategoryName,
        [property: JsonPropertyName("manufacturerName")] string ManufacturerName,
        [property: JsonPropertyName("modelName")] string ModelName,
        [property: JsonPropertyName("modelNumber")] string ModelNumber,
        [property: JsonPropertyName("registrationNumber")] string? RegistrationNumber,
        [property: JsonPropertyName("serialNumber")] string? SerialNumber,
        [property: JsonPropertyName("inventoryNumber")] string? InventoryNumber,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("blocked")] bool Blocked,
        [property: JsonPropertyName("broken")] bool Broken,
        [property: JsonPropertyName("applicationCapability")] MachineryApplicationCapability ApplicationCapability,
        [property: JsonPropertyName("isDefaultApplicationMachine")] bool IsDefaultApplicationMachine,
        [property: JsonPropertyName("workingWidth")] decimal? WorkingWidth,
        [property: JsonPropertyName("workingWidthUnitPubId")] Guid? WorkingWidthUnitPubId,
        [property: JsonPropertyName("capacity")] decimal? Capacity,
        [property: JsonPropertyName("capacityUnitPubId")] Guid? CapacityUnitPubId,
        [property: JsonPropertyName("defaultApplicationRate")] decimal? DefaultApplicationRate,
        [property: JsonPropertyName("defaultApplicationRateUnitPubId")] Guid? DefaultApplicationRateUnitPubId,
        [property: JsonPropertyName("defaultApplicationAreaUnitPubId")] Guid? DefaultApplicationAreaUnitPubId,
        [property: JsonPropertyName("lastCalibratedOn")] DateOnly? LastCalibratedOn,
        [property: JsonPropertyName("calibrationDueOn")] DateOnly? CalibrationDueOn,
        [property: JsonPropertyName("notes")] string Notes);

    public sealed record MachinerySelectionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("registeredIbuName")] string RegisteredIbuName,
        [property: JsonPropertyName("stationedIbuName")] string StationedIbuName,
        [property: JsonPropertyName("categoryName")] string CategoryName,
        [property: JsonPropertyName("manufacturerName")] string ManufacturerName,
        [property: JsonPropertyName("modelName")] string ModelName,
        [property: JsonPropertyName("applicationCapability")] MachineryApplicationCapability ApplicationCapability,
        [property: JsonPropertyName("isDefaultForField")] bool IsDefaultForField,
        [property: JsonPropertyName("isDefaultApplicationMachine")] bool IsDefaultApplicationMachine,
        [property: JsonPropertyName("workingWidth")] decimal? WorkingWidth,
        [property: JsonPropertyName("workingWidthUnit")] string WorkingWidthUnit,
        [property: JsonPropertyName("capacity")] decimal Capacity,
        [property: JsonPropertyName("capacityUnit")] string CapacityUnit,
        [property: JsonPropertyName("defaultApplicationRate")] decimal? DefaultApplicationRate,
        [property: JsonPropertyName("defaultApplicationRateUnit")] string DefaultApplicationRateUnit,
        [property: JsonPropertyName("defaultApplicationAreaUnit")] string DefaultApplicationAreaUnit,
        [property: JsonPropertyName("lastCalibratedOn")] DateOnly? LastCalibratedOn,
        [property: JsonPropertyName("calibrationDueOn")] DateOnly? CalibrationDueOn);

    public sealed record MachineryStationIbuDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("ibuName")] string IbuName,
        [property: JsonPropertyName("canStationWithoutField")] bool CanStationWithoutField,
        [property: JsonPropertyName("fields")] IReadOnlyList<MachineryStationFieldDto> Fields);

    public sealed record MachineryStationFieldDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("active")] bool Active);

    public sealed class MachinerySaveDto
    {
        [JsonPropertyName("ibuPubId")]
        public Guid IbuPubId { get; set; }

        [JsonPropertyName("name"), Required, StringLength(120, MinimumLength = 1)]
        public string Name { get; set; } = "";

        [JsonPropertyName("categoryName"), Required, StringLength(100, MinimumLength = 1)]
        public string CategoryName { get; set; } = "";

        [JsonPropertyName("manufacturerName"), StringLength(120)]
        public string ManufacturerName { get; set; } = "";

        [JsonPropertyName("modelName"), StringLength(250)]
        public string ModelName { get; set; } = "";

        [JsonPropertyName("modelNumber"), StringLength(250)]
        public string ModelNumber { get; set; } = "";

        [JsonPropertyName("registrationNumber"), StringLength(120)]
        public string? RegistrationNumber { get; set; }

        [JsonPropertyName("serialNumber"), StringLength(250)]
        public string? SerialNumber { get; set; }

        [JsonPropertyName("inventoryNumber"), StringLength(250)]
        public string? InventoryNumber { get; set; }

        [JsonPropertyName("stationedIbuPubId")]
        public Guid StationedIbuPubId { get; set; }

        [JsonPropertyName("stationedFieldPubId")]
        public Guid? StationedFieldPubId { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; } = true;

        [JsonPropertyName("blocked")]
        public bool Blocked { get; set; }

        [JsonPropertyName("broken")]
        public bool Broken { get; set; }

        [JsonPropertyName("applicationCapability")]
        public MachineryApplicationCapability ApplicationCapability { get; set; }

        [JsonPropertyName("isDefaultApplicationMachine")]
        public bool IsDefaultApplicationMachine { get; set; }

        [JsonPropertyName("workingWidth"), Range(typeof(decimal), "0.001", "9999999")]
        public decimal? WorkingWidth { get; set; }

        [JsonPropertyName("workingWidthUnitPubId")]
        public Guid? WorkingWidthUnitPubId { get; set; }

        [JsonPropertyName("capacity"), Range(typeof(decimal), "0.001", "999999999")]
        public decimal? Capacity { get; set; }

        [JsonPropertyName("capacityUnitPubId")]
        public Guid? CapacityUnitPubId { get; set; }

        [JsonPropertyName("defaultApplicationRate"), Range(typeof(decimal), "0.001", "999999999")]
        public decimal? DefaultApplicationRate { get; set; }

        [JsonPropertyName("defaultApplicationRateUnitPubId")]
        public Guid? DefaultApplicationRateUnitPubId { get; set; }

        [JsonPropertyName("defaultApplicationAreaUnitPubId")]
        public Guid? DefaultApplicationAreaUnitPubId { get; set; }

        [JsonPropertyName("lastCalibratedOn")]
        public DateOnly? LastCalibratedOn { get; set; }

        [JsonPropertyName("calibrationDueOn")]
        public DateOnly? CalibrationDueOn { get; set; }

        [JsonPropertyName("notes"), StringLength(2000)]
        public string Notes { get; set; } = "";
    }
}
