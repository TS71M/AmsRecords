using static AmsRecords.UserRoles.UserRoleDtos;

namespace AmsRecords.UserRoles;

public static class UserRoleExtensions
{
    public static UserRoleDto ToDto(this UserRole userRole)
        => new(
            UserId: userRole.UserId,
            RoleId: userRole.RoleId,
            RoleName: userRole.Role?.Name ?? string.Empty
        );

    public static UserRoleCreateDto ToCreateDto(this UserRoleDto dto)
        => new(dto.UserId, dto.RoleId);

    public static UserRole ToEntity(this UserRoleCreateDto dto, User user, AmsModels.Role role)
        => new()
        {
            UserId = dto.UserId,
            RoleId = dto.RoleId,
            User = user,
            Role = role
        };
}
