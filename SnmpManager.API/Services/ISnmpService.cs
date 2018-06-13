using System;
using System.Threading.Tasks;
using SnmpManager.API.Data;

namespace SnmpManager.API.Services
{
    public interface ISnmpService
    {
        WatcherSnmpData Request(string ip, string mib, Method method);
    }
}