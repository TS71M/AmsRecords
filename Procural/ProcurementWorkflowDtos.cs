using Lib.Enums;

namespace AmsRecords.Procural;

public static class ProcurementWorkflowDtos
{
    public record ProcurementOrderStockLineDto(Guid ProductPubId, string ProductName, string UnitLabel, decimal Quantity);

    public record ProcurementOrderStockDto(Guid FieldPubId, DateTime CalculatedAtUtc, bool IsEnabled,
        IReadOnlyList<ProcurementOrderStockLineDto> Lines);

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
        [param: MinLength(1)] IReadOnlyList<Guid> SupplierPubIds,
        [param: MaxLength(1000)] string? Note);

    public record ProcurementQuoteLineUpdateDto(
        Guid ProductPubId,
        [param: Range(typeof(decimal), "0", "999999999999.99", ParseLimitsInInvariantCulture = true)] decimal? UnitPrice,
        [param: MaxLength(500)] string? Notes);

    public record ProcurementQuoteUpdateDto(
        Guid PubId,
        [param: StringLength(3, MinimumLength = 3)] string CurrencyCode,
        DateTime? ValidUntilDate,
        [param: MaxLength(500)] string? Terms,
        [param: MaxLength(1000)] string? Notes,
        Guid ConcurrencyToken,
        IReadOnlyList<ProcurementQuoteLineUpdateDto> Lines);

    public record ProcurementQuoteSelectDto(Guid ConcurrencyToken, [param: MaxLength(1000)] string? Note);
    public record ProcurementQuoteReopenDto(Guid ConcurrencyToken);

    public record ProcurementPurchaseOrderLineInputDto(
        Guid ProductPubId,
        Guid? ProductSupplierPubId,
        [param: Range(typeof(decimal), "0.001", "999999999999.999", ParseLimitsInInvariantCulture = true)] decimal Quantity,
        [param: Range(typeof(decimal), "0", "999999999999.99", ParseLimitsInInvariantCulture = true)] decimal UnitPrice,
        [param: MaxLength(500)] string? Notes);

    public record ProcurementPurchaseOrderCreateDto(
        Guid RequisitionPubId,
        Guid SupplierPubId,
        Guid? SelectedQuotePubId,
        [param: Required, MaxLength(100)] string OrderNumber,
        [param: Required, StringLength(3, MinimumLength = 3)] string CurrencyCode,
        DateTime? ExpectedDeliveryDate,
        [param: MaxLength(500)] string? DeliveryLocation,
        [param: MaxLength(500)] string? Terms,
        [param: MaxLength(1000)] string? Notes,
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
        [param: ProcurementReceiptQuantity] decimal QuantityReceived,
        [param: ProcurementReceiptQuantity] decimal QuantityRejected,
        [param: MaxLength(500)] string? DiscrepancyReason);

    public record ProcurementReceiptCreateDto(
        Guid PurchaseOrderPubId,
        DateTime ReceivedDt,
        [param: MaxLength(100)] string? DeliveryReference,
        [param: MaxLength(1000)] string? Notes,
        Guid OrderConcurrencyToken,
        IReadOnlyList<ProcurementReceiptLineInputDto> Lines);

    public record ProcurementReceiptDto(
        Guid PubId,
        DateTime ReceivedDt,
        string ReceivedByName,
        string DeliveryReference,
        string Notes,
        IReadOnlyList<ProcurementReceiptLineInputDto> Lines,
        DateTime? InventoryPostedDt = null);

    /// <param name="ReceivedPercent">Equal-weight average of accepted fractions per line;
    /// incomplete orders remain below 100 even after rounding. Rejected goods do not contribute.</param>
    /// <param name="FullyReceivedLineCount">Fully accepted lines, or null from an older API.</param>
    /// <param name="LineCount">Total order lines, or null from an older API.</param>
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
        ProcurementInventoryHandoffStatus InventoryHandoffStatus,
        int? FullyReceivedLineCount = null,
        int? LineCount = null);

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

    public record ProcurementOrderActionDto(Guid ConcurrencyToken, [param: MaxLength(1000)] string? Note);
}
