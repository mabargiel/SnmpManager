using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SnmpManager.API.Data
{
    public class WatcherData
    {
        public WatcherData()
        {
            UpdatesEvery = 1000;
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string IpAddress { get; set; }
        public string Mib { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Method Method { get; set; }
        public int UpdatesEvery { get; set; }
    }

    public enum Method
    {
        Walk,
        Get
    }
}