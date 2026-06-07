using System.ComponentModel;
using Lib.Enums;

namespace AmsRecords.Procural;

public static class ProcurementDtos
{
    public record ProcurementScopeIbuDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("isFieldOwner")] bool IsFieldOwner
    );

    public record FieldProcurementSettingDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("procurementMode")] ProcurementMode ProcurementMode,
        [property: JsonPropertyName("procurementHubIbuPubId")] Guid? ProcurementHubIbuPubId,
        [property: JsonPropertyName("procurementHubIbuName")] string ProcurementHubIbuName,
        [property: JsonPropertyName("procurementSupplierPubId")] Guid? ProcurementSupplierPubId,
        [property: JsonPropertyName("procurementSupplierName")] string ProcurementSupplierName,
        [property: JsonPropertyName("procurementManagerUserPubId")] Guid? ProcurementManagerUserPubId,
        [property: JsonPropertyName("procurementManagerName")] string ProcurementManagerName,
        [property: JsonPropertyName("allowDirectSupplierOrdering")] bool AllowDirectSupplierOrdering,
        [property: JsonPropertyName("consolidateRequisitions")] bool ConsolidateRequisitions,
        [property: JsonPropertyName("notes")] string Notes,
        [property: JsonPropertyName("procurementScopeIbuPubIds")] IReadOnlyList<Guid> ProcurementScopeIbuPubIds,
        [property: JsonPropertyName("procurementScopeIbus")] IReadOnlyList<ProcurementScopeIbuDto> ProcurementScopeIbus
    );

    public record FieldProcurementSettingUpdateDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("procurementMode")]
        [property: DisplayName("Procurement Mode")]
        ProcurementMode ProcurementMode,
        [property: JsonPropertyName("procurementHubIbuPubId")] Guid? ProcurementHubIbuPubId,
        [property: JsonPropertyName("procurementSupplierPubId")] Guid? ProcurementSupplierPubId,
        [property: JsonPropertyName("procurementManagerUserPubId")] Guid? ProcurementManagerUserPubId,
        [property: JsonPropertyName("allowDirectSupplierOrdering")] bool AllowDirectSupplierOrdering,
        [property: JsonPropertyName("consolidateRequisitions")] bool ConsolidateRequisitions,
        [property: JsonPropertyName("notes")]
        [property: MaxLength(500)]
        string? Notes
    );

    public record PurchaseRequisitionLineDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("productName")] string ProductName,
        [property: JsonPropertyName("preferredProductSupplierPubId")] Guid? PreferredProductSupplierPubId,
        [property: JsonPropertyName("preferredSupplierName")] string PreferredSupplierName,
        [property: JsonPropertyName("quantity")] decimal Quantity,
        [property: JsonPropertyName("quantityLabel")] string QuantityLabel,
        [property: JsonPropertyName("notes")] string Notes
    );

    public record PurchaseRequisitionLineInputDto(
        [property: JsonPropertyName("productPubId")] Guid ProductPubId,
        [property: JsonPropertyName("preferredProductSupplierPubId")] Guid? PreferredProductSupplierPubId,
        [property: JsonPropertyName("quantity")] decimal Quantity,
        [property: JsonPropertyName("notes")]
        [property: MaxLength(500)]
        string? Notes
    );

    public record PurchaseRequisitionListDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("referenceNo")] string ReferenceNo,
        [property: JsonPropertyName("status")] PurchaseRequisitionStatus Status,
        [property: JsonPropertyName("requestedDt")] DateTime RequestedDt,
        [property: JsonPropertyName("needByDate")] DateTime? NeedByDate,
        [property: JsonPropertyName("requestedByUserPubId")] Guid RequestedByUserPubId,
        [property: JsonPropertyName("requestedByName")] string RequestedByName,
        [property: JsonPropertyName("procurementHubIbuPubId")] Guid? ProcurementHubIbuPubId,
        [property: JsonPropertyName("procurementHubIbuName")] string ProcurementHubIbuName,
        [property: JsonPropertyName("procurementManagerUserPubId")] Guid? ProcurementManagerUserPubId,
        [property: JsonPropertyName("procurementManagerName")] string ProcurementManagerName,
        [property: JsonPropertyName("lineCount")] int LineCount,
        [property: JsonPropertyName("totalQuantity")] decimal TotalQuantity
    );

    public record PurchaseRequisitionDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("ibuPubId")] Guid IbuPubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("referenceNo")] string ReferenceNo,
        [property: JsonPropertyName("status")] PurchaseRequisitionStatus Status,
        [property: JsonPropertyName("requestedDt")] DateTime RequestedDt,
        [property: JsonPropertyName("needByDate")] DateTime? NeedByDate,
        [property: JsonPropertyName("reviewedDt")] DateTime? ReviewedDt,
        [property: JsonPropertyName("requestedByUserPubId")] Guid RequestedByUserPubId,
        [property: JsonPropertyName("requestedByName")] string RequestedByName,
        [property: JsonPropertyName("procurementHubIbuPubId")] Guid? ProcurementHubIbuPubId,
        [property: JsonPropertyName("procurementHubIbuName")] string ProcurementHubIbuName,
        [property: JsonPropertyName("procurementManagerUserPubId")] Guid? ProcurementManagerUserPubId,
        [property: JsonPropertyName("procurementManagerName")] string ProcurementManagerName,
        [property: JsonPropertyName("notes")] string Notes,
        [property: JsonPropertyName("decisionNotes")] string DecisionNotes,
        [property: JsonPropertyName("lines")] List<PurchaseRequisitionLineDto> Lines
    );

    public record PurchaseRequisitionCreateDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("title")]
        [property: MaxLength(250)]
        string? Title,
        [property: JsonPropertyName("referenceNo")]
        [property: MaxLength(100)]
        string? ReferenceNo,
        [property: JsonPropertyName("needByDate")] DateTime? NeedByDate,
        [property: JsonPropertyName("notes")]
        [property: MaxLength(1000)]
        string? Notes,
        [property: JsonPropertyName("lines")] List<PurchaseRequisitionLineInputDto> Lines,
        [property: JsonPropertyName("submitForProcessing")] bool SubmitForProcessing = false
    );

    public record PurchaseRequisitionUpdateDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("title")]
        [property: MaxLength(250)]
        string? Title,
        [property: JsonPropertyName("referenceNo")]
        [property: MaxLength(100)]
        string? ReferenceNo,
        [property: JsonPropertyName("needByDate")] DateTime? NeedByDate,
        [property: JsonPropertyName("notes")]
        [property: MaxLength(1000)]
        string? Notes,
        [property: JsonPropertyName("lines")] List<PurchaseRequisitionLineInputDto> Lines,
        [property: JsonPropertyName("submitForProcessing")] bool SubmitForProcessing = false
    );

    public record PurchaseRequisitionStatusUpdateDto(
        [property: JsonPropertyName("status")] PurchaseRequisitionStatus Status,
        [property: JsonPropertyName("decisionNotes")]
        [property: MaxLength(1000)]
        string? DecisionNotes,
        [property: JsonPropertyName("procurementManagerUserPubId")] Guid? ProcurementManagerUserPubId
    );
}
