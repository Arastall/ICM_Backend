using ICMServer.DBContext;
using ICMServer.Managers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ICMServer.Services
{
    public interface IImportService
    {
        Task DoImportAsync();
    }

    public class ImportService : IImportService
    {
        private readonly ILogger<ImportService> _logger;
        private readonly IServiceProvider _sp;
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IProcessStateService _processState;

        public ImportService(ILogger<ImportService> logger, IServiceProvider sp, IHubContext<NotificationHub> hub, IProcessStateService processState)
        {
            _logger = logger;
            _sp = sp;
            _hub = hub;
            _processState = processState;
        }

        public async Task DoImportAsync()
        {
            try
            {
                _logger.LogInformation("Do Siebel Import - Started");

                _processState.UpdateStep("import_siebel_data", "Importing Siebel Data", "in_progress", "Processing records");
                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "import_siebel_data",
                    stepName = "Importing Siebel Data",
                    status = "in_progress",
                    message = "Processing records",
                    time = DateTime.Now
                });

                var scope = _sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ICMDBContext>();

                context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
                await context.Database.ExecuteSqlRawAsync("EXEC usp_RUN_DATA_COLLECTION_ENGINE");

                _logger.LogInformation("Do Siebel Import - Completed successfully");
                _processState.UpdateStep("import_siebel_data", "Importing Siebel Data", "completed", "Import completed");
                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "import_siebel_data",
                    stepName = "Importing Siebel Data",
                    status = "completed",
                    message = "Import completed",
                    time = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Siebel Import");
                _processState.UpdateStep("import_siebel_data", "Importing Siebel Data", "error", $"Error occurred: {ex.Message}");
                await _hub.Clients.All.SendAsync("ProcessStepUpdate", new
                {
                    stepId = "import_siebel_data",
                    stepName = "Importing Siebel Data",
                    status = "error",
                    message = $"Error occurred: {ex.Message}",
                    time = DateTime.Now
                });

                throw;
            }
        }

    }

}