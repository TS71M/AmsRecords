namespace AmsRecords.Procural;

public static class ProcurementReceivingPolicy
{
    public static string? ValidateLine(
        decimal quantityOrdered,
        decimal quantityAlreadyReceived,
        decimal quantityReceived,
        decimal quantityRejected,
        string? discrepancyReason)
    {
        if (quantityOrdered <= 0 || quantityAlreadyReceived < 0 || quantityReceived < 0 || quantityRejected < 0)
            return "Quantities cannot be negative and the ordered quantity must be positive.";

        if (quantityAlreadyReceived > quantityOrdered)
            return "The stored received quantity exceeds the order quantity.";

        if (quantityAlreadyReceived + quantityReceived > quantityOrdered)
            return "A received quantity exceeds the outstanding order quantity.";

        if (quantityRejected > 0 && string.IsNullOrWhiteSpace(discrepancyReason))
            return "Rejected quantities require a discrepancy reason.";

        return null;
    }
}
