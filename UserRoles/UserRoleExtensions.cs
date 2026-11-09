using static AmsRecords.UserRoles.UserRoleDtos;

namespace AmsRecords.UserRoles;

public static class UserRoleExtensions
{

    public static UserRoleCreateDto ToCreateDto(this UserRoleDto dto)
        => new(dto.UserId, dto.RoleId);
}
