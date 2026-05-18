using Lib.Enums;

namespace AmsRecords.GrassSpecies;

public static class GrassSpeciesDtos
{
    public sealed record GrassSpeciesDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("latinName")] string? LatinName,
        [property: JsonPropertyName("grassTypeId")] Guid GrassTypePubId,
        [property: JsonPropertyName("grassTypeName")] string GrassTypeName,
        [property: JsonPropertyName("showInGtsInterpretation")] bool ShowInGtsInterpretation,

        [property: JsonPropertyName("nMax")] decimal NMaxYear,
        [property: JsonPropertyName("nMaxMonth")] decimal NMaxMonth,
        [property: JsonPropertyName("knRatioMin")] decimal KNRatioMin,
        [property: JsonPropertyName("knRatioMax")] decimal KNRatioMax,
        [property: JsonPropertyName("pHMin")] decimal PHMin,
        [property: JsonPropertyName("pHMax")] decimal PHMax,

        [property: JsonPropertyName("saltTolMin")] int SaltToleranceMin,
        [property: JsonPropertyName("saltTolMax")] int SaltToleranceMax,

        [property: JsonPropertyName("growthHabit")] string? GrowthHabit,
        [property: JsonPropertyName("liguleLeafMargin")] string? LiguleLeafMargin,
        [property: JsonPropertyName("auriclesLeafHairs")] string? AuriclesLeafHairs,
        [property: JsonPropertyName("vernationLeafStructure")] string? VernationLeafStructure,
        [property: JsonPropertyName("collarLeafArrangement")] string? CollarLeafArrangement,
        [property: JsonPropertyName("seedheadColorFlowerColor")] string? SeedheadColorFlowerColor,
        [property: JsonPropertyName("rootType")] string? RootType,

        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("updatedAtUtc")] DateTime UpdatedAtUtc,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId
    );

    public sealed record GrassSpeciesCreateDto(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("latinName")] string? LatinName,
        [property: JsonPropertyName("grassTypeId")] Guid GrassTypePubId,
        [property: JsonPropertyName("showInGtsInterpretation")] bool ShowInGtsInterpretation,

        [property: JsonPropertyName("nMax")] decimal NMaxYear,
        [property: JsonPropertyName("nMaxMonth")] decimal NMaxMonth,
        [property: JsonPropertyName("knRatioMin")] decimal KNRatioMin,
        [property: JsonPropertyName("knRatioMax")] decimal KNRatioMax,
        [property: JsonPropertyName("pHMin")] decimal PHMin,
        [property: JsonPropertyName("pHMax")] decimal PHMax,

        [property: JsonPropertyName("saltTolMin")] int SaltToleranceMin,
        [property: JsonPropertyName("saltTolMax")] int SaltToleranceMax,

        [property: JsonPropertyName("growthHabit")] string? GrowthHabit,
        [property: JsonPropertyName("liguleLeafMargin")] string? LiguleLeafMargin,
        [property: JsonPropertyName("auriclesLeafHairs")] string? AuriclesLeafHairs,
        [property: JsonPropertyName("vernationLeafStructure")] string? VernationLeafStructure,
        [property: JsonPropertyName("collarLeafArrangement")] string? CollarLeafArrangement,
        [property: JsonPropertyName("seedheadColorFlowerColor")] string? SeedheadColorFlowerColor,
        [property: JsonPropertyName("rootType")] string? RootType
    );

    public sealed record GrassSpeciesUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("latinName")] string? LatinName,
        [property: JsonPropertyName("grassTypePubId")] Guid GrassTypePubId,
        [property: JsonPropertyName("showInGtsInterpretation")] bool ShowInGtsInterpretation,

        [property: JsonPropertyName("nMax")] decimal NMaxYear,
        [property: JsonPropertyName("nMaxMonth")] decimal NMaxMonth,
        [property: JsonPropertyName("knRatioMin")] decimal KNRatioMin,
        [property: JsonPropertyName("knRatioMax")] decimal KNRatioMax,
        [property: JsonPropertyName("pHMin")] decimal PHMin,
        [property: JsonPropertyName("pHMax")] decimal PHMax,

        [property: JsonPropertyName("saltTolMin")] int SaltToleranceMin,
        [property: JsonPropertyName("saltTolMax")] int SaltToleranceMax,

        [property: JsonPropertyName("growthHabit")] string? GrowthHabit,
        [property: JsonPropertyName("liguleLeafMargin")] string? LiguleLeafMargin,
        [property: JsonPropertyName("auriclesLeafHairs")] string? AuriclesLeafHairs,
        [property: JsonPropertyName("vernationLeafStructure")] string? VernationLeafStructure,
        [property: JsonPropertyName("collarLeafArrangement")] string? CollarLeafArrangement,
        [property: JsonPropertyName("seedheadColorFlowerColor")] string? SeedheadColorFlowerColor,
        [property: JsonPropertyName("rootType")] string? RootType
    )
    {
        public GrassSpeciesUpdateDto()
            : this(
                  PubId: Guid.Empty,
                  Active: true,
                  Name: "",
                  LatinName: null,
                  GrassTypePubId: Guid.Empty,
                  NMaxYear: 0,
                  NMaxMonth: 0,
                  KNRatioMin: 1.0m,
                  KNRatioMax: 1.5m,
                  PHMin: 5.0m,
                  PHMax: 8.0m,
                  SaltToleranceMin: 0,
                  SaltToleranceMax: 0,
                  GrowthHabit: null,
                  LiguleLeafMargin: null,
                  AuriclesLeafHairs: null,
                  VernationLeafStructure: null,
                  CollarLeafArrangement: null,
                  SeedheadColorFlowerColor: null,
                  RootType: null,
                  ShowInGtsInterpretation: false
              )
        {
        }
    }

    public sealed record GrassSpeciesMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name
    );

    public sealed record GrassSpeciesSetImageDto(
       [property: JsonPropertyName("imagePubId")] Guid ImagePubId
   );

    public sealed record ComparisonRow(
       Guid PubId,
       string Name,
       string? LatinName,
       string GrassTypeName,
       PhotosyntheticPathway Pathway,
       string NYear,
       string NMonth,
       string KN,
       string PH,
       string Salt
   );

}
