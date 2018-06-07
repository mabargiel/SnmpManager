using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
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

            try
            {
                IPEndPoint receiver = new IPEndPoint(IPAddress.Parse(ip), 161);


                var objectIdentifier = new ObjectIdentifier(mib);
                Messenger.Walk(VersionCode.V1, receiver, new OctetString("public"), objectIdentifier, result, 1000,
                    WalkMode.WithinSubtree);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            
            return new WatcherSnmpData(result.ToDictionary(variable => variable.Id.ToString(),
                variable => variable.Data.ToString()));
        }
    }
}