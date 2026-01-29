namespace ICMServer.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeSummaryDto?> GetEmployeeSummaryAsync(string login, string year, string month);
    }

    // ===== Employee Summary DTOs =====

    public class EmployeeSummaryDto
    {
        public EmployeeDetailDto? EmployeeDetails { get; set; }
        public List<AllocatedOrderDto> AllocatedOrders { get; set; } = new();
        public List<UnitSummaryDto> UnitSummary { get; set; } = new();
        public List<OrderAllocationTotalDto> OrderAllocationTotals { get; set; } = new();
        public List<AllocationTotalDto> AllocationTotals { get; set; } = new();
        public List<EmpPaymentDetailDto> PaymentDetails { get; set; } = new();
        public decimal PaymentTotal { get; set; }
        public List<AdjustmentSummaryDto> Adjustments { get; set; } = new();
        public List<OrderLineGroupDto> OrderLines { get; set; } = new();
    }

    public class EmployeeDetailDto
    {
        public string? RowId { get; set; }
        public string? Login { get; set; }
        public string? FstName { get; set; }
        public string? LastName { get; set; }
        public string? JobTitle { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? PositionName { get; set; }
        public string? XRepType { get; set; }
    }

    public class AllocatedOrderDto
    {
        public string? SapOrderReference { get; set; }
        public string? CustomerName { get; set; }
        public string? OrderNumber { get; set; }
        public string? PositionName { get; set; }
        public string? PayplanType { get; set; }
        public decimal OrderListVal { get; set; }
        public decimal OrderSaleVal { get; set; }
        public decimal? OrderDiscountPercent { get; set; }
        public string? ServiceType { get; set; }
        public decimal? ServiceDiscountPercent { get; set; }
        public string? OrderType { get; set; }
        public string? CustomerType { get; set; }
    }

    public class UnitSummaryDto
    {
        public string? OrderNumber { get; set; }
        public int? LineNumber { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDesc { get; set; }
        public string? AllocationDescription { get; set; }
        public int AllocationValue { get; set; }
        public string? ExcludeFromCalcs { get; set; }
    }

    public class OrderAllocationTotalDto
    {
        public string? PrimaryFlag { get; set; }
        public string? OrderNumber { get; set; }
        public string? PositionName { get; set; }
        public string? PayplanType { get; set; }
        public decimal OrderListVal { get; set; }
        public decimal OrderSaleVal { get; set; }
        public decimal? OrderDiscountPercent { get; set; }
        public decimal? ServiceDiscountPercent { get; set; }
        public decimal? AllocationPercentage { get; set; }
        public decimal AllocationList { get; set; }
        public decimal AllocationValue { get; set; }
        public string? ExcludeFromCalcs { get; set; }
    }

    public class AllocationTotalDto
    {
        public int AllocationTypeId { get; set; }
        public string? AllocationType { get; set; }
        public string? AllocationDescription { get; set; }
        public decimal AllocationValue { get; set; }
    }

    public class EmpPaymentDetailDto
    {
        public string? OrderNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? PaymentDescription { get; set; }
        public decimal? PaymentRate { get; set; }
        public decimal PaymentValue { get; set; }
        public string? PaymentWithheld { get; set; }
    }

    public class AdjustmentSummaryDto
    {
        public string? OrderNumber { get; set; }
        public string? CustomerName { get; set; }
        public string? AdjustmentReason { get; set; }
        public string? AdjustmentType { get; set; }
        public decimal AllocationList { get; set; }
        public decimal AllocationValue { get; set; }
        public decimal PaymentValue { get; set; }
    }

    public class OrderLineGroupDto
    {
        public string? OrderNumber { get; set; }
        public decimal TotalAllocationValue { get; set; }
        public string? ExcludeFromCalcs { get; set; }
        public List<OrderLineDetailDto> Lines { get; set; } = new();
    }

    public class OrderLineDetailDto
    {
        public string? AllocationDescription { get; set; }
        public decimal AllocationValue { get; set; }
    }
}
