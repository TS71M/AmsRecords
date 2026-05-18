namespace AmsRecords.RoleGroups;

public static class RoleGroupDtos
{
    public record RoleGroupDto(
        [property: JsonPropertyName("id")] int RoleGroupId,
        [property: JsonPropertyName("groupName")] string GroupName,
        [property: JsonPropertyName("roleIds")] List<int> RoleIds
    );

    public record RoleGroupCreateDto(
        [property: JsonPropertyName("groupName")] string GroupName,
        [property: JsonPropertyName("roleIds")] List<int> RoleIds
    );

    public record RoleGroupUpdateDto(
        [property: JsonPropertyName("id")] int RoleGroupId,
        [property: JsonPropertyName("groupName")] string GroupName
    )
    {
        [JsonPropertyName("roleIds")]
        public List<int> RoleIds { get; set; } = [];
    }
}