using AmsRecords.ContactDetails;
using AmsRecords.Name;
using AmsRecords.Roles;
using static AmsRecords.Users.UserDtos;

namespace AmsRecords.Users;

public static class UserExtensions
{

    public static UserUpdateDto ToUpdateDto(this UserDto dto)
        => new(
            PubId: dto.PubId,
            IbuPubId: dto.IbuPubId,
            Email: dto.Email,
            UserName: dto.UserName,
            LockedOut: dto.LockedOut,
            Name: dto.Name,
            ImagePubId: dto.ImagePubId,
            PreferredCountryPubId: dto.PreferredCountryPubId,
            PreferredLanguagePubId: dto.PreferredLanguagePubId,
            Contact: dto.Contact,
            ManagerUserPubId: dto.ManagerUserPubId,
            UserPositionPubId: dto.UserPositionPubId,
            AccessProfilePubId: dto.AccessProfilePubId,
            VisualizationTempUnitPubId: dto.VisualizationTempUnitPubId,
            VisualizationRainUnitPubId: dto.VisualizationRainUnitPubId,
            VisualizationWindUnitPubId: dto.VisualizationWindUnitPubId,
            VisualizationGreenSpeedUnitPubId: dto.VisualizationGreenSpeedUnitPubId,
            VisualizationCuttingHeightUnitPubId: dto.VisualizationCuttingHeightUnitPubId,
            VisualizationClippingsUnitPubId: dto.VisualizationClippingsUnitPubId,
            VisualizationSurfaceUnitPubId: dto.VisualizationSurfaceUnitPubId
            )
        {
            RoleIds = [.. dto.Roles.Select(r => r.Id)],
            ManagerUserPubIds = dto.ManagerUserPubIds.Count > 0
                ? [.. dto.ManagerUserPubIds]
                : dto.ManagerUserPubId is Guid managerPubId && managerPubId != Guid.Empty
                    ? [managerPubId]
                    : []
        };

    public static UserProfileUpdateDto ToProfileUpdateDto(this UserDto dto)
        => new(
            PubId: dto.PubId,
            Email: dto.Email,
            UserName: dto.UserName,
            Name: dto.Name,
            ImagePubId: dto.ImagePubId,
            PreferredCountryPubId: dto.PreferredCountryPubId,
            PreferredLanguagePubId: dto.PreferredLanguagePubId,
            Contact: dto.Contact,
            VisualizationTempUnitPubId: dto.VisualizationTempUnitPubId,
            VisualizationRainUnitPubId: dto.VisualizationRainUnitPubId,
            VisualizationWindUnitPubId: dto.VisualizationWindUnitPubId,
            VisualizationGreenSpeedUnitPubId: dto.VisualizationGreenSpeedUnitPubId,
            VisualizationCuttingHeightUnitPubId: dto.VisualizationCuttingHeightUnitPubId,
            VisualizationClippingsUnitPubId: dto.VisualizationClippingsUnitPubId,
            VisualizationSurfaceUnitPubId: dto.VisualizationSurfaceUnitPubId
            );

    public static UserCreateDto ToCreateDto(this UserUpdateDto dto, Guid ibuPubId)
        => new(
            Email: dto.Email,
            UserName: dto.UserName,
            LockedOut: dto.LockedOut,
            IbuPubId: dto.IbuPubId != Guid.Empty ? dto.IbuPubId : ibuPubId,
            PreferredCountryPubId: dto.PreferredCountryPubId,
            PreferredLanguagePubId: dto.PreferredLanguagePubId,
            AppImageCreateDto: dto.AppImageCreateDto,
            ManagerUserPubId: dto.ManagerUserPubId,
            UserPositionPubId: dto.UserPositionPubId,
            AccessProfilePubId: dto.AccessProfilePubId,
            VisualizationTempUnitPubId: dto.VisualizationTempUnitPubId,
            VisualizationRainUnitPubId: dto.VisualizationRainUnitPubId,
            VisualizationWindUnitPubId: dto.VisualizationWindUnitPubId,
            VisualizationGreenSpeedUnitPubId: dto.VisualizationGreenSpeedUnitPubId,
            VisualizationCuttingHeightUnitPubId: dto.VisualizationCuttingHeightUnitPubId,
            VisualizationClippingsUnitPubId: dto.VisualizationClippingsUnitPubId,
            VisualizationSurfaceUnitPubId: dto.VisualizationSurfaceUnitPubId
        )
        {
            RoleIds = dto.RoleIds,
            ManagerUserPubIds = [.. dto.ManagerUserPubIds],
            DirectReportUserPubIds = [.. dto.DirectReportUserPubIds],
            ReplaceDirectReports = dto.ReplaceDirectReports
        };
}
