using System;
using SnmpManager.API.Models;

namespace SnmpManager.API.Services
{
    public interface IWatcherService
    {
        void StartNewWatcher(WatcherData watcherData);
        void StopWatcher(Guid id);
        Guid[] GetActiveConnections();
    }
}