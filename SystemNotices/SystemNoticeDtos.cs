namespace AmsRecords.SystemNotices;

public static class SystemNoticeDtos
{
    public sealed record SystemNoticeDto(
        Guid PubId,
        string Title,
        string Message,
        SystemNoticeDisplayMode DisplayMode,
        DateTime StartsAtUtc,
        DateTime? ExpiresAtUtc,
        bool IsActive,
        DateTime CreatedAtUtc,
        string CreatedBy,
        int AcknowledgedUserCount);

    public sealed record PendingSystemNoticeDto(
        Guid PubId,
        string Title,
        string Message,
        SystemNoticeDisplayMode DisplayMode,
        DateTime? ExpiresAtUtc);

    public sealed record CreateSystemNoticeDto(
        string Title,
        string Message,
        SystemNoticeDisplayMode DisplayMode,
        DateTime? StartsAtUtc,
        DateTime? ExpiresAtUtc);

    public sealed record SetSystemNoticeActiveDto(bool IsActive);
}
