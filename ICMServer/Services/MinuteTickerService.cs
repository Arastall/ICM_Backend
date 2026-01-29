using ICMServer.Classes;
using ICMServer.Interfaces;
using ICMServer.Managers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace ICMServer.Services
{
    public class MinuteTickerService : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IServiceScopeFactory _scopeFactory;

        public MinuteTickerService(IHubContext<NotificationHub> hub, IServiceScopeFactory scopeFactory)
        {
            _hub = hub;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //await BroadcastAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    await BroadcastAsync(stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    // arrêt propre
                }
            }
        }

        private async Task BroadcastAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository>();

            var mainInfo = repository.GetMainInfo();

            if (mainInfo == null || mainInfo.Status == "NO_DATA")
                return;

            // ✅ Structure pour correspondre au TypeScript
            var payload = new
            {
                type = DetermineType(mainInfo.Status),
                message = FormatMessage(mainInfo),
                data = new
                {
                    status = mainInfo.Status,
                    user = mainInfo.User,
                    salesPeriod = mainInfo.Salesperiod, 
                    begin = mainInfo.BeginDate,
                    end = mainInfo.EndDate
                },
                timestamp = DateTime.UtcNow
            };

            await _hub.Clients.All.SendAsync("UpdateMainInfo", payload, ct);
        }

        private string DetermineType(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "info";

            return status.ToUpper() switch
            {
                "COMPLETE" or "SUCCESS" => "success",
                "ERROR" or "FAILED" => "error",
                "WARNING" => "warning",
                _ => "info"
            };
        }

        private string FormatMessage(MainInfo info)
        {
            var status = info.Status?.ToUpper() ?? "";

            return status switch
            {
                "RUNNING" => $"🔄 Calcul en cours pour {info.User} - Période {info.Salesperiod}",
                "COMPLETE" => $"✅ Calcul terminé avec succès pour {info.User}",
                "ERROR" or "FAILED" => $"❌ Erreur lors du calcul pour {info.User}",
                "PENDING" => $"⏳ Calcul en attente pour {info.User}",
                _ => $"Mise à jour : {status}"
            };
        }
    }
}
