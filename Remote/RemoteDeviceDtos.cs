using static Lib.Enums.RemoteAccess;

namespace AmsRecords.Remote;

public static class RemoteDeviceDtos
{
    public sealed record RemoteDeviceEnrollmentCreateDto(Guid? FieldPubId = null);

    public sealed record RemoteDeviceEnrollmentTicketDto(
        Guid PubId,
        string EnrollmentCode,
        DateTime ExpiresAtUtc);

    public sealed record RemoteDeviceManagementAccessDto(bool CanManageDevices);

    public sealed record RemoteDeviceUnattendedAccessUpdateDto(bool Enabled);

    public sealed record RemoteViewerAccessTokenDto(
        string AccessToken,
        DateTime ExpiresAtUtc);

    public sealed record RemoteDeviceListDto(
        Guid PubId,
        string DisplayName,
        string MachineName,
        string Platform,
        string OperatingSystem,
        string? OperatingSystemVersion,
        string AgentVersion,
        string OrganisationName,
        Guid? FieldPubId,
        string? FieldName,
        DeviceAvailability Availability,
        DateTime? LastSeenAtUtc,
        bool CanControl,
        bool CanClipboard,
        bool CanUnattended);

    public sealed record RemoteDeviceDetailDto(
        Guid PubId,
        string DisplayName,
        string MachineName,
        string Platform,
        string OperatingSystem,
        string? OperatingSystemVersion,
        string AgentVersion,
        string OrganisationName,
        Guid? FieldPubId,
        string? FieldName,
        DeviceAvailability Availability,
        DeviceAccessMode AccessMode,
        DateTime CreatedAtUtc,
        DateTime? LastSeenAtUtc,
        bool CanView,
        bool CanControl,
        bool CanClipboard,
        bool CanUnattended);
}
