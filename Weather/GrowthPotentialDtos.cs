namespace AmsRecords.Weather;

public sealed class GrowthPotentialDtos
{
    public sealed record GrowthPotentialFormulaDto(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("version")] int Version,
        [property: JsonPropertyName("defaults")] GrowthPotentialDefaultsDto Defaults,
        [property: JsonPropertyName("ast")] object Ast // see note below
    );

    public sealed record GrowthPotentialDefaultsDto(
        [property: JsonPropertyName("optC")] decimal OptC,
        [property: JsonPropertyName("varC")] decimal VarC
    );
}
