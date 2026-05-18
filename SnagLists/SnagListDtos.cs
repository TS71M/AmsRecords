namespace AmsRecords.SnagLists;

public static class SnagListDtos
{
    public sealed record SnagListDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("assignedUserPubId")] Guid? AssignedUserPubId,
        [property: JsonPropertyName("createdByPubId")] Guid CreatedByPubId,
        [property: JsonPropertyName("startImagePubId")] Guid? StartImagePubId,
        [property: JsonPropertyName("completedImagePubId")] Guid? CompletedImagePubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("areaName")] string AreaName,
        [property: JsonPropertyName("holeName")] string HoleName,
        [property: JsonPropertyName("holeNumber")] int HoleNumber,
        [property: JsonPropertyName("assignedUserName")] string? AssignedUserName,
        [property: JsonPropertyName("createdByName")] string CreatedByName,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("snagDate")] DateTime SnagDate,
        [property: JsonPropertyName("longitude")] double Longitude,
        [property: JsonPropertyName("latitude")] double Latitude,
        [property: JsonPropertyName("priority")] int Priority,
        [property: JsonPropertyName("completed")] bool Completed,
        [property: JsonPropertyName("completionDate")] DateTime? CompletionDate,
        [property: JsonPropertyName("completionRemarks")] string? CompletionRemarks,
        [property: JsonPropertyName("completionDurationMinutes")] int? CompletionDurationMinutes,
        [property: JsonPropertyName("acceptDate")] DateTime? AcceptDate,
        [property: JsonPropertyName("reactionTimeText")] string ReactionTimeText,
        [property: JsonPropertyName("executionTimeText")] string ExecutionTimeText,
        [property: JsonPropertyName("solvingTimeText")] string SolvingTimeText,
        [property: JsonPropertyName("canEdit")] bool CanEdit,
        [property: JsonPropertyName("canEditAllValues")] bool CanEditAllValues,
        [property: JsonPropertyName("canDelete")] bool CanDelete);

    public sealed record SnagListCreateDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("assignedUserPubId")] Guid? AssignedUserPubId,
        [property: JsonPropertyName("startImagePubId")] Guid? StartImagePubId,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("snagDate")] DateTime? SnagDate,
        [property: JsonPropertyName("longitude")] double Longitude,
        [property: JsonPropertyName("latitude")] double Latitude,
        [property: JsonPropertyName("priority")] int Priority);

    public sealed record SnagListUpdateDto(
        [property: JsonPropertyName("areaPubId")] Guid AreaPubId,
        [property: JsonPropertyName("holePubId")] Guid HolePubId,
        [property: JsonPropertyName("assignedUserPubId")] Guid? AssignedUserPubId,
        [property: JsonPropertyName("startImagePubId")] Guid? StartImagePubId,
        [property: JsonPropertyName("completedImagePubId")] Guid? CompletedImagePubId,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("snagDate")] DateTime SnagDate,
        [property: JsonPropertyName("longitude")] double Longitude,
        [property: JsonPropertyName("latitude")] double Latitude,
        [property: JsonPropertyName("priority")] int Priority,
        [property: JsonPropertyName("completed")] bool Completed,
        [property: JsonPropertyName("completionDate")] DateTime? CompletionDate,
        [property: JsonPropertyName("completionRemarks")] string? CompletionRemarks,
        [property: JsonPropertyName("completionDurationMinutes")] int? CompletionDurationMinutes,
        [property: JsonPropertyName("acceptDate")] DateTime? AcceptDate);

    public sealed record SnagListStatusDto(
        [property: JsonPropertyName("assignedUserPubId")] Guid? AssignedUserPubId,
        [property: JsonPropertyName("acceptDate")] DateTime? AcceptDate,
        [property: JsonPropertyName("completed")] bool Completed,
        [property: JsonPropertyName("completionDate")] DateTime? CompletionDate,
        [property: JsonPropertyName("completedImagePubId")] Guid? CompletedImagePubId,
        [property: JsonPropertyName("completionRemarks")] string? CompletionRemarks,
        [property: JsonPropertyName("completionDurationMinutes")] int? CompletionDurationMinutes);

    public sealed record SnagAssignableUserDto(
        [property: JsonPropertyName("userPubId")] Guid? UserPubId,
        [property: JsonPropertyName("name")] string Name);
}
