using System.Linq.Expressions;
using AmsModels;

namespace AmsRecords.Procural;

/// <summary>Shared account availability for procurement membership, assignment and readiness.</summary>
public static class ProcurementUserAvailability
{
    public static Expression<Func<User, bool>> At(DateTimeOffset now)
        => user => user.Active && (!user.LockoutEnd.HasValue || user.LockoutEnd <= now);
}
