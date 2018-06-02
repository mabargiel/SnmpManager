using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SnmpManager.API.Models;

namespace SnmpManager.API.Controllers
{
    public class AgentsHub : Hub
    {
        public async Task NewAgentFound(AgentDeviceInfo agent, CancellationToken cancellationToken)
        {
            if(Clients == null)
                return;

            await Clients.All.SendAsync("NEW-AGENT-FOUND", agent, cancellationToken);
        }
    }
}