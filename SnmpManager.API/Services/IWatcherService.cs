using System;
using SnmpManager.API.Models;

namespace SnmpManager.API.Services
{
    public interface IWatcherService
    {
        void StartNewWatcher(WatcherData watcherData, int updatesEvery);
        void StopWatcher(Guid id);
        Guid[] GetActiveConnections();
    }
}