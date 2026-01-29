using DocumentFormat.OpenXml.Bibliography;
using ICMServer.Classes;
using ICMServer.Interfaces;
using ICMServer.Managers;
using ICMServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.IO;

namespace ICMServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private IRepository _repository;
        private readonly ILogger<ReportController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<NotificationHub> _hub;

        public ReportController(ILogger<ReportController> logger, IRepository repository, IConfiguration configuration, IHubContext<NotificationHub> hub)
        {
            _logger = logger;
            _repository = repository;
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
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Generating revenues and payments report for ${year}", time = DateTime.Now });

            var reportBytes = _repository.GetYearlyRevenuesPaymentsReport(year);
            var env = _configuration["Environment"];
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{env}_FY{year}_Revenues_&_Payments_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx", false);
        }

        [HttpGet]
        [Route("GetMonthlyRevenuesPaymentsReport/{year}/{month}")]
        public IActionResult GetMonthlyRevenuesPaymentsReport(string year, string month)
        {
            _logger.LogInformation($"Get month {year}-{month} revenues and payments report");
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Generating revenues and payments report for ${year} and ${month}", time = DateTime.Now });

            var reportBytes = _repository.GetMonthRevenuesPaymentsReport(year, month);
            var env = _configuration["Environment"];
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{env}_FY{year}_Revenues_&_Payments_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx", false);
        }

        [HttpGet]
        [Route("GetMonthlyDealsInfoReport/{year}/{month}")]
        public IActionResult GetMonthlyDealsInfoReport(string year, string month)
        {
            _logger.LogInformation($"Get month {year}-{month} revenues and payments report");
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Generating revenues and payments report for ${year} and ${month}", time = DateTime.Now });

            var reportBytes = _repository.GetMonthlyDealsInfoReport(year, month);
            var env = _configuration["Environment"];
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{env}_FY{year}_Revenues_&_Payments_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx", false);
        }



        [HttpGet]
        [Route("GetPayFileReport/{year}/{month}")]
        public ActionResult GetPayFileReport(string year, string month)
        {
            _logger.LogInformation($"Get current SP payfile report");
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Preparing FY${year}SP${month} payfile ", time = DateTime.Now });
            var reportBytes = _repository.GetPayFileReport(year, month);
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"PayFile_FY{year}SP{month}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx", false);
        }


        [HttpGet]
        [Route("GetPayFileReportOld/{year}/{month}")]
        public ActionResult GetPayFileReportOld(string year, string month)
        {
            _logger.LogInformation($"Get current SP payfile report");
            _hub.Clients.All.SendAsync("GeneratingReport", new { status = $"Preparing FY${year}SP${month} payfile ", time = DateTime.Now });
            var reportBytes = _repository.GetPayFileReportOld(year, month);
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"PayFile_FY{year}SP{month}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx", false);
        }

        /*[HttpGet]
        [Route("GetCurrentSPPayFile")]
        public ActionResult GetCurrentSPPayFile()
        {
            _logger.LogInformation($"Get current SP payfile report");
            var reportBytes = _repository.GetCurrentSPPayFile();
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"FY{year}_Revenues_&_Payments_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx", false);
        }*/
    }
}
