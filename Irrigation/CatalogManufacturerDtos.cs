using System.ComponentModel.DataAnnotations;

namespace AmsRecords.Irrigation;

public sealed record CatalogManufacturerOption(string Name, IReadOnlyList<string> Aliases,
    string? DisplayName = null, string? WebSite = null, string? LogoUrl = null)
{
    public string Label => string.IsNullOrWhiteSpace(DisplayName) || DisplayName == Name ? Name : $"{DisplayName} ({Name})";
}
public sealed record CatalogManufacturerEditDto(
    [param: Required, MaxLength(120)] string CanonicalName,
    [param: Required, MaxLength(120)] string DisplayName,
    [param: Required, Url, MaxLength(250)] string WebSite,
    [param: MaxLength(500)] string? LogoUrl,
    [param: Required, MaxLength(64)] string ExpectedVersion);
public sealed record CatalogBatchManufacturerDto([param: Required, MaxLength(120)] string ManufacturerName);
public sealed record CatalogBatchManufacturerResult(int UpdatedCandidates, int SkippedCandidates);
public sealed record CatalogManufacturerSaveDto(
    [param: Required, MaxLength(120)] string Name,
    [param: Required, Url, MaxLength(250)] string WebSite);
