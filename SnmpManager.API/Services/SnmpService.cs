using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
                
                Messenger.Walk(VersionCode.V1, receiver, new OctetString("public"), objectIdentifier, result, 2000,
                    WalkMode.WithinSubtree);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return new WatcherSnmpData(result.ToDictionary(variable => variable.Id.ToString(),
                variable => new TypedSnmpValue(variable.Data.TypeCode, variable.Data.ToString())));
        }

        private WatcherSnmpData Get(string ip, string mib)
        {
            try
            {
                var receiver = new IPEndPoint(IPAddress.Parse(ip), 161);
                var objectIdentifier = new ObjectIdentifier(mib);

                var variables = Messenger.Get(VersionCode.V3, receiver, new OctetString("public"),
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

    public class TypedSnmpValue
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SnmpType Type { get; }
        public string Data { get; }

        public TypedSnmpValue(SnmpType type, string data)
        {
            Type = type;
            Data = data;
        }
    }
}