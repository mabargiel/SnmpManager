using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SnmpManager.API.Controllers
{
    //TODO ANTI PATTERN!! to be refactored
    public static class WatcherConnectionsHandler
    {
        public static readonly HashSet<string> ConnectedIds = new HashSet<string>();
    }
    
    public class WatchersHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            WatcherConnectionsHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            WatcherConnectionsHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}