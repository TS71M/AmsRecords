using System.Security.Cryptography;
using System.Text.Json;

namespace AmsRecords.ApplicationPlanner;

public static class PlannerActualCopyProductIdentity
{
    // A name alone never identifies a historical formulation. Unknown analysis
    // stays per record, so missing evidence cannot silently merge substitutions.
    public static string Key(PlannerActualCopyRowDto row) => Convert.ToHexString(SHA256.HashData(
        JsonSerializer.SerializeToUtf8Bytes(new {
            row.ProductName, row.SourcePlanProductSnapshotPubId, row.RateUnit,
            row.QuantityUnit, row.DensityKgPerL, row.AnalysisComplete,
            UnknownIdentity = row.Nutrients.Count == 0 ? (Guid?)row.ExecutionPubId : null,
            Analysis = row.Nutrients.OrderBy(n => n.Code, StringComparer.Ordinal)
                .Select(n => new { n.Code, n.AnalysisAmount, n.AnalysisBasis }) })));
}
