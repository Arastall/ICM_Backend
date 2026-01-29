namespace ICMServer.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderReprocessResult>> ReprocessOrdersAsync(List<string> orderNumbers);
        Task<OrderSummaryDto?> GetOrderSummaryAsync(string orderNumber);
    }

    public class OrderReprocessResult
    {
        public string OrderNumber { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    // ===== Order Summary DTOs =====

    public class OrderSummaryDto
    {
        public OrderHeaderDto Header { get; set; } = null!;
        public List<OrderLineDto> Lines { get; set; } = new();
        public List<AllocationPercentDto> Allocations { get; set; } = new();
        public List<CommissionPaymentDto> Payments { get; set; } = new();
        public List<AdjustmentDto> Adjustments { get; set; } = new();
        public List<CreditAllocationDto> CreditAllocations { get; set; } = new();
        public List<CreditAllocationDto> Rollups { get; set; } = new();
    }

    public class OrderHeaderDto
    {
        public string? OrderRowId { get; set; }
        public string? OrderNumber { get; set; }
        public string? OrderType { get; set; }
        public string? PromotionCode { get; set; }
        public string? SapOrderReference { get; set; }
        public string? CustomerAccountNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerType { get; set; }
        public string? PrimaryPosition { get; set; }
        public decimal? OrderListVal { get; set; }
        public decimal? OrderSaleVal { get; set; }
        public decimal? OrderDiscountVal { get; set; }
        public decimal? OrderDiscountPercent { get; set; }
        public decimal? ServiceListVal { get; set; }
        public decimal? ServiceSaleVal { get; set; }
        public decimal? ServiceDiscountVal { get; set; }
        public decimal? ServiceDiscountPercent { get; set; }
        public string? ServiceType { get; set; }
        public string? PeriodMonth { get; set; }
        public string? PeriodYear { get; set; }
        public string? MaintenanceTerm { get; set; }
    }

    public class OrderLineDto
    {
        public string? OrderItemId { get; set; }
        public int? LineNumber { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDesc { get; set; }
        public decimal? ListValue { get; set; }
        public decimal? SaleValue { get; set; }
        public decimal? Quantity { get; set; }
        public string? ProductLevel1 { get; set; }
        public string? ProductLevel2 { get; set; }
        public string? ProductLevel3 { get; set; }
        public string? ProductType { get; set; }
    }

    public class AllocationPercentDto
    {
        public string? EmployeeName { get; set; }
        public string? EmployeeRowId { get; set; }
        public string? PositionName { get; set; }
        public string? PayplanType { get; set; }
        public decimal? AllocationPercentage { get; set; }
    }

    public class CommissionPaymentDto
    {
        public string? PaymentSource { get; set; }
        public string? PaymentDescription { get; set; }
        public string? PositionName { get; set; }
        public string? PositionId { get; set; }
        public string? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EnhancementDesc { get; set; }
        public string? RateTableDesc { get; set; }
        public decimal? PaymentRate { get; set; }
        public decimal? PaymentValue { get; set; }
        public bool PaymentWithheld { get; set; }
        public string? PeriodYear { get; set; }
        public string? PeriodMonth { get; set; }
    }

    public class AdjustmentDto
    {
        public string? PaymentSource { get; set; }
        public string? PaymentDescription { get; set; }
        public string? PositionName { get; set; }
        public string? PositionId { get; set; }
        public string? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public decimal? PaymentValue { get; set; }
        public bool PaymentWithheld { get; set; }
        public string? PeriodMonth { get; set; }
        public string? PeriodYear { get; set; }
    }

    public class CreditAllocationDto
    {
        public string? EmployeeName { get; set; }
        public string? ProductDesc { get; set; }
        public string? AllocationDescription { get; set; }
        public decimal? AllocationList { get; set; }
        public decimal? AllocationValue { get; set; }
        public string? ExcludeFromCalcs { get; set; }
        public string? RollupProcessed { get; set; }
        public string? PeriodMonth { get; set; }
        public string? PeriodYear { get; set; }
    }
}
