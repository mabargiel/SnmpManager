using System.Collections.Generic;
using SnmpManager.API.Services;

namespace SnmpManager.API.Data
{
    public struct WatcherSnmpData
    {
        public Dictionary<string, TypedSnmpValue> Variables { get; }

        public WatcherSnmpData(Dictionary<string, TypedSnmpValue> variables)
        {
            Variables = variables;
        }
    }
}