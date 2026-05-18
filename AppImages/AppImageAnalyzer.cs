using ImageMagick;
using static AmsRecords.AppImages.AppImageDtos;

namespace AmsRecords.AppImages;

public static class AppImageAnalyzer
{
    public static AppImageInfoDto AnalyzeBase64Image(string base64Image)
    {
        byte[] bytes = Convert.FromBase64String(base64Image);

        using var image = new MagickImage(bytes);

        var format = image.Format;
        var info = MagickFormatInfo.Create(format);

        return new AppImageInfoDto(
            info?.MimeType ?? "application/octet-stream",
            format.ToString(),
            image.Width,
            image.Height,
            bytes.Length
        );
    }
}