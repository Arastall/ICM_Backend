using ICMServer.Interfaces;
using ICMServer.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ICMServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<NotificationHub> _hub;

        public ReportController(ILogger<ReportController> logger, IReportService reportService, IConfiguration configuration, IHubContext<NotificationHub> hub)
        {
            _logger = logger;
            _reportService = reportService;
            _configuration = configuration;
            _hub = hub;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Alive !");
        }

        [HttpGet]
        [Route("GetYearlyRevenuesPaymentsReport/{year}")]
        public IActionResult GetYearlyRevenuesPaymentsReport(string year)
        {
            _logger.LogInformation($"Get year {year} revenues and payments report");
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Generating revenues and payments report for {year}", time = DateTime.Now });

            var reportBytes = _reportService.GetYearlyRevenuesPaymentsReport(year);
            var env = _configuration["Environment"];
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{env}_FY{year}_Revenues_&_Payments_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx", false);
        }

        [HttpGet]
        [Route("GetMonthlyRevenuesPaymentsReport/{year}/{month}")]
        public IActionResult GetMonthlyRevenuesPaymentsReport(string year, string month)
        {
            _logger.LogInformation($"Get month {year}-{month} revenues and payments report");
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Generating revenues and payments report for {year} and {month}", time = DateTime.Now });

            var reportBytes = _reportService.GetMonthRevenuesPaymentsReport(year, month);
            var env = _configuration["Environment"];
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{env}_FY{year}_Revenues_&_Payments_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx", false);
        }

        [HttpGet]
        [Route("GetMonthlyDealsInfoReport/{year}/{month}")]
        public IActionResult GetMonthlyDealsInfoReport(string year, string month)
        {
            _logger.LogInformation($"Get month {year}-{month} deals info report");
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Generating deals info report for {year} and {month}", time = DateTime.Now });

            var reportBytes = _reportService.GetMonthlyDealsInfoReport(year, month);
            var env = _configuration["Environment"];
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{env}_FY{year}_Deals_Info_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx", false);
        }

        [HttpGet]
        [Route("GetPayFileReport/{year}/{month}")]
        public ActionResult GetPayFileReport(string year, string month)
        {
            _logger.LogInformation($"Get current SP payfile report");
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Preparing FY{year}SP{month} payfile", time = DateTime.Now });

            var reportBytes = _reportService.GetPayFileReport(year, month);
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"PayFile_FY{year}SP{month}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx", false);
        }

        [HttpGet]
        [Route("GetPayFileReportOld/{year}/{month}")]
        public ActionResult GetPayFileReportOld(string year, string month)
        {
            _logger.LogInformation($"Get current SP payfile report (old)");
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Preparing FY{year}SP{month} payfile", time = DateTime.Now });

            var reportBytes = _reportService.GetPayFileReportOld(year, month);
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"PayFile_FY{year}SP{month}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx", false);
        }

        [HttpGet]
        [Route("GetProductsReport")]
        public ActionResult GetProductsReport()
        {
            _logger.LogInformation("Get products report");
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = "Generating products report", time = DateTime.Now });

            var reportBytes = _reportService.GetProductsReport();
            var env = _configuration["Environment"];
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{env}_Products_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx", false);
        }
    }
}
