namespace AmsRecords.ApplicationPlanner;

public sealed record ApplicationPlanRowTotalsDto(int ApplicationCount, decimal? ProductQuantity,
    string? QuantityUnit, decimal? Cost);
