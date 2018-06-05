using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnmpManager.API.Models
{
    public class WatcherData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string IpAddress { get; set; }
        public string Mib { get; set; }
        public Method Method { get; set; }
        public int UpdatesEvery { get; set; }
    }

    public enum Method
    {
        Walk
    }
}