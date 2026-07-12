using static AmsRecords.UserPageVisits.UserPageVisitDtos;

namespace AmsRecords.UserPageVisits;

public static class UserPageVisitExtensions
{
    public static UserPageVisitDto ToDto(
        this UserPageVisit e,
        Guid? ibuPubId,
        string? ibuName,
        Guid userPubId,
        string? userDisplayName,
        string? userName,
        string? email)
        => new(
            PubId: e.PubId,
            IbuPubId: ibuPubId,
            IbuName: ibuName,
            UserPubId: userPubId,
            UserDisplayName: userDisplayName,
            UserName: userName,
            Email: email,
            App: e.App,
            Path: e.Path,
            PageTitle: e.PageTitle,
            Method: e.Method,
            IpAddress: e.IpAddress,
            UserAgent: e.UserAgent,
            VisitedUtc: e.VisitedUtc
        );
}
