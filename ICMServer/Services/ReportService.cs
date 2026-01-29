using ClosedXML.Excel;
using ICMServer.DBContext;
using ICMServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICMServer.Services
{
    public class ReportService : IReportService
    {
        private readonly ICMDBContext _context;
        private readonly ILogger<ReportService> _logger;
        private readonly IRepository _repository;

        public ReportService(ICMDBContext context, ILogger<ReportService> logger, IRepository repository)
        {
            _context = context;
            _logger = logger;
            _repository = repository;
        }

        // Delegate existing reports to repository for now (can be migrated later)
        public byte[] GetYearlyRevenuesPaymentsReport(string year)
            => _repository.GetYearlyRevenuesPaymentsReport(year);

        public byte[] GetMonthRevenuesPaymentsReport(string year, string month)
            => _repository.GetMonthRevenuesPaymentsReport(year, month);

        public byte[] GetMonthlyDealsInfoReport(string year, string month)
            => _repository.GetMonthlyDealsInfoReport(year, month);

        public byte[] GetPayFileReport(string year, string month)
            => _repository.GetPayFileReport(year, month);

        public byte[] GetPayFileReportOld(string year, string month)
            => _repository.GetPayFileReportOld(year, month);

        /// <summary>
        /// Products Report - extract all products from DataProductsIncluded (all columns except Id)
        /// </summary>
        public byte[] GetProductsReport()
        {
            _logger.LogInformation("Generating Products report");

            var products = _context.DataProductsIncludeds
                .OrderBy(p => p.ProductCode)
                .Select(p => new
                {
                    p.ItemId,
                    p.ProductDesc,
                    p.ProductCode,
                    p.ProductLevel1,
                    p.ProductLevel2,
                    p.ProductLevel3,
                    p.ProductType,
                    p.Included,
                    p.LevelBased,
                    p.AllocationId
                })
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Products");

                // Headers
                var headers = new[]
                {
                    "Item ID", "Product Description", "Product Code",
                    "Product Level 1", "Product Level 2", "Product Level 3",
                    "Product Type", "Included", "Level Based", "Allocation ID"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    ws.Cell(1, i + 1).Value = headers[i];
                }

                // Style headers
                var headerRange = ws.Range(1, 1, 1, headers.Length);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#1e3a5f");
                headerRange.Style.Font.FontColor = XLColor.White;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Data rows
                int row = 2;
                foreach (var p in products)
                {
                    ws.Cell(row, 1).Value = p.ItemId ?? "";
                    ws.Cell(row, 2).Value = p.ProductDesc ?? "";
                    ws.Cell(row, 3).Value = p.ProductCode ?? "";
                    ws.Cell(row, 4).Value = p.ProductLevel1 ?? "";
                    ws.Cell(row, 5).Value = p.ProductLevel2 ?? "";
                    ws.Cell(row, 6).Value = p.ProductLevel3 ?? "";
                    ws.Cell(row, 7).Value = p.ProductType ?? "";
                    ws.Cell(row, 8).Value = p.Included ?? 0;
                    ws.Cell(row, 9).Value = p.LevelBased ?? 0;
                    ws.Cell(row, 10).Value = p.AllocationId ?? 0;
                    row++;
                }

                // Auto-fit columns
                ws.Columns().AdjustToContents();

                // Auto-filter
                ws.RangeUsed()?.SetAutoFilter();

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
