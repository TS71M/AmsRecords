using static AmsRecords.Diseases.DiseaseDtos;

namespace AmsRecords.Diseases;

public static class DiseaseExtensions
{
    public static DiseaseSummaryDto ToSummaryDto(this Disease disease)
        => new(
            PubId: disease.PubId,
            Name: disease.Name,
            Summary: BuildSummary(disease),
            IndicatorPicturePubId: disease.IndicatorPicture?.PubId,
            Aliases: disease.DiseaseAliases
                .Select(x => x.AliasName)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x)
                .ToList(),
            GrassTypes: disease.DiseaseGrassTypes
                .Select(x => x.GrassType.GrassTypeName)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x)
                .ToList(),
            PictureCount: disease.DiseasePictures.Count);

    public static DiseaseAdminListDto ToAdminListDto(this Disease disease)
        => new(
            PubId: disease.PubId,
            Name: disease.Name,
            IndicatorPicturePubId: disease.IndicatorPicture?.PubId,
            AliasCount: disease.DiseaseAliases.Count,
            GrassTypeCount: disease.DiseaseGrassTypes.Count,
            PictureCount: disease.DiseasePictures.Count,
            TrainingExampleCount: disease.DiseaseTrainingExamples.Count);

    public static DiseaseDetailDto ToDetailDto(this Disease disease)
        => new(
            PubId: disease.PubId,
            Name: disease.Name,
            History: disease.History,
            SymptomsAndSigns: disease.SymptomsAndSigns,
            CausalAgent: disease.CausalAgent,
            DiseasePictureDescription: disease.DiseasePictureDescription,
            Epidemiology: disease.Epidemiology,
            DiseaseManagement: disease.DiseaseManagement,
            TempMin: disease.TempMin,
            TempMax: disease.TempMax,
            TempNight: disease.TempNight,
            Wet: disease.Wet,
            Humid: disease.Humid,
            Dry: disease.Dry,
            VeryDry: disease.VeryDry,
            IndicatorPicturePubId: disease.IndicatorPicture?.PubId,
            DiseaseCyclePicturePubId: disease.DiseaseCyclePicture?.PubId,
            Aliases: disease.DiseaseAliases
                .Select(x => x.AliasName)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x)
                .ToList(),
            GrassTypes: disease.DiseaseGrassTypes
                .OrderBy(x => x.GrassType.GrassTypeName)
                .Select(x => new DiseaseGrassTypeDto(
                    x.PubId,
                    x.GrassType.PubId,
                    x.GrassType.GrassTypeName))
                .ToList(),
            Pictures: disease.DiseasePictures
                .OrderBy(x => x.DiseasePictureId)
                .Select(x => new DiseasePictureDto(x.PubId, x.AppImage?.PubId))
                .ToList());

    public static DiseaseTrainingExampleDto ToTrainingDto(this DiseaseTrainingExample example)
        => new(
            PubId: example.PubId,
            DiseasePubId: example.Disease.PubId,
            DiseaseName: example.Disease.Name,
            ImagePubId: example.AppImage.PubId,
            AiPredictedCondition: example.AiPredictedCondition,
            FinalCondition: example.FinalCondition,
            SeasonTag: example.SeasonTag,
            WeatherTag: example.WeatherTag,
            Confidence: example.Confidence,
            ReviewNote: example.ReviewNote,
            IsApproved: example.IsApproved,
            UseForLearning: example.UseForLearning,
            UseForRetrieval: example.UseForRetrieval,
            Active: example.Active,
            CreatedAtUtc: example.CreatedAtUtc,
            ReviewedAtUtc: example.ReviewedAtUtc,
            SubmittedByName: example.SubmittedBy?.FullNameSnapshot,
            ReviewedByName: example.ReviewedBy?.FullNameSnapshot,
            Findings: example.Findings
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.DiseaseTrainingExampleFindingId)
                .Select(x => new DiseaseFindingDto(
                    x.PubId,
                    x.Disease?.PubId,
                    x.Disease?.Name,
                    x.ConditionName,
                    x.Role,
                    x.Confidence,
                    x.Note,
                    x.SortOrder))
                .ToList());

    static string BuildSummary(Disease disease)
    {
        var source = !string.IsNullOrWhiteSpace(disease.SymptomsAndSigns)
            ? disease.SymptomsAndSigns
            : disease.DiseaseManagement;

        if (string.IsNullOrWhiteSpace(source))
            return "";

        var trimmed = source.Trim();
        return trimmed.Length <= 220
            ? trimmed
            : trimmed[..217].TrimEnd() + "...";
    }
}
