namespace AmsRecords.UserRoles;

public static class UserRoleDtos
{
    public record UserRoleDto(
        [property: JsonPropertyName("userId")] int UserId,
        [property: JsonPropertyName("roleId")] int RoleId,
        [property: JsonPropertyName("roleName")] string RoleName
    );

    public record UserRoleCreateDto(
        [property: JsonPropertyName("userId")] int UserId,
        [property: JsonPropertyName("roleId")] int RoleId
    );
}
