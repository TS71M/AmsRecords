namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationPlanPeriodNutrientDto(string Code, decimal TotalGrams, decimal? GramsPerM2);
public sealed record ApplicationPlanPeriodNutrientsDto(string Period, int Year, int Number, int ApplicationCount,
    int IncompleteApplications, decimal ReferenceAreaM2, IReadOnlyList<ApplicationPlanPeriodNutrientDto> Nutrients);
