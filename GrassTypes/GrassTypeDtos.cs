using Lib.Enums;

namespace AmsRecords.GrassTypes;

public static class GrassTypeDtos
{
    public sealed record GrassTypeDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")][param: MaxLength(250)] string GrassTypeName,
        [property: JsonPropertyName("pathway")] PhotosyntheticPathway Pathway,
        [property: JsonPropertyName("description")] string? Description,
        [property: JsonPropertyName("tempOpt")] decimal OptimalTemperatur,
        [property: JsonPropertyName("variation")] decimal Variation
    )
    {
        public GrassTypeDto() : this(Guid.Empty, string.Empty, PhotosyntheticPathway.None, null, 0, 0) { }
    }

    public sealed record GrassTypeMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")][param: MaxLength(250)] string GrassTypeName
    )
    {
        public GrassTypeMiniDto() : this(Guid.Empty, string.Empty) { }
    }

    public sealed record GrassTypeCreateDto(
        [property: JsonPropertyName("name")][param: MaxLength(250)] string GrassTypeName,
        [property: JsonPropertyName("pathway")] PhotosyntheticPathway Pathway,
        [property: JsonPropertyName("description")] string? Description,
        [property: JsonPropertyName("tempOpt")] decimal OptimalTemperatur,
        [property: JsonPropertyName("variation")] decimal Variation
    );

    public sealed record GrassTypeUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")][param: MaxLength(250)] string GrassTypeName,
        [property: JsonPropertyName("pathway")] PhotosyntheticPathway Pathway,
        [property: JsonPropertyName("description")] string? Description,
        [property: JsonPropertyName("tempOpt")] decimal OptimalTemperatur,
        [property: JsonPropertyName("variation")] decimal Variation
    );
}