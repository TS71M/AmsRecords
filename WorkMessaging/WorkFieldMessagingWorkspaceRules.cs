using static AmsRecords.WorkMessaging.WorkMessagingDtos;

namespace AmsRecords.WorkMessaging;

public static class WorkFieldMessagingWorkspaceRules
{
    public static WorkFieldMessagingWorkspaceDto BuildWorkspace(
        Guid fieldPubId,
        IReadOnlyList<WorkConversationDto> conversations,
        IReadOnlyList<WorkFieldChatTargetDto> targets)
    {
        var fieldConversations = conversations
            .Where(x => IsConversationInFieldScope(x, fieldPubId))
            .ToList();
        var activeConversations = FilterConversationsToCurrentTargets(fieldConversations, targets);
        var activeConversationPubIds = activeConversations.Select(x => x.PubId).ToHashSet();
        var archivedConversations = SortArchivedConversations(fieldConversations
            .Where(x => !activeConversationPubIds.Contains(x.PubId))
            .ToList());
        var availableTargets = targets
            .Where(x => x.CanStart && !HasConversationForTarget(activeConversations, x))
            .ToList();

        return new WorkFieldMessagingWorkspaceDto(
            fieldPubId,
            activeConversations,
            archivedConversations,
            availableTargets,
            activeConversations.FirstOrDefault()?.PubId);
    }

    public static bool IsConversationInFieldScope(WorkConversationDto conversation, Guid fieldPubId)
        => string.Equals(conversation.ContextType, "field", StringComparison.OrdinalIgnoreCase)
           && conversation.ContextPubId == fieldPubId
           && IsFieldRoleConversation(conversation);

    public static bool IsFieldRoleConversation(WorkConversationDto conversation)
        => string.Equals(conversation.Type, "superior", StringComparison.OrdinalIgnoreCase)
           || string.Equals(conversation.Type, "subordinates", StringComparison.OrdinalIgnoreCase)
           || string.Equals(conversation.Type, "team", StringComparison.OrdinalIgnoreCase)
           || string.Equals(conversation.Type, "supervisor", StringComparison.OrdinalIgnoreCase)
           || string.Equals(conversation.Type, "teamleader", StringComparison.OrdinalIgnoreCase)
           || string.Equals(conversation.Type, "field", StringComparison.OrdinalIgnoreCase);

    public static List<WorkConversationDto> FilterConversationsToCurrentTargets(
        IReadOnlyList<WorkConversationDto> conversations,
        IReadOnlyList<WorkFieldChatTargetDto> targets)
    {
        var targetKeys = targets
            .Where(x => x.CanStart)
            .Select(TargetKey)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return conversations
            .Select(x => new { Conversation = x, Key = ConversationTargetKey(x) })
            .Where(x => !string.IsNullOrWhiteSpace(x.Key) && targetKeys.Contains(x.Key))
            .GroupBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
            .Select(x => x
                .OrderByDescending(item => item.Conversation.LastMessageUtc ?? DateTime.MinValue)
                .ThenBy(item => item.Conversation.PubId)
                .First()
                .Conversation)
            .OrderByDescending(x => x.LastMessageUtc ?? DateTime.MinValue)
            .ThenBy(x => x.Title)
            .ToList();
    }

    public static List<WorkConversationDto> SortArchivedConversations(IReadOnlyList<WorkConversationDto> conversations)
        => conversations
            .OrderByDescending(x => x.LastMessageUtc ?? DateTime.MinValue)
            .ThenBy(x => x.Title)
            .ToList();

    public static string TargetKey(WorkFieldChatTargetDto target)
        => target.ParticipantUserPubId.HasValue && target.ParticipantUserPubId.Value != Guid.Empty
            ? $"{target.Type}:{target.ParticipantUserPubId.Value:N}"
            : $"{target.Type}:group";

    public static string? ConversationTargetKey(WorkConversationDto conversation)
    {
        if (string.Equals(conversation.Type, "team", StringComparison.OrdinalIgnoreCase))
            return "team:group";

        if (!string.Equals(conversation.Type, "superior", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(conversation.Type, "subordinates", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var otherParticipantPubIds = conversation.Participants
            .Where(x => !string.Equals(x.DisplayName, "You", StringComparison.OrdinalIgnoreCase))
            .Select(x => x.UserPubId)
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToList();

        return otherParticipantPubIds.Count == 1
            ? $"{conversation.Type}:{otherParticipantPubIds[0]:N}"
            : $"{conversation.Type}:group";
    }

    public static string NormalizeTitle(string title)
    {
        title = title.Trim();
        const string chatSuffix = " chat";
        return title.EndsWith(chatSuffix, StringComparison.OrdinalIgnoreCase)
            ? title[..^chatSuffix.Length].Trim()
            : title;
    }

    static bool HasConversationForTarget(
        IReadOnlyList<WorkConversationDto> conversations,
        WorkFieldChatTargetDto target)
        => conversations.Any(x => string.Equals(ConversationTargetKey(x), TargetKey(target), StringComparison.OrdinalIgnoreCase));
}
