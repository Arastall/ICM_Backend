using ICMServer.Interfaces;
using ICMServer.Managers;
using ICMServer.Models;
using ICMServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ICMServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IProcessStateService _processState;

        public MenuController(ILogger<MenuController> logger, IServiceScopeFactory scopeFactory,
            IHubContext<NotificationHub> hub, IProcessStateService processState)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _hub = hub;
            _processState = processState;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Alive!");
        }

       /*[HttpGet]
        [Route("GetMenuList")]
        public ActionResult<Menu> GetMenuList()
        {
            _logger.LogInformation("Get menu list");
            var menus = new Menu();
            return Ok(menus);
        }*/

        [HttpPost("RunICM")]
        public IActionResult RunICM([FromServices] ProcessQueueService queue)
        {
            queue.Enqueue(async (serviceProvider) =>
            {
                var repository = serviceProvider.GetRequiredService<IRepository>();
                await repository.RunICMFrontend();
            });

            return Ok();
        }

        [HttpPost]
        [Route("StartICMRunThreading")]
        public IActionResult StartICMRunThreading([FromServices] ProcessQueueService queue)
        {
            _logger.LogInformation("Starting ICM Run Queue...");
            queue.Enqueue(async (serviceProvider) =>
            {
                var repository = serviceProvider.GetRequiredService<IRepository>();

                await _hub.Clients.All.SendAsync("ProcessStepInit", new
                {
                    stepId = "import_siebel_data",
                    stepName = "Importing Siebel Data",
                    status = "in_progress",
                    message = "Processing records",
                    time = DateTime.Now
                });

                await repository.RunICM();
            });

            return Ok();
        }

        [HttpPost]
        [Route("StartICMRun")]
        public async Task<IActionResult> StartICMRun()
        {
            _logger.LogInformation("Starting ICM Run...");

            using var serviceScope = _scopeFactory.CreateScope();
            var repo = serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            await repo.RunICM();

            return Ok();
        }

        [HttpPost]
        [Route("StartICMRunOld")]
        public async Task<IActionResult> StartICMRunOld()
        {
            _logger.LogInformation("Starting ICM Run Old...");

            using var serviceScope = _scopeFactory.CreateScope();
            var repo = serviceScope.ServiceProvider.GetRequiredService<IRepository>();
            await repo.RunICMOld();

            return Ok();
        }

        [HttpGet]
        [Route("GetProcessState")]
        public IActionResult GetProcessState()
        {
            var state = _processState.GetState();
            return Ok(state);
        }

        [HttpPost]
        [Route("ResetProcessState")]
        public IActionResult ResetProcessState()
        {
            _processState.Reset();
            return Ok();
        }
    }
}
