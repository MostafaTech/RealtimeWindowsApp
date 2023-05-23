using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace RealTimeServer.SignalR
{
    public class RealtimeHub : Hub
    {
        private readonly ILogger<RealtimeHub> _logger;
        public RealtimeHub(ILogger<RealtimeHub> logger)
        {
            _logger = logger;
        }

        public async Task Register(string branch)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, branch);
            _logger.LogInformation("new branch registered: " + branch);
        }
    }
}
