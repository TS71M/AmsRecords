using AmsRecords.SnagLists;

namespace AmsRecords.Assistant;

public sealed record AssistantSnagReportDto(string Title, string Description, Guid? HolePubId,
    Guid? AssignedUserPubId = null, int? Priority = null)
{
    public bool IsValid() => !string.IsNullOrWhiteSpace(Title) && Title.Length <= 45
        && !string.IsNullOrWhiteSpace(Description) && Description.Length <= 450
        && HolePubId != Guid.Empty && AssignedUserPubId != Guid.Empty
        && Priority is null or >= SnagListPriority.Low and <= SnagListPriority.Critical;
}

public sealed record AssistantSnagCompletionDto(Guid SnagPubId, string ReviewStamp, string? Remarks)
{
    public bool IsValid() => SnagPubId != Guid.Empty && SnagListDtos.IsReviewStamp(ReviewStamp) && Remarks?.Length is not > 500;
}
