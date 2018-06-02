using System.Collections.Generic;
using System.Linq;

namespace SnmpManager.API.Models.Repositories
{
    public class ActiveAgentsCache
    {
        private readonly HashSet<AgentDeviceInfo> _devices = new HashSet<AgentDeviceInfo>();
        private readonly object _lock = new object();
        
        public AgentDeviceInfo[] GetActiveDevices()
        {
            lock (_lock)
            {
                return _devices.ToArray();
            }
        }

        public void AddOrUpdateDevice(AgentDeviceInfo agentDeviceInfo)
        {
            lock (_lock)
            {
                _devices.Add(agentDeviceInfo);
            }
        }

        public void DeleteDevice(AgentDeviceInfo agentDeviceInfo)
        {
            lock (_lock)
            {
                _devices.Remove(agentDeviceInfo);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _devices.Clear();
            }
        }
    }
}