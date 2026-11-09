using static AmsRecords.Roles.RoleDtos;

namespace AmsRecords.Roles;

public static class RoleExtensions
{

    public static RoleCreateDto ToCreateDto(this RoleUpdateDto dto)
        => new(dto.Name);

    public static RoleUpdateDto ToUpdateDto(this RoleDto role)
    => new(
        Id: role.Id,
        Name: role.Name ?? string.Empty
    );
}
