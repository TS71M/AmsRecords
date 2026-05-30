using static AmsRecords.LoginLogs.LoginLogDtos;

namespace AmsRecords.LoginLogs;

public static class LoginLogExtensions
{
    public static LoginLogDto ToDto(
        this LoginLog e,
        Guid? ibuPubId,
        string? ibuName,
        Guid? userPubId,
        string? userDisplayName)
        => new(
            PubId: e.PubId,
            IbuPubId: ibuPubId,
            IbuName: ibuName,
            UserPubId: userPubId,
            UserDisplayName: userDisplayName,
            AttemptedUserNameMasked: e.AttemptedUserNameMasked,
            AttemptedUserNameHash: e.AttemptedUserNameHash,
            OccuredUtc: e.OccuredUtc,
            Succeeded: e.Succeeded,
            FailureCode: e.FailureCode,
            IpAddress: e.IpAddress,
            UserAgent: e.UserAgent
        );
}
