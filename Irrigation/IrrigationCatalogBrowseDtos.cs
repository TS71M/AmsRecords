namespace AmsRecords.Irrigation;

public sealed record CatalogBrowseEntry(Guid PubId, string Name, string Description, string Kind,
    string Manufacturer = "", string? ImageUrl = null, bool Active = true);
public sealed record CatalogDocument(Guid PubId, string Title, string Number, string Revision, int? Page, Guid? BatchPubId, string SourceUrl);
public sealed record CatalogPerformance(decimal PressureBar, decimal FlowM3H, decimal RadiusM,
    decimal? TrajectoryDegrees, decimal? PrecipitationRateMmH, decimal? RotationSeconds, string Context, string Evidence, int SourcePage,
    Guid? OwnerPubId = null, string? OwnerKind = null, Guid? SourceDocumentPubId = null);
public sealed record CatalogDistributionPoint(decimal Distance, decimal Application);
public sealed record CatalogDistribution(decimal PressureBar, string Context, string Source, string Confidence,
    IReadOnlyList<CatalogDistributionPoint> Points);
public sealed record CatalogBrowsePage(string Title, string Manufacturer, Guid? ModelPubId,
    string Description, IReadOnlyList<CatalogBrowseEntry> Items, int TotalCount, int PageNumber,
    IReadOnlyList<CatalogDocument> Documents, IReadOnlyList<CatalogPerformance> Performance,
    IReadOnlyList<string> Images, Guid? EditPubId = null,
    IReadOnlyList<CatalogDistribution>? Distributions = null,
    IReadOnlyList<CatalogBrowseEntry>? RelatedSets = null,
    IReadOnlyList<CatalogSetColumn>? SetColumns = null);

public sealed record CatalogSetPart(Guid? PubId, string Kind, string PartNumber, string Name, string Color,
    string Role, Lib.Enums.IrrigationNozzlePositionKind? PositionKind, int? Position,
    decimal? InstallationAngle, bool Required, string? ImageUrl);

public sealed record CatalogSetColumn(Guid PubId, string Kind, string Name, string Code, string Generation,
    DateOnly? ValidFrom, DateOnly? ValidUntil, string Evidence, bool Active,
    IReadOnlyList<CatalogSetPart> Parts, CatalogDocument? Source);
