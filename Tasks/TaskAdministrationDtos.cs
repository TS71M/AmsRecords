namespace AmsRecords.Tasks;

public static class TaskAdministrationDtos
{
    public sealed record ComparableTaskDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("active")] bool Active);

    public sealed record TaskDefinitionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("standardDurationMinutes")] int StandardDurationMinutes,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("comparableTaskPubId")] Guid? ComparableTaskPubId,
        [property: JsonPropertyName("comparableTaskCode")] string? ComparableTaskCode,
        [property: JsonPropertyName("comparableTaskName")] string? ComparableTaskName,
        [property: JsonPropertyName("hasLegacyFieldLink")] bool HasLegacyFieldLink);

    public sealed record TaskAdministrationWorkspaceDto(
        [property: JsonPropertyName("tasks")] IReadOnlyList<TaskDefinitionDto> Tasks,
        [property: JsonPropertyName("comparableTasks")] IReadOnlyList<ComparableTaskDto> ComparableTasks,
        [property: JsonPropertyName("mappedTaskCount")] int MappedTaskCount,
        [property: JsonPropertyName("unmappedTaskCount")] int UnmappedTaskCount);

    public sealed record TaskDefinitionPageDto(IReadOnlyList<TaskDefinitionDto> Tasks, bool HasMore, int? NextOffset,
        bool PagingLimitReached);

    public sealed class TaskDefinitionSaveDto
    {
        public const int MaximumDurationMinutes = 1435;
        public const int DurationStepMinutes = 15;
        public static bool IsDurationOnStep(int minutes) => minutes % DurationStepMinutes == 0;

        [JsonPropertyName("name"), Required, StringLength(250, MinimumLength = 1)]
        public string Name { get; set; } = "";

        [JsonPropertyName("description"), StringLength(1000)]
        public string Description { get; set; } = "";

        [JsonPropertyName("standardDurationMinutes"), Range(0, MaximumDurationMinutes)]
        public int StandardDurationMinutes { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; } = true;
    }

    public sealed class TaskMappingUpdateDto
    {
        [JsonPropertyName("comparableTaskPubId")]
        public Guid? ComparableTaskPubId { get; set; }
    }
}
