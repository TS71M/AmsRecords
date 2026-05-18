using static AmsRecords.Departments.DepartmentDtos;

namespace AmsRecords.Departments;

public static class DepartmentExtensions
{
    public static DepartmentDto ToDto(this Department department)
        => new(
            PubId: department.PubId,
            IbuPubId: department.Ibu.PubId,
            IbuName: department.Ibu.BusinessUnitName,
            FieldPubId: department.Field?.PubId,
            FieldName: department.Field?.FieldName ?? "",
            Scope: department.FieldId is null ? "ibu" : "field",
            DepartmentName: department.DepartmentName,
            Description: department.Description
        );

    public static Department ToEntity(this DepartmentCreateDto dto, Ibu ibu, Field? field)
        => new()
        {
            IbuId = ibu.IbuId,
            FieldId = field?.FieldId,
            DepartmentName = dto.DepartmentName,
            Description = dto.Description,
            Ibu = ibu,
            Field = field
        };

    public static void UpdateEntity(this Department entity, DepartmentUpdateDto dto, Ibu ibu, Field? field)
    {
        entity.DepartmentName = dto.DepartmentName;
        entity.Description = dto.Description;
        entity.IbuId = ibu.IbuId;
        entity.FieldId = field?.FieldId;
        entity.Ibu = ibu;
        entity.Field = field;
    }
}
