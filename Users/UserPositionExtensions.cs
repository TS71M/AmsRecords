using static AmsRecords.Users.UserPositionDtos;

namespace AmsRecords.Users;

public static class UserPositionExtensions
{
    public static UserPositionDto ToDto(this UserPosition position)
        => new(
            PubId: position.PubId,
            IbuPubId: position.Ibu.PubId,
            DepartmentPubId: position.Department.PubId,
            DepartmentName: position.Department.DepartmentName,
            FieldPubId: position.Field?.PubId,
            FieldName: position.Field?.FieldName ?? "",
            ParentPositionPubId: position.ParentUserPosition?.PubId,
            DefaultAccessProfilePubId: position.DefaultAccessProfile?.PubId,
            DefaultAccessProfileName: position.DefaultAccessProfile?.ProfileName ?? "",
            PositionName: position.UserPositionName,
            PositionCode: position.PositionCode,
            HierarchyLevel: position.HierarchyLevel,
            SortOrder: position.SortOrder,
            IsLeadership: position.IsLeadership,
            Active: position.Active,
            ColorHex: position.ColorHex);
}
