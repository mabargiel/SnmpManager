using Lextm.SharpSnmpLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SnmpManager.API.Data
{
    public class TypedSnmpValue
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SnmpType Type { get; }
        public string Value { get; }

        public TypedSnmpValue(SnmpType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}