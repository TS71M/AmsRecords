namespace AmsRecords.Products;

public sealed record ProductCreationChoiceDto(Guid Id, string Name);
public sealed record ProductCreationUnitDto(Guid Id, string Name, string? TypeName);
public sealed record ProductCreationSetupDto(Guid IbuPubId, string IbuName,
    IReadOnlyList<ProductCreationChoiceDto> Accounts, IReadOnlyList<ProductCreationChoiceDto> ProductTypes,
    IReadOnlyList<ProductCreationUnitDto> Units, IReadOnlyList<ProductCreationChoiceDto> Fields);
