using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;

namespace ICMServer.Managers
{
    public class ProcessQueueService : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<ProcessQueueService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Channel<Func<IServiceProvider, Task>> _queue; // Changement de signature!

        public ProcessQueueService(
            IHubContext<NotificationHub> hubContext,
            ILogger<ProcessQueueService> logger, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _logger = logger;
            _queue = Channel.CreateUnbounded<Func<IServiceProvider, Task>>(new UnboundedChannelOptions
            {
                SingleReader = true,  // une seule tâche consommatrice
                SingleWriter = false  // plusieurs producteurs possibles
            });
        }

        // Méthode publique pour ajouter un job à la queue
        public ValueTask Enqueue(Func<IServiceProvider, Task> job) // ✅ Changement ici
        {
            if (!_queue.Writer.TryWrite(job))
            {
                _logger.LogWarning("Impossible d'ajouter un job : le writer du channel est fermé ou completé.");
                throw new InvalidOperationException("Le writer du channel est fermé.");
            }

            _logger.LogInformation("Job ajouté à la file d’attente.");
            return ValueTask.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"ProcessQueueService started on thread {Environment.CurrentManagedThreadId}");

            try
            {
                await foreach (var job in _queue.Reader.ReadAllAsync(stoppingToken))
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        _logger.LogInformation("Exécution d’un job...");
                        try
                        {
                            await job(scope.ServiceProvider);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Erreur dans la tâche de la queue");
                            await SafeNotifyErrorAsync(ex);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("ProcessQueueService arrêté par token d’annulation.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception fatale dans ExecuteAsync — le service s’arrête.");
            }
            finally
            {
                _logger.LogWarning("ExecuteAsync terminé — la queue ne sera plus lue !");
            }
        }

        private async Task SafeNotifyErrorAsync(Exception ex)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                try
                {
                    await _hubContext.Clients.All.SendAsync("QueueError", new
                    {
                        error = ex.Message,
                        time = DateTime.Now
                    });
                }
                catch (Exception innerEx)
                {
                    _logger.LogError(innerEx, "Erreur lors de l’envoi du message SignalR");
                }
            }
        }
    }
}
