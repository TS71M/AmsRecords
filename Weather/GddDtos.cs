using AmsRecords.Messages;
using AmsRecords.Units;

namespace AmsRecords.Weather;

public sealed class GddDtos
{
    public sealed record GddSeasonStartDto(
        [property: JsonPropertyName("rule")] AppMessageDto Rule,
        [property: JsonPropertyName("status")] AppMessageDto Status,
        [property: JsonPropertyName("dateUtc")] DateTime? DateUtc,
        [property: JsonPropertyName("source")] AppMessageDto? Source,
        [property: JsonPropertyName("reason")] AppMessageDto? Reason
    );

    public sealed record GddDayDto(
        [property: JsonPropertyName("dateUtc")] DateOnly DateUtc,
        [property: JsonPropertyName("hasData")] bool HasData,

        [property: JsonPropertyName("tMin")] UnitValueDto? TMin,
        [property: JsonPropertyName("tMax")] UnitValueDto? TMax,
        [property: JsonPropertyName("tMean")] UnitValueDto? TMean,

        [property: JsonPropertyName("gdd")] decimal? Gdd,
        [property: JsonPropertyName("cumGdd")] decimal? CumGdd,
        [property: JsonPropertyName("cumGddSinceBiofix")] decimal? CumGddSinceBiofix,

        [property: JsonPropertyName("notes")] IReadOnlyList<AppMessageDto> Notes
    );

    public sealed record GddSeriesDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,

        [property: JsonPropertyName("fromUtc")] DateTime FromUtc,
        [property: JsonPropertyName("toUtc")] DateTime ToUtc,

        [property: JsonPropertyName("baseTempC")] decimal BaseTempC,
        [property: JsonPropertyName("capTempC")] decimal? CapTempC,
        [property: JsonPropertyName("baseTemp")] UnitValueDto? BaseTemp,
        [property: JsonPropertyName("capTemp")] UnitValueDto? CapTemp,
        [property: JsonPropertyName("temperatureUnitShort")] string TemperatureUnitShort,

        [property: JsonPropertyName("method")] AppMessageDto Method,
        [property: JsonPropertyName("generatedAtUtc")] DateTimeOffset GeneratedAtUtc,

        [property: JsonPropertyName("biofixMode")] string? BiofixMode,
        [property: JsonPropertyName("biofixDateUtc")] DateTime? BiofixDateUtc,
        [property: JsonPropertyName("cumGddSinceBiofix")] decimal? CumGddSinceBiofix,

        [property: JsonPropertyName("seasonStart")] GddSeasonStartDto? SeasonStart,

        [property: JsonPropertyName("days")] IReadOnlyList<GddDayDto> Days
    );

    public sealed record DailyTempAgg(
        DateOnly Day,
        int Count,
        decimal TminC,
        decimal TmaxC,
        string? Source = null
    );

    public static GddSeriesDto EmptySeries(
        Field field,
        DateTime fromUtc,
        DateTime toUtc,
        decimal baseTempC,
        decimal? capTempC,
        string? biofixMode,
        DateTime? biofixDateUtc)
        => new(
            FieldPubId: field.PubId,
            FieldName: field.FieldName,
            FromUtc: fromUtc,
            ToUtc: toUtc,
            BaseTempC: baseTempC,
            CapTempC: capTempC,
            BaseTemp: null,
            CapTemp: null,
            TemperatureUnitShort: field.TempUni?.UnitShort ?? "°C",
            Method: new(
                Code: GddAppMessageCodes.Methods.TminTmaxAvg,
                Severity: null,
                Args: null),
            GeneratedAtUtc: DateTimeOffset.UtcNow,
            BiofixMode: biofixMode is "none" or null ? null : biofixMode,
            BiofixDateUtc: biofixDateUtc,
            CumGddSinceBiofix: null,
            SeasonStart: null,
            Days: []
        );
}
