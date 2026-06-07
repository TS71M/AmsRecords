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
        )
        {
            CaptureMetadata = appImage.ToCaptureMetadataDto()
        };
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
        )
        {
            CaptureMetadata = appImage.ToCaptureMetadataDto()
        };
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
        )
        {
            CaptureMetadata = appImage.ToCaptureMetadataDto()
        };
    }

    public static AppImageCaptureMetadataDto? ToCaptureMetadataDto(this AppImage? appImage)
    {
        if (appImage is null
            || (appImage.CapturedAtUtc is null
                && appImage.CaptureLatitude is null
                && appImage.CaptureLongitude is null
                && appImage.CaptureLocationAccuracyMeters is null
                && string.IsNullOrWhiteSpace(appImage.CaptureMetadataSource)))
        {
            return null;
        }

        return new AppImageCaptureMetadataDto(
            appImage.CapturedAtUtc.HasValue
                ? new DateTimeOffset(DateTime.SpecifyKind(appImage.CapturedAtUtc.Value, DateTimeKind.Utc))
                : null,
            appImage.CaptureLatitude.HasValue ? (double)appImage.CaptureLatitude.Value : null,
            appImage.CaptureLongitude.HasValue ? (double)appImage.CaptureLongitude.Value : null,
            appImage.CaptureLocationAccuracyMeters.HasValue ? (double)appImage.CaptureLocationAccuracyMeters.Value : null,
            string.IsNullOrWhiteSpace(appImage.CaptureMetadataSource) ? null : appImage.CaptureMetadataSource.Trim());
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
