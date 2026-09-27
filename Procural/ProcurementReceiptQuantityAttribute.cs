using System.ComponentModel.DataAnnotations;

namespace AmsRecords.Procural;

/// <summary>Shared receipt quantity bounds for API validation and client-side error presentation.</summary>
public sealed class ProcurementReceiptQuantityAttribute : RangeAttribute
{
    public ProcurementReceiptQuantityAttribute() : base(typeof(decimal), "0", "999999999999.999")
    {
        ParseLimitsInInvariantCulture = true;
        ErrorMessage = "Enter a valid non-negative quantity.";
    }
}
