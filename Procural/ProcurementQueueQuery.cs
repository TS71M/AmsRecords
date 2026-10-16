using System.ComponentModel.DataAnnotations;

namespace AmsRecords.Procural;

public sealed class ProcurementQueueQuery
{
    [Range(1, 1_000_000)] public int Page { get; set; } = 1;
    [Range(1, 100)] public int PageSize { get; set; } = 50;
    [MaxLength(100)] public string? Search { get; set; }

    public bool IsValid() => Validator.TryValidateObject(this, new ValidationContext(this), null, true);
    public int Offset => checked((Page - 1) * PageSize);
}
