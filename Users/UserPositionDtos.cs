namespace AmsRecords.Users;

public static class UserPositionDtos
{
    public sealed record UserPositionDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("departmentPubId")] Guid DepartmentPubId,
        [property: JsonPropertyName("departmentName")] string DepartmentName,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("parentPositionPubId")] Guid? ParentPositionPubId,
        [property: JsonPropertyName("defaultAccessProfilePubId")] Guid? DefaultAccessProfilePubId,
        [property: JsonPropertyName("defaultAccessProfileName")] string DefaultAccessProfileName,
        [property: JsonPropertyName("positionName")] string PositionName,
        [property: JsonPropertyName("positionCode")] string PositionCode,
        [property: JsonPropertyName("hierarchyLevel")] int HierarchyLevel,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("isLeadership")] bool IsLeadership,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("colorHex")] string ColorHex);

    public sealed record UserPositionCreateDto(
        [property: JsonPropertyName("departmentPubId")][Required] Guid DepartmentPubId,
        [property: JsonPropertyName("parentPositionPubId")] Guid? ParentPositionPubId,
        [property: JsonPropertyName("defaultAccessProfilePubId")] Guid? DefaultAccessProfilePubId,
        [property: JsonPropertyName("positionName")][Required] string PositionName,
        [property: JsonPropertyName("positionCode")] string PositionCode,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("isLeadership")] bool IsLeadership,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("colorHex")] string ColorHex);

    public sealed record UserPositionUpdateDto(
        [property: JsonPropertyName("publicId")][Required] Guid PubId,
        [property: JsonPropertyName("departmentPubId")][Required] Guid DepartmentPubId,
        [property: JsonPropertyName("parentPositionPubId")] Guid? ParentPositionPubId,
        [property: JsonPropertyName("defaultAccessProfilePubId")] Guid? DefaultAccessProfilePubId,
        [property: JsonPropertyName("positionName")][Required] string PositionName,
        [property: JsonPropertyName("positionCode")] string PositionCode,
        [property: JsonPropertyName("sortOrder")] int SortOrder,
        [property: JsonPropertyName("isLeadership")] bool IsLeadership,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("colorHex")] string ColorHex);
}
