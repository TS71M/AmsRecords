namespace AmsRecords.WorkMessaging;

public static class WorkMessagingDtos
{
    public sealed record WorkConversationDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("contextType")] string? ContextType,
        [property: JsonPropertyName("contextPubId")] Guid? ContextPubId,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("lastMessagePreview")] string? LastMessagePreview,
        [property: JsonPropertyName("lastMessageUtc")] DateTime? LastMessageUtc,
        [property: JsonPropertyName("unreadCount")] int UnreadCount,
        [property: JsonPropertyName("participants")] IReadOnlyList<WorkConversationParticipantDto> Participants);

    public sealed record WorkConversationParticipantDto(
        [property: JsonPropertyName("userPubId")] Guid UserPubId,
        [property: JsonPropertyName("displayName")] string DisplayName,
        [property: JsonPropertyName("relationship")] string Relationship);

    public sealed record WorkMessageDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("conversationPubId")] Guid ConversationPubId,
        [property: JsonPropertyName("senderUserPubId")] Guid SenderUserPubId,
        [property: JsonPropertyName("senderName")] string SenderName,
        [property: JsonPropertyName("body")] string Body,
        [property: JsonPropertyName("createdUtc")] DateTime CreatedUtc,
        [property: JsonPropertyName("isMine")] bool IsMine,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId = null,
        [property: JsonPropertyName("imageThumbnailDataUrl")] string? ImageThumbnailDataUrl = null,
        [property: JsonPropertyName("originalBody")] string? OriginalBody = null,
        [property: JsonPropertyName("isTranslated")] bool IsTranslated = false,
        [property: JsonPropertyName("translationCultureCode")] string? TranslationCultureCode = null);

    public sealed record WorkConversationDetailDto(
        [property: JsonPropertyName("conversation")] WorkConversationDto Conversation,
        [property: JsonPropertyName("messages")] IReadOnlyList<WorkMessageDto> Messages);

    public sealed record WorkChatContactDto(
        [property: JsonPropertyName("userPubId")] Guid UserPubId,
        [property: JsonPropertyName("displayName")] string DisplayName,
        [property: JsonPropertyName("relationship")] string Relationship);

    public sealed record WorkFieldUnreadCountDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("unreadCount")] int UnreadCount);

    public sealed record WorkFieldChatTargetDto(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("participantCount")] int ParticipantCount,
        [property: JsonPropertyName("canStart")] bool CanStart,
        [property: JsonPropertyName("participantUserPubId")] Guid? ParticipantUserPubId = null,
        [property: JsonPropertyName("isGroup")] bool IsGroup = false);

    public sealed record WorkFieldMessagingWorkspaceDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("activeConversations")] IReadOnlyList<WorkConversationDto> ActiveConversations,
        [property: JsonPropertyName("archivedConversations")] IReadOnlyList<WorkConversationDto> ArchivedConversations,
        [property: JsonPropertyName("availableTargets")] IReadOnlyList<WorkFieldChatTargetDto> AvailableTargets,
        [property: JsonPropertyName("defaultConversationPubId")] Guid? DefaultConversationPubId);

    public sealed record WorkMessageCreateDto(
        [property: JsonPropertyName("body")] string? Body,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId = null);

    public sealed record WorkDirectConversationCreateDto(
        [property: JsonPropertyName("participantUserPubId")] Guid ParticipantUserPubId);
}
