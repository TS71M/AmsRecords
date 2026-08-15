using System.ComponentModel;
using Lib.Enums;

namespace AmsRecords.Procural;

public static class ProcurementDtos
{
    public record ProcurementHubMemberDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("userPubId")] Guid UserPubId,
        [property: JsonPropertyName("userName")] string UserName,
        [property: JsonPropertyName("userEmail")] string UserEmail,
        [property: JsonPropertyName("role")] string Role,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary
    );

    public record ProcurementHubSettingDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("hubIbuPubId")] Guid HubIbuPubId,
        [property: JsonPropertyName("hubIbuName")] string HubIbuName,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("allowDirectSupplierOrderingDefault")] bool AllowDirectSupplierOrderingDefault,
        [property: JsonPropertyName("consolidateRequisitionsDefault")] bool ConsolidateRequisitionsDefault,
        [property: JsonPropertyName("notes")] string Notes,
        [property: JsonPropertyName("members")] IReadOnlyList<ProcurementHubMemberDto> Members,
        [property: JsonPropertyName("requireApprovalDefault")] bool RequireApprovalDefault = true,
        [property: JsonPropertyName("allowSelfApprovalDefault")] bool AllowSelfApprovalDefault = false,
        [property: JsonPropertyName("requireRequestForQuoteDefault")] bool RequireRequestForQuoteDefault = false
    );

    public record ProcurementHubMemberUpdateDto(
        [property: JsonPropertyName("userPubId")] Guid UserPubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("isPrimary")] bool IsPrimary,
        [property: JsonPropertyName("role")]
        [param: MaxLength(100)]
        string? Role
    );

    public record ProcurementHubSettingUpdateDto(
        [property: JsonPropertyName("hubIbuPubId")] Guid HubIbuPubId,
        [property: JsonPropertyName("active")] bool Active,
        [property: JsonPropertyName("allowDirectSupplierOrderingDefault")] bool AllowDirectSupplierOrderingDefault,
        [property: JsonPropertyName("consolidateRequisitionsDefault")] bool ConsolidateRequisitionsDefault,
        [property: JsonPropertyName("notes")]
        [param: MaxLength(500)]
        string? Notes,
        [property: JsonPropertyName("members")] IReadOnlyList<ProcurementHubMemberUpdateDto> Members,
        [property: JsonPropertyName("requireApprovalDefault")] bool RequireApprovalDefault = true,
        [property: JsonPropertyName("allowSelfApprovalDefault")] bool AllowSelfApprovalDefault = false,
        [property: JsonPropertyName("requireRequestForQuoteDefault")] bool RequireRequestForQuoteDefault = false
    );

    public record ProcurementScopeIbuDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("isFieldOwner")] bool IsFieldOwner,
        [property: JsonPropertyName("isActiveProcurementHub")] bool IsActiveProcurementHub = false
    );

    public record ProcurementDeliveryLocationOptionDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("address")] string Address
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
        [property: JsonPropertyName("procurementScopeIbus")] IReadOnlyList<ProcurementScopeIbuDto> ProcurementScopeIbus,
        [property: JsonPropertyName("requireApproval")] bool RequireApproval = true,
        [property: JsonPropertyName("allowSelfApproval")] bool AllowSelfApproval = false,
        [property: JsonPropertyName("requireRequestForQuote")] bool RequireRequestForQuote = false,
        [property: JsonPropertyName("requireCostCenter")] bool RequireCostCenter = false,
        [property: JsonPropertyName("requireBudgetReference")] bool RequireBudgetReference = false,
        [property: JsonPropertyName("defaultDeliveryLocation")] string DefaultDeliveryLocation = "",
        [property: JsonPropertyName("deliveryLocationOptions")] IReadOnlyList<ProcurementDeliveryLocationOptionDto>? DeliveryLocationOptions = null
    );

    public record FieldProcurementSettingListDto(
        [property: JsonPropertyName("fieldPubId")] Guid FieldPubId,
        [property: JsonPropertyName("fieldName")] string FieldName,
        [property: JsonPropertyName("fieldActive")] bool FieldActive,
        [property: JsonPropertyName("procurementMode")] ProcurementMode ProcurementMode,
        [property: JsonPropertyName("procurementHubIbuPubId")] Guid? ProcurementHubIbuPubId,
        [property: JsonPropertyName("procurementHubIbuName")] string ProcurementHubIbuName,
        [property: JsonPropertyName("allowDirectSupplierOrdering")] bool AllowDirectSupplierOrdering,
        [property: JsonPropertyName("consolidateRequisitions")] bool ConsolidateRequisitions,
        [property: JsonPropertyName("requireApproval")] bool RequireApproval = true,
        [property: JsonPropertyName("allowSelfApproval")] bool AllowSelfApproval = false,
        [property: JsonPropertyName("requireRequestForQuote")] bool RequireRequestForQuote = false,
        [property: JsonPropertyName("requireCostCenter")] bool RequireCostCenter = false,
        [property: JsonPropertyName("requireBudgetReference")] bool RequireBudgetReference = false
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
        [param: MaxLength(500)]
        string? Notes,
        [property: JsonPropertyName("requireApproval")] bool RequireApproval = true,
        [property: JsonPropertyName("allowSelfApproval")] bool AllowSelfApproval = false,
        [property: JsonPropertyName("requireRequestForQuote")] bool RequireRequestForQuote = false,
        [property: JsonPropertyName("requireCostCenter")] bool RequireCostCenter = false,
        [property: JsonPropertyName("requireBudgetReference")] bool RequireBudgetReference = false,
        [property: JsonPropertyName("defaultDeliveryLocation")]
        [param: MaxLength(500)]
        string? DefaultDeliveryLocation = null
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
        [property: JsonPropertyName("totalQuantity")] decimal TotalQuantity,
        [property: JsonPropertyName("urgency")] ProcurementUrgency Urgency = ProcurementUrgency.Routine,
        [property: JsonPropertyName("isOverdue")] bool IsOverdue = false
    );

    public record PurchaseRequisitionEventDto(
        [property: JsonPropertyName("pubId")] Guid PubId,
        [property: JsonPropertyName("eventType")] ProcurementEventType EventType,
        [property: JsonPropertyName("fromStatus")] PurchaseRequisitionStatus? FromStatus,
        [property: JsonPropertyName("toStatus")] PurchaseRequisitionStatus? ToStatus,
        [property: JsonPropertyName("actorUserPubId")] Guid? ActorUserPubId,
        [property: JsonPropertyName("actorName")] string ActorName,
        [property: JsonPropertyName("isAutomatic")] bool IsAutomatic,
        [property: JsonPropertyName("createdDt")] DateTime CreatedDt,
        [property: JsonPropertyName("note")] string Note
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
        [property: JsonPropertyName("lines")] List<PurchaseRequisitionLineDto> Lines,
        [property: JsonPropertyName("urgency")] ProcurementUrgency Urgency = ProcurementUrgency.Routine,
        [property: JsonPropertyName("allowSubstitution")] bool AllowSubstitution = false,
        [property: JsonPropertyName("deliveryLocation")] string DeliveryLocation = "",
        [property: JsonPropertyName("costCenter")] string CostCenter = "",
        [property: JsonPropertyName("budgetReference")] string BudgetReference = "",
        [property: JsonPropertyName("concurrencyToken")] Guid ConcurrencyToken = default,
        [property: JsonPropertyName("events")] IReadOnlyList<PurchaseRequisitionEventDto>? Events = null
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
        [property: JsonPropertyName("submitForProcessing")] bool SubmitForProcessing = false,
        [property: JsonPropertyName("urgency")] ProcurementUrgency Urgency = ProcurementUrgency.Routine,
        [property: JsonPropertyName("allowSubstitution")] bool AllowSubstitution = false,
        [property: JsonPropertyName("deliveryLocation")]
        [property: MaxLength(500)]
        string? DeliveryLocation = null,
        [property: JsonPropertyName("costCenter")]
        [property: MaxLength(100)]
        string? CostCenter = null,
        [property: JsonPropertyName("budgetReference")]
        [property: MaxLength(100)]
        string? BudgetReference = null
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
        [property: JsonPropertyName("submitForProcessing")] bool SubmitForProcessing = false,
        [property: JsonPropertyName("urgency")] ProcurementUrgency Urgency = ProcurementUrgency.Routine,
        [property: JsonPropertyName("allowSubstitution")] bool AllowSubstitution = false,
        [property: JsonPropertyName("deliveryLocation")]
        [property: MaxLength(500)]
        string? DeliveryLocation = null,
        [property: JsonPropertyName("costCenter")]
        [property: MaxLength(100)]
        string? CostCenter = null,
        [property: JsonPropertyName("budgetReference")]
        [property: MaxLength(100)]
        string? BudgetReference = null,
        [property: JsonPropertyName("concurrencyToken")] Guid ConcurrencyToken = default
    );

    public record PurchaseRequisitionStatusUpdateDto(
        [property: JsonPropertyName("status")] PurchaseRequisitionStatus Status,
        [property: JsonPropertyName("decisionNotes")]
        [property: MaxLength(1000)]
        string? DecisionNotes,
        [property: JsonPropertyName("procurementManagerUserPubId")] Guid? ProcurementManagerUserPubId,
        [property: JsonPropertyName("concurrencyToken")] Guid ConcurrencyToken = default
    );
}
