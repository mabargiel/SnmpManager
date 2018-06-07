using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SnmpManager.API.Controllers;
using SnmpManager.API.Models;

namespace SnmpManager.API.Services
{
    public class WatcherService : IWatcherService
    {
        private readonly IHubContext<WatchersHub> _watcherHubContext;
        private readonly ISnmpService _snmpService;

        private readonly Dictionary<Guid, (CancellationTokenSource cts, int)> _activeWatchers =
            new Dictionary<Guid, (CancellationTokenSource, int)>();

        public WatcherService(IHubContext<WatchersHub> watcherHubContext, ISnmpService snmpService)
        {
            _watcherHubContext = watcherHubContext;
            _snmpService = snmpService;
        }
        
        public void StartNewWatcher(WatcherData watcherData)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            
            if(WatcherConnectionsHandler.ConnectedIds.Any(id => id == watcherData.Id.ToString()))
                return;

            Task.Run(async () =>
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.CompletedTask;
                
                while (!cancellationToken.IsCancellationRequested)
                {
                    if(WatcherConnectionsHandler.ConnectedIds.Any())
                    {
                        var watcherPingData = await _snmpService.Request(watcherData.IpAddress, watcherData.Mib, watcherData.Method);

                        await _watcherHubContext.Clients.All.SendAsync("WATCHER-DATA-RECEIVED", new { Id=watcherData.Id.ToString(), Data=watcherPingData},
                            cancellationToken);
                        Console.WriteLine("DATA_SENT");
                    }

                    await Task.Delay(watcherData.UpdatesEvery, cancellationToken);
                }

                return Task.CompletedTask;

            }, cancellationToken);

            _activeWatchers[watcherData.Id] = (cts, watcherData.UpdatesEvery);
        }

        public void StopWatcher(Guid id)
        {
            if(!_activeWatchers.ContainsKey(id))
                return;
            
            _activeWatchers[id].cts.Cancel();
            _activeWatchers.Remove(id);
        }

        public Guid[] GetActiveConnections()
        {
            return _activeWatchers.Keys.ToArray();
        }
    }
}