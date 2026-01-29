namespace ICMServer.Classes
{
    public class PaymentDto
    {
        public string month { get; set; }
        public string year { get; set; }
        public string SalePeriod { get; set; }
        public string Employee { get; set; }
        public int? CommID { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string CustomerName { get; set; }
        public string OrderNumber { get; set; }
        public string AdjOrderNumber { get; set; }
        public string Reason { get; set; }
        public string PaymentDescription { get; set; }
        public string PaymentSource { get; set; }
        public decimal? PaymentRate { get; set; }
        public decimal? PaymentValue { get; set; }
        public string Type { get; set; }
    }
}
