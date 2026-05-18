using static AmsRecords.Ibus.IbuAccessProfileDtos;

namespace AmsRecords.Ibus;

public static class IbuAccessProfileExtensions
{
    public static IbuAccessProfileDto ToDto(this IbuAccessProfile profile)
        => new(
            PubId: profile.PubId,
            IbuPubId: profile.Ibu.PubId,
            ProfileName: profile.ProfileName,
            Description: profile.Description,
            IsDefault: profile.IsDefault,
            Active: profile.Active,
            SortOrder: profile.SortOrder,
            Permissions: profile.Permissions
                .OrderBy(x => x.ModuleKey)
                .ThenBy(x => x.PermissionKey)
                .Select(x => new IbuAccessProfilePermissionDto(
                    ModuleKey: x.ModuleKey,
                    PermissionKey: x.PermissionKey,
                    IsAllowed: x.IsAllowed))
                .ToList());
}
