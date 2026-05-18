using static AmsRecords.Weather.GrowthPotentialDtos;

namespace AmsRecords.ClimateNormals;

public static class ClimateNormalDtos
{
    public sealed record FieldClimateNormalPivotDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("sourceType")] short SourceType,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("fromYear")] short? FromYear,
        [property: JsonPropertyName("toYear")] short? ToYear,
        [property: JsonPropertyName("updatedUtc")] DateTime UpdatedUtc,
        [property: JsonPropertyName("jan")] decimal Jan,
        [property: JsonPropertyName("feb")] decimal Feb,
        [property: JsonPropertyName("mar")] decimal Mar,
        [property: JsonPropertyName("apr")] decimal Apr,
        [property: JsonPropertyName("may")] decimal May,
        [property: JsonPropertyName("jun")] decimal Jun,
        [property: JsonPropertyName("jul")] decimal Jul,
        [property: JsonPropertyName("aug")] decimal Aug,
        [property: JsonPropertyName("sep")] decimal Sep,
        [property: JsonPropertyName("oct")] decimal Oct,
        [property: JsonPropertyName("nov")] decimal Nov,
        [property: JsonPropertyName("dec")] decimal Dec
    );

    public sealed record FieldClimateNormalPivotUpsertDto(
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("fromYear")] short? FromYear,
        [property: JsonPropertyName("toYear")] short? ToYear,
        [property: JsonPropertyName("jan")] decimal Jan,
        [property: JsonPropertyName("feb")] decimal Feb,
        [property: JsonPropertyName("mar")] decimal Mar,
        [property: JsonPropertyName("apr")] decimal Apr,
        [property: JsonPropertyName("may")] decimal May,
        [property: JsonPropertyName("jun")] decimal Jun,
        [property: JsonPropertyName("jul")] decimal Jul,
        [property: JsonPropertyName("aug")] decimal Aug,
        [property: JsonPropertyName("sep")] decimal Sep,
        [property: JsonPropertyName("oct")] decimal Oct,
        [property: JsonPropertyName("nov")] decimal Nov,
        [property: JsonPropertyName("dec")] decimal Dec
    );

    public sealed record FieldClimateNormalsViewDto(
        [property: JsonPropertyName("manual")] FieldClimateNormalPivotDto? Manual,
        [property: JsonPropertyName("calculated")] FieldClimateNormalPivotDto? Calculated
    );

    public sealed record TurfAreaOptionDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("gpParams")] GpParamsDto GpParams
    );

    public sealed record GpParamsDto(
        [property: JsonPropertyName("optC")] decimal OptC,
        [property: JsonPropertyName("varC")] decimal VarC,
        [property: JsonPropertyName("clipFactor")] decimal ClipFactor
    );

    public sealed record FieldClimateNormalsGpViewDto(
        [property: JsonPropertyName("normals")] FieldClimateNormalsViewDto Normals,
        [property: JsonPropertyName("formula")] GrowthPotentialFormulaDto Formula,
        [property: JsonPropertyName("availability")] ClimateNormalAvailabilityDto Availability
    );

    public static FieldClimateNormalPivotUpsertDto NewDefaultManual()
        => new(
            Active: false,
            FromYear: null,
            ToYear: null,
            Jan: 0, Feb: 0, Mar: 0, Apr: 0, May: 0, Jun: 0,
            Jul: 0, Aug: 0, Sep: 0, Oct: 0, Nov: 0, Dec: 0);

    public sealed record ClimateNormalAvailabilityDto(
        [property: JsonPropertyName("firstAvailableYear")] short? FirstAvailableYear,
        [property: JsonPropertyName("lastAvailableYear")] short? LastAvailableYear,
        [property: JsonPropertyName("fullYears")] List<short> FullYears,
        [property: JsonPropertyName("fullYearCount")] int FullYearCount,
        [property: JsonPropertyName("canLastFullYear")] bool CanLastFullYear,
        [property: JsonPropertyName("canLast5FullYears")] bool CanLast5FullYears,
        [property: JsonPropertyName("canLast10FullYears")] bool CanLast10FullYears
    );

    public sealed record FieldClimateNormalRecalcResultDto(
        [property: JsonPropertyName("pivot")] FieldClimateNormalPivotDto? Pivot,
        [property: JsonPropertyName("requestedFromYear")] short? RequestedFromYear,
        [property: JsonPropertyName("requestedToYear")] short? RequestedToYear,
        [property: JsonPropertyName("actualFromYear")] short? ActualFromYear,
        [property: JsonPropertyName("actualToYear")] short? ActualToYear,
        [property: JsonPropertyName("availableFromYear")] short? AvailableFromYear,
        [property: JsonPropertyName("availableToYear")] short? AvailableToYear,
        [property: JsonPropertyName("fullYearsAvailable")] int FullYearsAvailable,
        [property: JsonPropertyName("fullYearsUsed")] int FullYearsUsed,
        [property: JsonPropertyName("wasAdjusted")] bool WasAdjusted,
        [property: JsonPropertyName("message")] string? Message
    );
}
