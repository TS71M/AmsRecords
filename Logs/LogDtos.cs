namespace AmsRecords.Logs;

public static class LogDtos
{
    public sealed record LogEntryDto(
        [property: JsonPropertyName("ref")] string Ref,
        [property: JsonPropertyName("message")] string? Message,
        [property: JsonPropertyName("messageTemplate")] string? MessageTemplate,
        [property: JsonPropertyName("level")] string? Level,
        [property: JsonPropertyName("timeStamp")] DateTime TimeStamp,
        [property: JsonPropertyName("exception")] string? Exception,
        [property: JsonPropertyName("properties")] string? Properties
    );

    public sealed record LogQueryDto(
        [property: JsonPropertyName("q")] string? Q,
        [property: JsonPropertyName("level")] string? Level,
        [property: JsonPropertyName("fromUtc")] DateTime? FromUtc,
        [property: JsonPropertyName("toUtc")] DateTime? ToUtc,
        [property: JsonPropertyName("page")] int Page = 1,
        [property: JsonPropertyName("pageSize")] int PageSize = 50
    );

    public sealed record PagedResultDto<T>(
        [property: JsonPropertyName("items")] IReadOnlyList<T> Items,
        [property: JsonPropertyName("total")] int Total
    );

    public sealed record LogRetentionResultDto(
        [property: JsonPropertyName("deletedInfo")] int DeletedInfo,
        [property: JsonPropertyName("deletedOther")] int DeletedOther,
        [property: JsonPropertyName("cutoffInfoUtc")] DateTime CutoffInfoUtc,
        [property: JsonPropertyName("cutoffOtherUtc")] DateTime CutoffOtherUtc
    );

    public sealed record DeleteSelectedRequest(
        [property: JsonPropertyName("refs")]
    IReadOnlyList<string> Refs
    );
}