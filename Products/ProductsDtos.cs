using System.ComponentModel;
using Lib.Constants;
using static AmsRecords.AppImages.AppImageDtos;

namespace AmsRecords.Products;

public static class ProductsDtos
{
    #region Products

    public record ProductMiniDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name
    );

    public record ProductListDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("accountName")] string AccountName,
        [property: JsonPropertyName("productTypeName")] string ProductTypeName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("packSize")] string PackSize,
        [property: JsonPropertyName("pricePerUnit")] string PricePerUnit,
        [property: JsonPropertyName("image")] string? Image
    );

    public sealed record ProductFormDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("accountPubId")] Guid AccountPubId,
        [property: JsonPropertyName("accountName")] string AccountName,
        [property: JsonPropertyName("productTypePubId")] Guid? ProductTypePubId,
        [property: JsonPropertyName("productTypeName")] string ProductTypeName,
        [property: JsonPropertyName("rate")] decimal Rate,
        [property: JsonPropertyName("recommendedRate")] decimal RecommendedRate,
        [property: JsonPropertyName("unitWeightPubId")] Guid UnitWeightPubId,
        [property: JsonPropertyName("unitSurfacePubId")] Guid UnitSurfacePubId,
        [property: JsonPropertyName("unitPackPubId")] Guid UnitPackPubId,
        [property: JsonPropertyName("unitWeightLabel")] string UnitWeightLabel,
        [property: JsonPropertyName("unitSurfaceLabel")] string UnitSurfaceLabel,
        [property: JsonPropertyName("unitPackLabel")] string UnitPackLabel,
        [property: JsonPropertyName("price")] decimal Price,
        [property: JsonPropertyName("pack")] decimal Pack,
        [property: JsonPropertyName("highlight")] bool Highlight,
        [property: JsonPropertyName("alarm")] decimal Alarm,
        [property: JsonPropertyName("isPgr")] bool IsPgr,
        [property: JsonPropertyName("image")] string? Image,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId,
        [property: JsonPropertyName("sdsName")] string SdsName,
        [property: JsonPropertyName("sdsPath")] string SdsPath,
        [property: JsonPropertyName("sdsUploadDt")] DateTime? SdsUploadDt,
        [property: JsonPropertyName("sdsSize")] int? SdsSize,
        [property: JsonPropertyName("nutrients")] List<ProductNutrientDto> Nutrients,
        [property: JsonPropertyName("supplements")] List<ProductSupplementDto> Supplements,
        [property: JsonPropertyName("labels")] List<ProductLabelDto> Labels
    );

    public sealed record ProductCreateDto(
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("accountPubId")] Guid AccountPubId,
        [property: JsonPropertyName("productTypePubId")] Guid? ProductTypePubId,
        [property: JsonPropertyName("name")]
        [property: DisplayName("Product Name")]
        [param: Required]
        [param: MaxLength(250)]
        string Name,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("rate")] decimal Rate,
        [property: JsonPropertyName("recommendedRate")] decimal RecommendedRate,
        [property: JsonPropertyName("unitWeightPubId")] Guid UnitWeightPubId,
        [property: JsonPropertyName("unitSurfacePubId")] Guid UnitSurfacePubId,
        [property: JsonPropertyName("price")] decimal Price,
        [property: JsonPropertyName("pack")] decimal Pack,
        [property: JsonPropertyName("unitPackPubId")] Guid UnitPackPubId,
        [property: JsonPropertyName("highlight")] bool Highlight,
        [property: JsonPropertyName("alarm")] decimal Alarm,
        [property: JsonPropertyName("isPgr")] bool IsPgr,
        [property: JsonPropertyName("imageSourceUrl")][param: MaxLength(1000)] string? ImageSourceUrl = null,
        [property: JsonPropertyName("sdsName")][param: MaxLength(250)] string? SdsName = null,
        [property: JsonPropertyName("sdsPath")][param: MaxLength(1000)] string? SdsPath = null,
        [property: JsonPropertyName("sdsSize")] int? SdsSize = null,
        [property: JsonPropertyName("sdsUploadDt")] DateTime? SdsUploadDt = null)
    {
        public AppImageCreateDto ImageCreateDto { get; set; } = new(null, ImageCategories.Products);
    }

    public sealed record ProductUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid? FieldPubId,
        [property: JsonPropertyName("accountPubId")] Guid AccountPubId,
        [property: JsonPropertyName("productTypePubId")] Guid? ProductTypePubId,
        [property: JsonPropertyName("name")]
        [property: DisplayName("Product Name")]
        [param: Required]
        [param: MaxLength(250)]
        string Name,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("rate")] decimal Rate,
        [property: JsonPropertyName("recommendedRate")] decimal RecommendedRate,
        [property: JsonPropertyName("unitWeightPubId")] Guid UnitWeightPubId,
        [property: JsonPropertyName("unitSurfacePubId")] Guid UnitSurfacePubId,
        [property: JsonPropertyName("price")] decimal Price,
        [property: JsonPropertyName("pack")] decimal Pack,
        [property: JsonPropertyName("unitPackPubId")] Guid UnitPackPubId,
        [property: JsonPropertyName("highlight")] bool Highlight,
        [property: JsonPropertyName("alarm")] decimal Alarm,
        [property: JsonPropertyName("isPgr")] bool IsPgr,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId = null,
        [property: JsonPropertyName("imageSourceUrl")][param: MaxLength(1000)] string? ImageSourceUrl = null,
        [property: JsonPropertyName("sdsName")][param: MaxLength(250)] string? SdsName = null,
        [property: JsonPropertyName("sdsPath")][param: MaxLength(1000)] string? SdsPath = null,
        [property: JsonPropertyName("sdsSize")] int? SdsSize = null,
        [property: JsonPropertyName("sdsUploadDt")] DateTime? SdsUploadDt = null)
    {
        public AppImageCreateDto ImageCreateDto { get; set; } = new(null, ImageCategories.Products);
    }

    public record ProductSupplierDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("productName")] string ProductName,
        [property: JsonPropertyName("supplierPubId")] Guid SupplierPubId,
        [property: JsonPropertyName("supplierName")] string SupplierName,
        [property: JsonPropertyName("supplierProductCode")] string SupplierProductCode,
        [property: JsonPropertyName("packSize")] decimal? PackSize,
        [property: JsonPropertyName("unitPrice")] decimal? UnitPrice,
        [property: JsonPropertyName("currencyCode")] string CurrencyCode,
        [property: JsonPropertyName("leadTimeDays")] int? LeadTimeDays,
        [property: JsonPropertyName("minimumOrderQty")] decimal? MinimumOrderQty,
        [property: JsonPropertyName("isPreferred")] bool IsPreferred,
        [property: JsonPropertyName("canRequestPrice")] bool CanRequestPrice,
        [property: JsonPropertyName("canOrder")] bool CanOrder,
        [property: JsonPropertyName("isActive")] bool IsActive,
        [property: JsonPropertyName("notes")] string Notes
    );

    public record ProductSupplierCreateDto(
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("supplierPubId")] Guid SupplierPubId,
        [property: JsonPropertyName("supplierProductCode")][MaxLength(100)] string SupplierProductCode,
        [property: JsonPropertyName("packSize")] decimal? PackSize,
        [property: JsonPropertyName("unitPrice")] decimal? UnitPrice,
        [property: JsonPropertyName("currencyCode")][MaxLength(3)] string CurrencyCode,
        [property: JsonPropertyName("leadTimeDays")] int? LeadTimeDays,
        [property: JsonPropertyName("minimumOrderQty")] decimal? MinimumOrderQty,
        [property: JsonPropertyName("isPreferred")] bool IsPreferred,
        [property: JsonPropertyName("canRequestPrice")] bool CanRequestPrice,
        [property: JsonPropertyName("canOrder")] bool CanOrder,
        [property: JsonPropertyName("isActive")] bool IsActive,
        [property: JsonPropertyName("notes")][MaxLength(500)] string Notes
    );

    public record ProductSupplierUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("supplierPubId")] Guid SupplierPubId,
        [property: JsonPropertyName("supplierProductCode")][MaxLength(100)] string SupplierProductCode,
        [property: JsonPropertyName("packSize")] decimal? PackSize,
        [property: JsonPropertyName("unitPrice")] decimal? UnitPrice,
        [property: JsonPropertyName("currencyCode")][MaxLength(3)] string CurrencyCode,
        [property: JsonPropertyName("leadTimeDays")] int? LeadTimeDays,
        [property: JsonPropertyName("minimumOrderQty")] decimal? MinimumOrderQty,
        [property: JsonPropertyName("isPreferred")] bool IsPreferred,
        [property: JsonPropertyName("canRequestPrice")] bool CanRequestPrice,
        [property: JsonPropertyName("canOrder")] bool CanOrder,
        [property: JsonPropertyName("isActive")] bool IsActive,
        [property: JsonPropertyName("notes")][MaxLength(500)] string Notes
    );

    #endregion

    #region Product Nutrients

    public record NutrientLookupDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("orderNumber")] int OrderNumber
    );

    public record ProductNutrientDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("nutrientPubId")] Guid NutrientPubId,
        [property: JsonPropertyName("nutrientName")] string NutrientName,
        [property: JsonPropertyName("amount")] decimal Amount
    );

    public record ProductNutrientCreateDto(
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("nutrientPubId")] Guid NutrientPubId,
        [property: JsonPropertyName("amount")] decimal Amount
    );

    public record ProductNutrientUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("nutrientPubId")] Guid NutrientPubId,
        [property: JsonPropertyName("amount")] decimal Amount
    );

    #endregion

    #region Product Supplements

    public record ProductSupplementDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("category")] string Category,
        [property: JsonPropertyName("amount")] decimal? Amount,
        [property: JsonPropertyName("unitText")] string UnitText,
        [property: JsonPropertyName("notes")] string Notes
    );

    public record ProductSupplementCreateDto(
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("name")][param: Required][param: MaxLength(150)] string Name,
        [property: JsonPropertyName("category")][param: MaxLength(50)] string? Category,
        [property: JsonPropertyName("amount")] decimal? Amount,
        [property: JsonPropertyName("unitText")][param: MaxLength(50)] string? UnitText,
        [property: JsonPropertyName("notes")][param: MaxLength(500)] string? Notes
    );

    public record ProductSupplementUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("name")][param: Required][param: MaxLength(150)] string Name,
        [property: JsonPropertyName("category")][param: MaxLength(50)] string? Category,
        [property: JsonPropertyName("amount")] decimal? Amount,
        [property: JsonPropertyName("unitText")][param: MaxLength(50)] string? UnitText,
        [property: JsonPropertyName("notes")][param: MaxLength(500)] string? Notes
    );

    #endregion

    #region Product Labels

    public record ProductLabelDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("path")] string Path,
        [property: JsonPropertyName("size")] int Size,
        [property: JsonPropertyName("uploadDt")] DateTime UploadDt,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId,
        [property: JsonPropertyName("image")] string? Image
    );

    public sealed record ProductLabelCreateDto(
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("name")][param: Required][param: MaxLength(250)] string Name,
        [property: JsonPropertyName("imageSourceUrl")][param: MaxLength(1000)] string? ImageSourceUrl = null)
    {
        public AppImageCreateDto ImageCreateDto { get; set; } = new(null, ImageCategories.Products);
    }

    public sealed record ProductLabelUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("name")][param: Required][param: MaxLength(250)] string Name,
        [property: JsonPropertyName("imagePubId")] Guid? ImagePubId = null,
        [property: JsonPropertyName("imageSourceUrl")][param: MaxLength(1000)] string? ImageSourceUrl = null)
    {
        public AppImageCreateDto ImageCreateDto { get; set; } = new(null, ImageCategories.Products);
    }

    #endregion

    #region Product Website Import

    public record ProductWebsiteImportRequestDto(
        [property: JsonPropertyName("websiteUrl")][MaxLength(1000)] string WebsiteUrl,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId
    );

    public record ProductWebsiteImportNutrientDto(
        [property: JsonPropertyName("nutrientPubId")] Guid? NutrientPubId,
        [property: JsonPropertyName("nutrientName")] string NutrientName,
        [property: JsonPropertyName("sourceName")] string SourceName,
        [property: JsonPropertyName("amount")] decimal Amount,
        [property: JsonPropertyName("isMatched")] bool IsMatched
    );

    public record ProductWebsiteImportSupplementDto(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("category")] string Category,
        [property: JsonPropertyName("amount")] decimal? Amount,
        [property: JsonPropertyName("unitText")] string UnitText,
        [property: JsonPropertyName("notes")] string Notes
    );

    public record ProductWebsiteImportLabelDto(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("isImage")] bool IsImage
    );

    public record ProductWebsiteImportPreviewDto(
        [property: JsonPropertyName("websiteUrl")] string WebsiteUrl,
        [property: JsonPropertyName("name")] string? Name,
        [property: JsonPropertyName("productTypePubId")] Guid? ProductTypePubId,
        [property: JsonPropertyName("productTypeName")] string? ProductTypeName,
        [property: JsonPropertyName("pack")] decimal? Pack,
        [property: JsonPropertyName("unitPackPubId")] Guid? UnitPackPubId,
        [property: JsonPropertyName("unitPackLabel")] string? UnitPackLabel,
        [property: JsonPropertyName("sdsName")] string? SdsName,
        [property: JsonPropertyName("sdsPath")] string? SdsPath,
        [property: JsonPropertyName("productImageUrl")] string? ProductImageUrl,
        [property: JsonPropertyName("nutrients")] List<ProductWebsiteImportNutrientDto> Nutrients,
        [property: JsonPropertyName("supplements")] List<ProductWebsiteImportSupplementDto> Supplements,
        [property: JsonPropertyName("labels")] List<ProductWebsiteImportLabelDto> Labels,
        [property: JsonPropertyName("sourceUrls")] List<string> SourceUrls,
        [property: JsonPropertyName("notes")] List<string> Notes
    );

    #endregion

    #region Product Types

    public record ProductTypeDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("productTypeActive")] bool IsActive,
        [property: JsonPropertyName("productTypeName")][MaxLength(45)] string Name
    );

    public record ProductTypeCreateDto(
        [property: JsonPropertyName("productTypeName")][MaxLength(45)] string Name
    );

    public record ProductTypeUpdateDto(
        [property: JsonPropertyName("publicId")] Guid PubId,
        [property: JsonPropertyName("productTypeActive")] bool IsActive,
        [property: JsonPropertyName("productTypeName")][MaxLength(45)] string Name
    );

    #endregion
}
