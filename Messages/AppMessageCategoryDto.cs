namespace AmsRecords.Messages;

public sealed record AppMessageCategoryDto(
    [property: JsonPropertyName("category")] string Category,
    [property: JsonPropertyName("messages")] AppMessageDto[] Messages
);