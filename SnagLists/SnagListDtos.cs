namespace AmsRecords.SnagLists;

public static class SnagListDtos
{
    public static bool IsReviewStamp(string? value) => value is { Length: 64 } && value.All(Uri.IsHexDigit);

    public static string ReviewStamp(SnagListDto snag) => Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(
        System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new
        {
            snag.PubId, snag.FieldPubId, snag.AreaPubId, snag.HolePubId, snag.AssignedUserPubId,
            snag.FieldName, snag.AreaName, snag.HoleName, snag.HoleNumber,
            snag.Title, snag.Description, snag.Priority, snag.SnagDate, snag.Latitude, snag.Longitude,
            snag.AcceptDate, snag.Completed, snag.CompletionDate, snag.CompletionDurationMinutes,
            snag.CompletionRemarks, snag.StartImagePubId, snag.CompletedImagePubId, snag.CanEditAllValues
        })));

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
        [property: JsonPropertyName("priority")] int Priority,
        [property: JsonPropertyName("clientSubmissionId")] string? ClientSubmissionId = null);

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

    [method: JsonConstructor]
    public sealed record SnagListStatusDto(
        [property: JsonPropertyName("assignedUserPubId")] Guid? AssignedUserPubId,
        [property: JsonPropertyName("acceptDate")] DateTime? AcceptDate,
        [property: JsonPropertyName("completed")] bool Completed,
        [property: JsonPropertyName("completionDate")] DateTime? CompletionDate,
        [property: JsonPropertyName("completedImagePubId")] Guid? CompletedImagePubId,
        [property: JsonPropertyName("completionRemarks")] string? CompletionRemarks,
        [property: JsonPropertyName("completionDurationMinutes")] int? CompletionDurationMinutes,
        [property: JsonPropertyName("expectedReviewStamp"),
            JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? ExpectedReviewStamp = null)
    {
        public SnagListStatusDto(Guid? AssignedUserPubId, DateTime? AcceptDate, bool Completed, DateTime? CompletionDate,
            Guid? CompletedImagePubId, string? CompletionRemarks, int? CompletionDurationMinutes)
            : this(AssignedUserPubId, AcceptDate, Completed, CompletionDate, CompletedImagePubId, CompletionRemarks,
                CompletionDurationMinutes, null)
        {
        }

        public void Deconstruct(out Guid? AssignedUserPubId, out DateTime? AcceptDate, out bool Completed,
            out DateTime? CompletionDate, out Guid? CompletedImagePubId, out string? CompletionRemarks,
            out int? CompletionDurationMinutes)
            => (AssignedUserPubId, AcceptDate, Completed, CompletionDate, CompletedImagePubId, CompletionRemarks, CompletionDurationMinutes)
                = (this.AssignedUserPubId, this.AcceptDate, this.Completed, this.CompletionDate, this.CompletedImagePubId,
                    this.CompletionRemarks, this.CompletionDurationMinutes);
    }

    public sealed record SnagAssignableUserDto(
        [property: JsonPropertyName("userPubId")] Guid? UserPubId,
        [property: JsonPropertyName("name")] string Name);
}
