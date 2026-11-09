using ImageMagick;
using Lib.Images;
using static AmsRecords.AppImages.AppImageDtos;

namespace AmsRecords.AppImages;

public static class AppImageExtensions
{
    static string StripDataUrlPrefix(string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
            return string.Empty;

        var commaIndex = data.IndexOf(',');
        return commaIndex >= 0 ? data[(commaIndex + 1)..] : data;
    }

    public static AppImageInfoDto AnalyzeBase64Image(this string base64Image)
    {
        byte[] bytes = Convert.FromBase64String(StripDataUrlPrefix(base64Image));

        using var image = new MagickImage(bytes);
        var format = image.Format;
        var formatInfo = MagickFormatInfo.Create(format);

        return new AppImageInfoDto(
            formatInfo?.MimeType ?? "application/octet-stream",
            format.ToString(),
            image.Width,
            image.Height,
            bytes.Length
        );
    }

    public static AppImageCreateDto NormalizeImageDto(this AppImageCreateDto? dto, string imageCategorie)
        => new(
            Base64Image: dto?.Base64Image,
            Category: imageCategorie
        );
}
