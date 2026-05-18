using static AmsRecords.SnagLists.SnagListDtos;

namespace AmsRecords.SnagLists;

public static class SnagListExtensions
{
    public static SnagListDto ToDto(this SnagList snag, bool canEdit = false, bool canEditAllValues = false, bool canDelete = false)
        => new(
            PubId: snag.PubId,
            FieldPubId: snag.Field.PubId,
            AreaPubId: snag.Area.PubId,
            HolePubId: snag.Hole.PubId,
            AssignedUserPubId: snag.AssignedUser?.PubId,
            CreatedByPubId: snag.User.PubId,
            StartImagePubId: snag.SnagImg?.PubId,
            CompletedImagePubId: snag.SnagCompletedImg?.PubId,
            FieldName: snag.Field.FieldName,
            AreaName: snag.Area.AreaName,
            HoleName: snag.Hole.HolDes,
            HoleNumber: snag.Hole.HoleNumber,
            AssignedUserName: ResolveAssignedUserName(snag),
            CreatedByName: snag.CreatedByName,
            Title: snag.Title,
            Description: snag.Description,
            SnagDate: snag.SnagDate,
            Longitude: snag.Longitude,
            Latitude: snag.Latitude,
            Priority: snag.Priority,
            Completed: snag.Completed,
            CompletionDate: snag.CompletionDate,
            CompletionRemarks: snag.CompletionRemarks,
            CompletionDurationMinutes: snag.CompletionDurationMinutes,
            AcceptDate: snag.AcceptDate,
            ReactionTimeText: snag.ReactionTimeTxt,
            ExecutionTimeText: snag.ExecutionTimeTxt,
            SolvingTimeText: snag.SolvingTimeTXt,
            CanEdit: canEdit,
            CanEditAllValues: canEditAllValues,
            CanDelete: canDelete);

    static string? ResolveAssignedUserName(SnagList snag)
    {
        if (!string.IsNullOrWhiteSpace(snag.AssignedUser?.FullNameSnapshot))
            return snag.AssignedUser.FullNameSnapshot;

        return snag.AssignedUser?.UserName;
    }
}
