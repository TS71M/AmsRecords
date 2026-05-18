namespace AmsRecords.Roles;

public static class RoleDtos
{
    public record RoleDto(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("name")] string Name
    );

    public record RoleCreateDto(
        [property: JsonPropertyName("name")] string Name
    );

    public record RoleUpdateDto(
        [property: JsonPropertyName("id")] int? Id,
        [property: JsonPropertyName("name")] string Name
    )
    {
        public RoleUpdateDto() : this(0, string.Empty) { }
    }
}
