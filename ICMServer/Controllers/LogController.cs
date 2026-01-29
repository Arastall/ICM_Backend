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
    public class LogController : ControllerBase
    {
        private IRepository _repository;
        private readonly ILogger<LogController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<NotificationHub> _hub;

        public LogController(ILogger<LogController> logger, IRepository repository, IConfiguration configuration, IHubContext<NotificationHub> hub)
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
        [Route("GetOrderSequenceFromLogs/{runID}")]
        public async Task<IActionResult> GetOrderSequenceFromLogs(int runID)
        {
            _logger.LogInformation($"Get runID {runID} deals sequences");


            await _hub.Clients.All.SendAsync("GeneratingReport", new { status = "running", time = DateTime.Now });

            var reportBytes = _repository.GetOrderSequenceFromLogs(runID);
            var env = _configuration["Environment"];
            await _hub.Clients.All.SendAsync("GeneratingReport", new { status = "completed", time = DateTime.Now });
            return File(reportBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{env}_RUN_{runID}_OrderSequence_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx", false);
        }

    }
}
