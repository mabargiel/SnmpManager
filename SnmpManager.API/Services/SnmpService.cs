using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using SnmpManager.API.Controllers;
using SnmpManager.API.Data;

namespace SnmpManager.API.Services
{
    public class SnmpService : ISnmpService
    {
        public WatcherSnmpData Request(string ip, string mib, Method method)
        {
            switch (method)
            {
                case Method.Walk:
                    return this.Walk(ip, mib);
                case Method.Get:
                    return this.Get(ip, mib);
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, null);
            }
        }

        private WatcherSnmpData Walk(string ip, string mib)
        {
            var result = new List<Variable>();

            try
            {
                var receiver = new IPEndPoint(IPAddress.Parse(ip), 161);
                var objectIdentifier = new ObjectIdentifier(mib);
                
                Messenger.Walk(VersionCode.V2, receiver, new OctetString("public"), objectIdentifier, result, 60000,
                    WalkMode.WithinSubtree);

                return new WatcherSnmpData(result.ToDictionary(variable => variable.Id.ToString(),
                    variable => new TypedSnmpValue(variable.Data.TypeCode, variable.Data.ToString())));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private WatcherSnmpData Get(string ip, string mib)
        {
            try
            {
                var receiver = new IPEndPoint(IPAddress.Parse(ip), 161);
                var objectIdentifier = new ObjectIdentifier(mib);

                var variables = Messenger.Get(VersionCode.V2, receiver, new OctetString("public"),
                    new List<Variable> {new Variable(objectIdentifier)}, 2000);

                return new WatcherSnmpData(variables.ToDictionary(variable => variable.Id.ToString(),
                    variable => new TypedSnmpValue(variable.Data.TypeCode, variable.Data.ToString())));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}