using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using SnmpManager.API.Controllers;
using SnmpManager.API.Models;

namespace SnmpManager.API.Services
{
    public class SnmpService : ISnmpService
    {
        public async Task<WatcherSnmpData> Request(string ip, string mib, Method method)
        {
            switch (method)
            {
                case Method.Walk:
                    return await this.WalkAsync(ip, mib);
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }

        private async Task<WatcherSnmpData> WalkAsync(string ip, string mib)
        {
            var result = new List<Variable>();
            await Messenger.BulkWalkAsync(VersionCode.V3, new IPEndPoint(IPAddress.Parse(ip), 161), new OctetString("public"),
                new OctetString(string.Empty), new ObjectIdentifier(mib), result, 10, WalkMode.WithinSubtree, null,
                null);

            return new WatcherSnmpData(result.ToDictionary(variable => variable.Id.ToString(),
                variable => variable.Data.ToString()));
        }
    }
}