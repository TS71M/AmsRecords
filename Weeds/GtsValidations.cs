using AmsRecords.Messages;
using Lib.Lookups.Weather;
using static AmsRecords.Weather.GtsDtos;
using static AmsRecords.Weeds.WeedDtos;

namespace AmsRecords.Weeds;

public static class GtsValidations
{
    public static GtsTimingDto ToTiming(
        string category,
        string code,
        string name,
        decimal preferredFromGts,
        decimal preferredToGts,
        decimal acceptableFromGts,
        decimal acceptableToGts,
        string? note,
        AppMessageDto? noteMessage,
        decimal currentGts)
    {
        var (statusCode, statusName) = GtsTimingEvaluator.Evaluate(
            currentGts,
            preferredFromGts,
            preferredToGts,
            acceptableFromGts,
            acceptableToGts);

        return new GtsTimingDto(
            code,
            name,
            category,
            statusCode,
            statusName,
            preferredFromGts,
            preferredToGts,
            acceptableFromGts,
            acceptableToGts,
            note,
            noteMessage);
    }

    public static string? Validate(
        decimal preferredFromGts,
        decimal preferredToGts,
        decimal acceptableFromGts,
        decimal acceptableToGts)
    {
        if (preferredFromGts > preferredToGts)
            return "Preferred from GTS must be less than or equal to preferred to GTS.";

        if (acceptableFromGts > acceptableToGts)
            return "Acceptable from GTS must be less than or equal to acceptable to GTS.";

        if (preferredFromGts < acceptableFromGts || preferredToGts > acceptableToGts)
            return "Preferred GTS range must be inside the acceptable GTS range.";

        return null;
    }

    public static string? Validate(WeedCreateDto dto)
        => Validate(
            dto.PreferredFromGts,
            dto.PreferredToGts,
            dto.AcceptableFromGts,
            dto.AcceptableToGts);

    public static string? Validate(WeedUpdateDto dto)
         => Validate(
             dto.PreferredFromGts,
             dto.PreferredToGts,
             dto.AcceptableFromGts,
             dto.AcceptableToGts);
}