namespace AmsRecords.ClimateZones;

public static class ClimateZoneDtos
{
    public sealed record ClimateZoneDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("zoneCode")] string ZoneCode,
        [property: JsonPropertyName("zoneName")] string ZoneName,
        [property: JsonPropertyName("classificationSystem")] string ClassificationSystem,
        [property: JsonPropertyName("sourceName")] string? SourceName,
        [property: JsonPropertyName("sourceUrl")] string? SourceUrl,
        [property: JsonPropertyName("notes")] string? Notes,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("active")] bool Active
    )
    {
        [JsonPropertyName("displayName")]
        public string DisplayName => $"{ZoneCode} - {ZoneName}";
    }

    public sealed record ClimateZoneCreateDto(
        [property: JsonPropertyName("zoneCode")] string ZoneCode,
        [property: JsonPropertyName("zoneName")] string ZoneName,
        [property: JsonPropertyName("classificationSystem")] string ClassificationSystem,
        [property: JsonPropertyName("sourceName")] string? SourceName,
        [property: JsonPropertyName("sourceUrl")] string? SourceUrl,
        [property: JsonPropertyName("notes")] string? Notes,
        [property: JsonPropertyName("sortOrder")] int SortOrder
    );

    public sealed record ClimateZoneUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("zoneCode")] string ZoneCode,
        [property: JsonPropertyName("zoneName")] string ZoneName,
        [property: JsonPropertyName("classificationSystem")] string ClassificationSystem,
        [property: JsonPropertyName("sourceName")] string? SourceName,
        [property: JsonPropertyName("sourceUrl")] string? SourceUrl,
        [property: JsonPropertyName("notes")] string? Notes,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record ClimateZoneImportResultDto(
        [property: JsonPropertyName("created")] int Created,
        [property: JsonPropertyName("updated")] int Updated,
        [property: JsonPropertyName("totalOfficialZones")] int TotalOfficialZones
    );

    public sealed record ClimateZoneAssignFieldsRequestDto(
        [property: JsonPropertyName("overwriteExisting")] bool OverwriteExisting
    );

    public sealed record ClimateZoneAssignFieldsResultDto(
        [property: JsonPropertyName("totalFields")] int TotalFields,
        [property: JsonPropertyName("assigned")] int Assigned,
        [property: JsonPropertyName("unchanged")] int Unchanged,
        [property: JsonPropertyName("missingCoordinates")] int MissingCoordinates,
        [property: JsonPropertyName("unresolved")] int Unresolved,
        [property: JsonPropertyName("missingRaster")] bool MissingRaster,
        [property: JsonPropertyName("message")] string Message
    );
}
