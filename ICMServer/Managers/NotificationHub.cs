using ICMServer.DBContext;
using ICMServer.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ICMServer.Managers
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        // Appelable par Angular avec hubConnection.invoke("GetLastRunInfo")
        public async Task<object?> GetLastRunInfo()
        {
            using var scope = Context.GetHttpContext()!.RequestServices.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository>();

            return new
            {
                type = "minute-tick",
                serverTimeUtc = DateTime.UtcNow,
                data = repository.GetMainInfo()
            };

        }
    }
}
