namespace ICMServer.Classes
{
    public class RevenueDto
    {
        public int RefID { get; set; }
        public string SalePeriod { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public DateTime OriginalOrderDate { get; set; }
        public string Employee { get; set; } = string.Empty;
        public string Reason { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string OrderNumber { get; set; }
        public string AdjOrderNumber { get; set; }
        public string ProductDesc { get; set; } = string.Empty;
        public string AllocationSource { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string PayPlan { get; set; } = string.Empty;
        public string AllocationDescription { get; set; } = string.Empty;
        public int AllocationTypeId { get; set; }

    }
}
