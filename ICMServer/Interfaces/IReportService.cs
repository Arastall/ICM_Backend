namespace ICMServer.Interfaces
{
    public interface IReportService
    {
        byte[] GetYearlyRevenuesPaymentsReport(string year);
        byte[] GetMonthRevenuesPaymentsReport(string year, string month);
        byte[] GetMonthlyDealsInfoReport(string year, string month);
        byte[] GetPayFileReport(string year, string month);
        byte[] GetPayFileReportOld(string year, string month);
        byte[] GetProductsReport();
    }
}
