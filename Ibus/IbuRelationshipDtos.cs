namespace AmsRecords.Ibus;

public static class IbuRelationshipDtos
{
    public sealed record ConsultantAccessMatrixDto(
        [property: JsonPropertyName("clientIbuPubId")] Guid ClientIbuPubId,
        [property: JsonPropertyName("clientIbuName")] string ClientIbuName,
        [property: JsonPropertyName("fields")] IReadOnlyList<ConsultantAccessFieldItemDto> Fields,
        [property: JsonPropertyName("consultants")] IReadOnlyList<ConsultantAccessRowDto> Consultants
    );

    public sealed record ConsultantManagedFieldsDto(
        [property: JsonPropertyName("consultantIbuPubId")] Guid ConsultantIbuPubId,
        [property: JsonPropertyName("consultantIbuName")] string ConsultantIbuName,
        [property: JsonPropertyName("fields")] IReadOnlyList<ConsultantManagedFieldItemDto> Fields
    );

    public sealed record ConsultantAccessFieldItemDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record ConsultantManagedFieldItemDto(
        [property: JsonPropertyName("clientIbuPubId")] Guid ClientIbuPubId,
        [property: JsonPropertyName("clientIbuName")] string ClientIbuName,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("fieldTypeName")] string FieldTypeName,
        [property: JsonPropertyName("active")] bool Active
    );

    public sealed record ConsultantAccessRowDto(
        [property: JsonPropertyName("consultantIbuPubId")] Guid ConsultantIbuPubId,
        [property: JsonPropertyName("consultantIbuName")] string ConsultantIbuName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("scope")] IbuRelationshipFieldScopes.IbuRelationshipFieldScope Scope,
        [property: JsonPropertyName("fieldPubIds")] IReadOnlyList<Guid> FieldPubIds
    );

    public sealed record SaveConsultantAccessMatrixDto(
        [property: JsonPropertyName("clientIbuPubId")] Guid ClientIbuPubId,
        [property: JsonPropertyName("consultants")] IReadOnlyList<SaveConsultantAccessRowDto> Consultants
    );

    public sealed record SaveConsultantAccessRowDto(
        [property: JsonPropertyName("consultantIbuPubId")] Guid ConsultantIbuPubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("scope")] IbuRelationshipFieldScopes.IbuRelationshipFieldScope Scope,
        [property: JsonPropertyName("fieldPubIds")] IReadOnlyList<Guid> FieldPubIds
    );

    public sealed record ExtractFieldToNewIbuDto(
        [property: JsonPropertyName("sourceIbuPubId")] Guid SourceIbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("targetMode")] FieldExtractionTargetModes.FieldExtractionTargetMode TargetMode,
        [property: JsonPropertyName("newIbuName")] string? NewIbuName,
        [property: JsonPropertyName("existingTargetIbuPubId")] Guid? ExistingTargetIbuPubId,
        [property: JsonPropertyName("websiteHint")] string? WebsiteHint
    );

    public sealed record ExtractFieldToNewIbuResultDto(
        [property: JsonPropertyName("sourceIbuPubId")] Guid SourceIbuPubId,
        [property: JsonPropertyName("sourceIbuName")] string SourceIbuName,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("targetIbuPubId")] Guid TargetIbuPubId,
        [property: JsonPropertyName("targetIbuName")] string TargetIbuName,
        [property: JsonPropertyName("createdNewIbu")] bool CreatedNewIbu,
        [property: JsonPropertyName("relationshipPubId")] Guid RelationshipPubId
    );
}
