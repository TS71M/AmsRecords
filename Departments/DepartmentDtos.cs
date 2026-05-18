namespace AmsRecords.Departments;

public static class DepartmentDtos
{
    public record DepartmentDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("ibuName")] string IbuName,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("scope")] string Scope,
        [property: JsonPropertyName("departmentName")] string DepartmentName,
        [property: JsonPropertyName("description")] string Description
    );
    public record DepartmentCreateDto(
        [property: JsonPropertyName("ibuPubId")] Guid? IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("departmentName")][Required] string DepartmentName,
        [property: JsonPropertyName("description")] string Description
    );
    public record DepartmentUpdateDto(
        [property: JsonPropertyName("publicId")][Required] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid? IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("departmentName")][Required] string DepartmentName,
        [property: JsonPropertyName("description")] string Description
    );
}
