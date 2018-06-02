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
        
        public void StartNewWatcher(WatcherData watcherData, int updatesEvery)
        {
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            Task.Run(async () =>
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.CompletedTask;
                
                while (!cancellationToken.IsCancellationRequested)
                {
                    if(!WatcherConnectionsHandler.ConnectedIds.Any())
                    {
                        var watcherPingData = _snmpService.Request(watcherData.IpAddress, watcherData.Mib, watcherData.Method);
                        await _watcherHubContext.Clients.All.SendAsync("WATCHER-DATA", watcherPingData,
                            cancellationToken);
                    }

                    await Task.Delay(updatesEvery, cancellationToken);
                }

                return Task.CompletedTask;

            }, cancellationToken);

            _activeWatchers[watcherData.Id] = (cts, updatesEvery);
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