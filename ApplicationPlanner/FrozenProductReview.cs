using System.Security.Cryptography;
using System.Text.Json;
using static AmsRecords.ApplicationPlanner.ApplicationPlannerDtos;

namespace AmsRecords.ApplicationPlanner;

public static class FrozenProductReview
{
    public static string Stamp(ApplicationPlanProductSnapshotDto product)
        => Convert.ToHexString(SHA256.HashData(JsonSerializer.SerializeToUtf8Bytes(product)));
}
