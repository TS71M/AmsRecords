namespace AmsRecords.Global;

public sealed record SetActiveDto(
    [property: JsonPropertyName("active")] bool Active
);
