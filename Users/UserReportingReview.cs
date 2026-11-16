using System.Security.Cryptography;
using System.Text.Json;
using static AmsRecords.Users.UserDtos;

namespace AmsRecords.Users;

public static class UserReportingReview
{
    public static string Stamp(UserDto account) => Convert.ToHexString(SHA256.HashData(
        JsonSerializer.SerializeToUtf8Bytes(new
        {
            account.PubId,
            account.IbuPubId,
            account.UserPositionPubId,
            account.ManagerUserPubId,
            Managers = account.ManagerUserPubIds.Distinct().Order()
        })));

    public static string DirectReportsStamp(Guid accountId, IEnumerable<(Guid Id, bool Active)> directReports)
        => Convert.ToHexString(SHA256.HashData(JsonSerializer.SerializeToUtf8Bytes(new
        {
            AccountId = accountId,
            Reports = directReports.Distinct().OrderBy(report => report.Id)
                .Select(report => new { report.Id, report.Active })
        })));
}
