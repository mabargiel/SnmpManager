using System;
using System.Threading.Tasks;
using SnmpManager.API.Models;

namespace SnmpManager.API.Services
{
    public interface ISnmpService
    {
        Task<WatcherSnmpData> Request(string ip, string mib, Method method);
    }
}