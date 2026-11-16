namespace AmsRecords.Lynx;

public static class LynxUploadPolicy
{
    public const int MaximumFileMiB = 69;
    public const long MaximumFileBytes = MaximumFileMiB * 1024L * 1024;
    public const long MaximumRequestBytes = 70 * 1024L * 1024;
}
