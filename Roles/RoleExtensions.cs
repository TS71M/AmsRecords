using static AmsRecords.Roles.RoleDtos;

namespace AmsRecords.Roles;

public static class RoleExtensions
{
    public static RoleDto ToDto(this AmsModels.Role role)
        => new(
            Id: role.Id,
            Name: role.Name ?? string.Empty
        );

    public static AmsModels.Role ToEntity(this RoleCreateDto dto)
        => new(dto.Name);

    public static void UpdateEntity(this AmsModels.Role entity, RoleUpdateDto dto)
    {
        entity.Name = dto.Name;
        entity.NormalizedName = dto.Name.ToUpperInvariant();
    }

    public static RoleCreateDto ToCreateDto(this RoleUpdateDto dto)
        => new(dto.Name);

    public static RoleUpdateDto ToUpdateDto(this AmsModels.Role role)
    => new(
        Id: role.Id,
        Name: role.Name ?? string.Empty
    );

    public static RoleUpdateDto ToUpdateDto(this RoleDto role)
    => new(
        Id: role.Id,
        Name: role.Name ?? string.Empty
    );
}
