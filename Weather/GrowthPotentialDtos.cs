namespace AmsRecords.Weather;

public sealed class GrowthPotentialDtos
{
    public sealed record GrowthPotentialFormulaDto(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("version")] int Version,
        [property: JsonPropertyName("defaults")] GrowthPotentialDefaultsDto Defaults,
        [property: JsonPropertyName("ast")] object Ast, // see note below
        [property: JsonPropertyName("profile")] GrowthPotentialProfileDto? Profile = null
    );

    public sealed record GrowthPotentialDefaultsDto(
        [property: JsonPropertyName("optC")] decimal OptC,
        [property: JsonPropertyName("varC")] decimal VarC
    );

    public sealed record GrowthPotentialProfileDto(
        [property: JsonPropertyName("status")] string Status,
        [property: JsonPropertyName("source")] string Source,
        [property: JsonPropertyName("profile")] string Profile,
        [property: JsonPropertyName("areaPubId")] Guid? AreaPubId,
        [property: JsonPropertyName("areaName")] string? AreaName,
        [property: JsonPropertyName("optC")] decimal? OptC,
        [property: JsonPropertyName("varC")] decimal? VarC,
        [property: JsonPropertyName("components")] IReadOnlyList<GrowthPotentialComponentDto> Components,
        [property: JsonPropertyName("message")] string? Message = null);

    public sealed record GrowthPotentialComponentDto(
        [property: JsonPropertyName("label")] string Label,
        [property: JsonPropertyName("pathway")] string Pathway,
        [property: JsonPropertyName("weightPct")] decimal WeightPct,
        [property: JsonPropertyName("optC")] decimal OptC,
        [property: JsonPropertyName("varC")] decimal VarC);
}
