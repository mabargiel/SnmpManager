using System.Collections.Generic;

namespace SnmpManager.API.Models
{
    public struct WatcherSnmpData
    {
        public Dictionary<string, string> Variables { get; }

        public WatcherSnmpData(Dictionary<string, string> variables)
        {
            Variables = variables;
        }
    }
}