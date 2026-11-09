using static AmsRecords.RoleGroups.RoleGroupDtos;

namespace AmsRecords.RoleGroups;

public static class RoleGroupExtensions
{

    public static RoleGroupUpdateDto ToUpdateDto(this RoleGroupDto dto)
        => new(
            RoleGroupId: dto.RoleGroupId,
            GroupName: dto.GroupName
        )
        {
            RoleIds = dto.RoleIds
        };
}