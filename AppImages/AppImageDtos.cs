namespace AmsRecords.AppImages;

public class AppImageDtos
{
    public record AppImageDto(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("base64Image")] string? Base64Image,
        [property: JsonPropertyName("mimeType")] string? MimeType,
        [property: JsonPropertyName("format")] string? Format,
        [property: JsonPropertyName("width")] uint Width,
        [property: JsonPropertyName("height")] uint Height,
        [property: JsonPropertyName("fileSize")] long FileSize,
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("relativePath")] string? RelativePath
        );

    public record GetAppImageWithThumbnailDto(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("base64Image")] string? Base64Image,
        [property: JsonPropertyName("base64Thumbnail")] string? Base64Thumbnail,
        [property: JsonPropertyName("mimeType")] string? MimeType,
        [property: JsonPropertyName("format")] string? Format,
        [property: JsonPropertyName("width")] uint Width,
        [property: JsonPropertyName("height")] uint Height,
        [property: JsonPropertyName("fileSize")] long FileSize
        )
    {
        [JsonIgnore]
        public string Base64ImageUrl =>
            string.IsNullOrWhiteSpace(Base64Image)
                ? string.Empty
                : $"data:{(string.IsNullOrWhiteSpace(MimeType) ? "image/webp" : MimeType)};base64,{Base64Image}";

        [JsonIgnore]
        public string ThumbnailDataUrl =>
            string.IsNullOrWhiteSpace(Base64Thumbnail)
                ? string.Empty
                : $"data:{(string.IsNullOrWhiteSpace(MimeType) ? "image/webp" : MimeType)};base64,{Base64Thumbnail}";
    }

    public record GetAppImageThumbnailDto(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("base64Thumbnail")] string? Base64Thumbnail,
        [property: JsonPropertyName("mimeType")] string? MimeType,
        [property: JsonPropertyName("format")] string? Format,
        [property: JsonPropertyName("width")] uint Width,
        [property: JsonPropertyName("height")] uint Height,
        [property: JsonPropertyName("fileSize")] long FileSize
        )
    {
        [JsonIgnore]
        public string ThumbnailDataUrl =>
            string.IsNullOrWhiteSpace(Base64Thumbnail)
                ? string.Empty
                : $"data:{(string.IsNullOrWhiteSpace(MimeType) ? "image/webp" : MimeType)};base64,{Base64Thumbnail}";
    }

    public record AppImageCreateDto(
        [property: JsonPropertyName("base64Image")] string? Base64Image = null,
        [property: JsonPropertyName("category")] string Category = ImageCategories.Grasses
        );

    public record AppImageInfoDto(
        [property: JsonPropertyName("mimeType")] string MimeType,
        [property: JsonPropertyName("format")] string Format,
        [property: JsonPropertyName("width")] uint Width,
        [property: JsonPropertyName("height")] uint Height,
        [property: JsonPropertyName("fileSize")] long FileSize
        );

    public sealed record AppImageRefDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("relativePath")] string? RelativePath
        );

    public sealed record AppImageCreatedDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("relativePath")] string? RelativePath,
        [property: JsonPropertyName("mimeType")] string? MimeType,
        [property: JsonPropertyName("format")] string? Format,
        [property: JsonPropertyName("width")] uint Width,
        [property: JsonPropertyName("height")] uint Height,
        [property: JsonPropertyName("fileSize")] long FileSize
        );

    public sealed record ImageThumbResult(string? Url, string? Error)
    {
        public bool HasImage => !string.IsNullOrWhiteSpace(Url);
    }
}
