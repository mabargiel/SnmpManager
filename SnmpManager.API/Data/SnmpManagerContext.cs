using Microsoft.EntityFrameworkCore;

namespace SnmpManager.API.Models
{
    public class SnmpManagerContext : DbContext
    {
        public SnmpManagerContext(DbContextOptions<SnmpManagerContext> context)
            :base(context)
        {
            
        }
        
        public DbSet<WatcherData> Watchers { get; set; }
    }
}