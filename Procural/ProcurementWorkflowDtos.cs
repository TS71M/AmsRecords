using Lib.Enums;

namespace AmsRecords.Procural;

public static class ProcurementWorkflowDtos
{
    public record ProcurementQuoteLineDto(
        Guid ProductPubId,
        string ProductName,
        decimal Quantity,
        decimal? UnitPrice,
        string Notes,
        decimal? LineTotal);

    public record ProcurementQuoteDto(
        Guid PubId,
        Guid RequisitionPubId,
        Guid SupplierPubId,
        string SupplierName,
        ProcurementQuoteStatus Status,
        DateTime RequestedDt,
        DateTime? ReceivedDt,
        DateTime? ValidUntilDate,
        string CurrencyCode,
        string Terms,
        string Notes,
        Guid ConcurrencyToken,
        IReadOnlyList<ProcurementQuoteLineDto> Lines,
        decimal? Total);

    public record ProcurementQuoteRequestDto(
        Guid RequisitionPubId,
        [property: MinLength(1)] IReadOnlyList<Guid> SupplierPubIds,
        [property: MaxLength(1000)] string? Note);

    public record ProcurementQuoteLineUpdateDto(
        Guid ProductPubId,
        [property: Range(typeof(decimal), "0", "999999999999.99")] decimal? UnitPrice,
        [property: MaxLength(500)] string? Notes);

    public record ProcurementQuoteUpdateDto(
        Guid PubId,
        [property: StringLength(3, MinimumLength = 3)] string CurrencyCode,
        DateTime? ValidUntilDate,
        [property: MaxLength(500)] string? Terms,
        [property: MaxLength(1000)] string? Notes,
        Guid ConcurrencyToken,
        IReadOnlyList<ProcurementQuoteLineUpdateDto> Lines);

    public record ProcurementQuoteSelectDto(Guid ConcurrencyToken, [property: MaxLength(1000)] string? Note);

    public record ProcurementPurchaseOrderLineInputDto(
        Guid ProductPubId,
        Guid? ProductSupplierPubId,
        [property: Range(typeof(decimal), "0.001", "999999999999.999")] decimal Quantity,
        [property: Range(typeof(decimal), "0", "999999999999.99")] decimal UnitPrice,
        [property: MaxLength(500)] string? Notes);

    public record ProcurementPurchaseOrderCreateDto(
        Guid RequisitionPubId,
        Guid SupplierPubId,
        Guid? SelectedQuotePubId,
        [property: Required, MaxLength(100)] string OrderNumber,
        [property: Required, StringLength(3, MinimumLength = 3)] string CurrencyCode,
        DateTime? ExpectedDeliveryDate,
        [property: MaxLength(500)] string? DeliveryLocation,
        [property: MaxLength(500)] string? Terms,
        [property: MaxLength(1000)] string? Notes,
        IReadOnlyList<ProcurementPurchaseOrderLineInputDto> Lines,
        bool PlaceImmediately = true,
        Guid RequisitionConcurrencyToken = default);

    public record ProcurementPurchaseOrderLineDto(
        int LineId,
        Guid ProductPubId,
        string ProductName,
        string UnitLabel,
        string SupplierProductCode,
        decimal QuantityOrdered,
        decimal QuantityReceived,
        decimal QuantityRejected,
        decimal QuantityOutstanding,
        decimal UnitPrice,
        decimal LineTotal,
        string Notes);

    public record ProcurementReceiptLineInputDto(
        int OrderLineId,
        [property: Range(typeof(decimal), "0", "999999999999.999")] decimal QuantityReceived,
        [property: Range(typeof(decimal), "0", "999999999999.999")] decimal QuantityRejected,
        [property: MaxLength(500)] string? DiscrepancyReason);

    public record ProcurementReceiptCreateDto(
        Guid PurchaseOrderPubId,
        DateTime ReceivedDt,
        [property: MaxLength(100)] string? DeliveryReference,
        [property: MaxLength(1000)] string? Notes,
        Guid OrderConcurrencyToken,
        IReadOnlyList<ProcurementReceiptLineInputDto> Lines);

    public record ProcurementReceiptDto(
        Guid PubId,
        DateTime ReceivedDt,
        string ReceivedByName,
        string DeliveryReference,
        string Notes,
        IReadOnlyList<ProcurementReceiptLineInputDto> Lines);

    public record ProcurementPurchaseOrderListDto(
        Guid PubId,
        Guid RequisitionPubId,
        string OrderNumber,
        string FieldName,
        string SupplierName,
        ProcurementPurchaseOrderStatus Status,
        DateTime CreatedDt,
        DateTime? ExpectedDeliveryDate,
        string CurrencyCode,
        decimal Total,
        decimal ReceivedPercent,
        ProcurementInventoryHandoffStatus InventoryHandoffStatus);

    public record ProcurementPurchaseOrderDto(
        Guid PubId,
        Guid RequisitionPubId,
        string RequisitionTitle,
        string OrderNumber,
        Guid FieldPubId,
        string FieldName,
        Guid SupplierPubId,
        string SupplierName,
        ProcurementPurchaseOrderStatus Status,
        ProcurementInventoryHandoffStatus InventoryHandoffStatus,
        DateTime CreatedDt,
        DateTime? PlacedDt,
        DateTime? ExpectedDeliveryDate,
        DateTime? ClosedDt,
        string CurrencyCode,
        string DeliveryLocation,
        string Terms,
        string Notes,
        Guid ConcurrencyToken,
        IReadOnlyList<ProcurementPurchaseOrderLineDto> Lines,
        IReadOnlyList<ProcurementReceiptDto> Receipts,
        decimal Total);

    public record ProcurementOrderActionDto(Guid ConcurrencyToken, [property: MaxLength(1000)] string? Note);
}
