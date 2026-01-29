using DocumentFormat.OpenXml.Drawing.Charts;
using ICMServer.Classes;
using ICMServer.Interfaces;
using ICMServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.IO;

namespace ICMServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        private IRepository _repository;
        private readonly ILogger<SettingsController> _logger;
        private readonly IConfiguration _configuration;

        public SettingsController(ILogger<SettingsController> logger, IRepository repository, IConfiguration configuration)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
        }



        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Alive !");
        }

        [HttpGet]
        [Route("GetServerEnvironmentData")]
        public ActionResult<string> GetServerEnvironmentData()
        {
            _logger.LogInformation($"Getting server environment");
            
            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = _configuration.GetConnectionString("iis");
            builder.TryGetValue("Server", out var serverDb);
            builder.TryGetValue("Database", out var dbName);

            dynamic result = _repository.GetLastRunInfo();
            return Ok(new
            {
                serverdb = serverDb,
                dbName,
                serverName = Environment.MachineName,
                result.status,
                result.dtLastRun
            });
        }

        [HttpGet]
        [Route("GetCurrentSalesPeriodStr")]
        public ActionResult<object> GetCurrentSalesPeriodStr()
        {
            _logger.LogInformation($"Getting current sales period");
            var salesPeriod = _repository.GetCurrentSalesPeriodStr();
            return Ok(salesPeriod); 
        }

        [HttpGet]
        [Route("GetCurrentSalesPeriod")]
        public ActionResult<object> GetCurrentSalesPeriod()
        {
            _logger.LogInformation($"Getting current sales period");
            var salesPeriod = _repository.GetCurrentSalesPeriod();
            return Ok(salesPeriod);
        }

        [HttpPost]
        [Route("SetCurrentSalesPeriod/{year}/{month}")]
        public ActionResult<bool> SetCurrentSalesPeriod(string year, string month)
        {
            _logger.LogInformation($"setting new current sales period");
            var result = _repository.SetCurrentSalesPeriod(year, month);
            return Ok(result);
        }
    }
}
