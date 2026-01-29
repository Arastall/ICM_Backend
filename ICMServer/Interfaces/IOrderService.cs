namespace ICMServer.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderReprocessResult>> ReprocessOrdersAsync(List<string> orderNumbers);
    }

    public class OrderReprocessResult
    {
        public string OrderNumber { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
