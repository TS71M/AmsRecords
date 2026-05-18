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

    public static AppImageDto ToAppImageDto(this AppImage appImage)
    {
        var image = ImageX.ImageToString(appImage.ImageFile);
        var imageInfo = AnalyzeBase64Image(image);

        return new AppImageDto(
            appImage.ImageId,
            image,
            imageInfo.MimeType,
            imageInfo.Format,
            imageInfo.Width,
            imageInfo.Height,
            imageInfo.FileSize,
            appImage.PubId,
            appImage.RelativePath
        );
    }

    public static GetAppImageWithThumbnailDto ToAppImageWithThumbnailsDto(this AppImage appImage)
    {
        var image = ImageX.ImageToString(appImage.ImageFile);
        var imageInfo = AnalyzeBase64Image(image);
        var thumbnail = appImage.ImageFile is { Length: > 0 }
            ? $"data:image/png;base64,{ImageX.GenerateThumbnailBase64(appImage.ImageFile)}"
            : image;

        return new GetAppImageWithThumbnailDto(
            appImage.ImageId,
            image,
            thumbnail,
            imageInfo.MimeType,
            imageInfo.Format,
            imageInfo.Width,
            imageInfo.Height,
            imageInfo.FileSize
        );
    }

    public static GetAppImageThumbnailDto ToAppImageThumbnailDto(this AppImage appImage)
    {
        var thumbnail = appImage.ImageFile is { Length: > 0 }
            ? $"data:image/png;base64,{ImageX.GenerateThumbnailBase64(appImage.ImageFile)}"
            : ImageX.ImageToString(appImage.ImageFile);
        var imageInfo = AnalyzeBase64Image(thumbnail);

        return new GetAppImageThumbnailDto(
            appImage.ImageId,
            appImage.ImageName,
            thumbnail,
            imageInfo.MimeType,
            imageInfo.Format,
            imageInfo.Width,
            imageInfo.Height,
            imageInfo.FileSize
        );
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
