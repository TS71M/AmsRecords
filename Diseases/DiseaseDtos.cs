using static AmsRecords.GrassTypes.GrassTypeDtos;

namespace AmsRecords.Diseases;

public static class DiseaseDtos
{
    public sealed record DiseaseFindingDto(
        [property: JsonPropertyName("pubId")] Guid? PubId,
        [property: JsonPropertyName("diseasePubId")] Guid? DiseasePubId,
        [property: JsonPropertyName("diseaseName")] string? DiseaseName,
        [property: JsonPropertyName("conditionName")] string ConditionName,
        [property: JsonPropertyName("role")] string Role,
        [property: JsonPropertyName("confidence")] decimal? Confidence,
        [property: JsonPropertyName("note")] string? Note,
        [property: JsonPropertyName("sortOrder")] int SortOrder);

    public sealed record DiseaseSummaryDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("summary")] string Summary,
        [property: JsonPropertyName("indicatorPicturePubId")] Guid? IndicatorPicturePubId,
        [property: JsonPropertyName("aliases")] List<string> Aliases,
        [property: JsonPropertyName("grassTypes")] List<string> GrassTypes,
        [property: JsonPropertyName("pictureCount")] int PictureCount);

    public sealed record DiseasePictureDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId);

    public sealed record DiseaseGrassTypeDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("grassTypePubId")] Guid GrassTypePubId,
        [property: JsonPropertyName("grassTypeName")] string GrassTypeName);

    public sealed record DiseaseDetailDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("history")] string History,
        [property: JsonPropertyName("symptomsAndSigns")] string SymptomsAndSigns,
        [property: JsonPropertyName("causalAgent")] string CausalAgent,
        [property: JsonPropertyName("diseasePictureDescription")] string DiseasePictureDescription,
        [property: JsonPropertyName("epidemiology")] string Epidemiology,
        [property: JsonPropertyName("diseaseManagement")] string DiseaseManagement,
        [property: JsonPropertyName("tempMin")] decimal TempMin,
        [property: JsonPropertyName("tempMax")] decimal TempMax,
        [property: JsonPropertyName("tempNight")] decimal TempNight,
        [property: JsonPropertyName("wet")] bool Wet,
        [property: JsonPropertyName("humid")] bool Humid,
        [property: JsonPropertyName("dry")] bool Dry,
        [property: JsonPropertyName("veryDry")] bool VeryDry,
        [property: JsonPropertyName("indicatorPicturePubId")] Guid? IndicatorPicturePubId,
        [property: JsonPropertyName("diseaseCyclePicturePubId")] Guid? DiseaseCyclePicturePubId,
        [property: JsonPropertyName("aliases")] List<string> Aliases,
        [property: JsonPropertyName("grassTypes")] List<DiseaseGrassTypeDto> GrassTypes,
        [property: JsonPropertyName("pictures")] List<DiseasePictureDto> Pictures);

    public sealed record DiseaseAdminListDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("indicatorPicturePubId")] Guid? IndicatorPicturePubId,
        [property: JsonPropertyName("aliasCount")] int AliasCount,
        [property: JsonPropertyName("grassTypeCount")] int GrassTypeCount,
        [property: JsonPropertyName("pictureCount")] int PictureCount,
        [property: JsonPropertyName("trainingExampleCount")] int TrainingExampleCount);

    public sealed record DiseaseCreateDto(
        [property: JsonPropertyName("name")]
        [Required, MinLength(2), MaxLength(250)]
        string Name,

        [property: JsonPropertyName("history")]
        string History,

        [property: JsonPropertyName("symptomsAndSigns")]
        string SymptomsAndSigns,

        [property: JsonPropertyName("causalAgent")]
        string CausalAgent,

        [property: JsonPropertyName("diseasePictureDescription")]
        [MaxLength(250)]
        string DiseasePictureDescription,

        [property: JsonPropertyName("epidemiology")]
        string Epidemiology,

        [property: JsonPropertyName("diseaseManagement")]
        string DiseaseManagement,

        [property: JsonPropertyName("tempMin")]
        decimal TempMin,

        [property: JsonPropertyName("tempMax")]
        decimal TempMax,

        [property: JsonPropertyName("tempNight")]
        decimal TempNight,

        [property: JsonPropertyName("wet")]
        bool Wet,

        [property: JsonPropertyName("humid")]
        bool Humid,

        [property: JsonPropertyName("dry")]
        bool Dry,

        [property: JsonPropertyName("veryDry")]
        bool VeryDry,

        [property: JsonPropertyName("indicatorPicturePubId")]
        Guid IndicatorPicturePubId,

        [property: JsonPropertyName("diseaseCyclePicturePubId")]
        Guid DiseaseCyclePicturePubId,

        [property: JsonPropertyName("aliases")]
        List<string>? Aliases,

        [property: JsonPropertyName("grassTypePubIds")]
        List<Guid>? GrassTypePubIds);

    public sealed record DiseaseUpdateDto(
        [property: JsonPropertyName("pubId")]
        [Required]
        Guid PubId,

        [property: JsonPropertyName("name")]
        [Required, MinLength(2), MaxLength(250)]
        string Name,

        [property: JsonPropertyName("history")]
        string History,

        [property: JsonPropertyName("symptomsAndSigns")]
        string SymptomsAndSigns,

        [property: JsonPropertyName("causalAgent")]
        string CausalAgent,

        [property: JsonPropertyName("diseasePictureDescription")]
        [MaxLength(250)]
        string DiseasePictureDescription,

        [property: JsonPropertyName("epidemiology")]
        string Epidemiology,

        [property: JsonPropertyName("diseaseManagement")]
        string DiseaseManagement,

        [property: JsonPropertyName("tempMin")]
        decimal TempMin,

        [property: JsonPropertyName("tempMax")]
        decimal TempMax,

        [property: JsonPropertyName("tempNight")]
        decimal TempNight,

        [property: JsonPropertyName("wet")]
        bool Wet,

        [property: JsonPropertyName("humid")]
        bool Humid,

        [property: JsonPropertyName("dry")]
        bool Dry,

        [property: JsonPropertyName("veryDry")]
        bool VeryDry,

        [property: JsonPropertyName("indicatorPicturePubId")]
        Guid IndicatorPicturePubId,

        [property: JsonPropertyName("diseaseCyclePicturePubId")]
        Guid DiseaseCyclePicturePubId,

        [property: JsonPropertyName("aliases")]
        List<string>? Aliases,

        [property: JsonPropertyName("grassTypePubIds")]
        List<Guid>? GrassTypePubIds);

    public sealed record DiseaseTrainingExampleDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("diseasePubId")] Guid DiseasePubId,
        [property: JsonPropertyName("diseaseName")] string DiseaseName,
        [property: JsonPropertyName("imagePubId")] Guid ImagePubId,
        [property: JsonPropertyName("aiPredictedCondition")] string? AiPredictedCondition,
        [property: JsonPropertyName("finalCondition")] string? FinalCondition,
        [property: JsonPropertyName("seasonTag")] string? SeasonTag,
        [property: JsonPropertyName("weatherTag")] string? WeatherTag,
        [property: JsonPropertyName("confidence")] decimal? Confidence,
        [property: JsonPropertyName("reviewNote")] string? ReviewNote,
        [property: JsonPropertyName("isApproved")] bool IsApproved,
        [property: JsonPropertyName("useForLearning")] bool UseForLearning,
        [property: JsonPropertyName("useForRetrieval")] bool UseForRetrieval,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("createdAtUtc")] DateTime CreatedAtUtc,
        [property: JsonPropertyName("reviewedAtUtc")] DateTime? ReviewedAtUtc,
        [property: JsonPropertyName("submittedByName")] string? SubmittedByName,
        [property: JsonPropertyName("reviewedByName")] string? ReviewedByName,
        [property: JsonPropertyName("findings")] IReadOnlyList<DiseaseFindingDto> Findings);

    public sealed record DiseaseTrainingExampleCreateDto(
        [property: JsonPropertyName("diseasePubId")]
        [Required]
        Guid DiseasePubId,

        [property: JsonPropertyName("imagePubId")]
        [Required]
        Guid ImagePubId,

        [property: JsonPropertyName("aiPredictedCondition")]
        [MaxLength(250)]
        string? AiPredictedCondition,

        [property: JsonPropertyName("finalCondition")]
        [MaxLength(250)]
        string? FinalCondition,

        [property: JsonPropertyName("seasonTag")]
        [MaxLength(250)]
        string? SeasonTag,

        [property: JsonPropertyName("weatherTag")]
        [MaxLength(250)]
        string? WeatherTag,

        [property: JsonPropertyName("confidence")]
        decimal? Confidence,

        [property: JsonPropertyName("reviewNote")]
        [MaxLength(4000)]
        string? ReviewNote,

        [property: JsonPropertyName("isApproved")]
        bool IsApproved,

        [property: JsonPropertyName("useForLearning")]
        bool UseForLearning,

        [property: JsonPropertyName("useForRetrieval")]
        bool UseForRetrieval,

        [property: JsonPropertyName("active")]
        bool Active,

        [property: JsonPropertyName("findings")]
        List<DiseaseFindingDto>? Findings);

    public sealed record DiseaseTrainingExampleUpdateDto(
        [property: JsonPropertyName("pubId")]
        [Required]
        Guid PubId,

        [property: JsonPropertyName("diseasePubId")]
        [Required]
        Guid DiseasePubId,

        [property: JsonPropertyName("imagePubId")]
        [Required]
        Guid ImagePubId,

        [property: JsonPropertyName("aiPredictedCondition")]
        [MaxLength(250)]
        string? AiPredictedCondition,

        [property: JsonPropertyName("finalCondition")]
        [MaxLength(250)]
        string? FinalCondition,

        [property: JsonPropertyName("seasonTag")]
        [MaxLength(250)]
        string? SeasonTag,

        [property: JsonPropertyName("weatherTag")]
        [MaxLength(250)]
        string? WeatherTag,

        [property: JsonPropertyName("confidence")]
        decimal? Confidence,

        [property: JsonPropertyName("reviewNote")]
        [MaxLength(4000)]
        string? ReviewNote,

        [property: JsonPropertyName("isApproved")]
        bool IsApproved,

        [property: JsonPropertyName("useForLearning")]
        bool UseForLearning,

        [property: JsonPropertyName("useForRetrieval")]
        bool UseForRetrieval,

        [property: JsonPropertyName("active")]
        bool Active,

        [property: JsonPropertyName("findings")]
        List<DiseaseFindingDto>? Findings);

    public sealed record DiseaseTrainingAnalyzeDto(
        [property: JsonPropertyName("diseasePubId")]
        Guid? DiseasePubId,

        [property: JsonPropertyName("imagePubId")]
        [Required]
        Guid ImagePubId,

        [property: JsonPropertyName("seasonTag")]
        [MaxLength(250)]
        string? SeasonTag,

        [property: JsonPropertyName("weatherTag")]
        [MaxLength(250)]
        string? WeatherTag,

        [property: JsonPropertyName("contextNote")]
        [MaxLength(4000)]
        string? ContextNote);

    public sealed record DiseaseTrainingAnalyzeResultDto(
        [property: JsonPropertyName("aiPredictedCondition")] string? AiPredictedCondition,
        [property: JsonPropertyName("suggestedFinalCondition")] string? SuggestedFinalCondition,
        [property: JsonPropertyName("confidence")] decimal? Confidence,
        [property: JsonPropertyName("seasonTag")] string? SeasonTag,
        [property: JsonPropertyName("weatherTag")] string? WeatherTag,
        [property: JsonPropertyName("reviewNote")] string? ReviewNote,
        [property: JsonPropertyName("findings")] IReadOnlyList<DiseaseFindingDto>? Findings);
}
