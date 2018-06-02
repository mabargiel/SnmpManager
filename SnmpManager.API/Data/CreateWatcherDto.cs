namespace SnmpManager.API.Models
{
    public class CreateWatcherDto
    {
        public string IpAddress { get; set; }
        public string Mib { get; set; }
        public int UpdatesEvery { get; set; }
        public Method Method { get; set; }
    }
}