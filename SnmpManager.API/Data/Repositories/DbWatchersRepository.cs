using System;
using System.Linq;
using System.Threading.Tasks;

namespace SnmpManager.API.Models.Repositories
{
    public class DbWatchersRepository : IWatchersRepository
    {
        private readonly SnmpManagerContext _context;

        public DbWatchersRepository(SnmpManagerContext context)
        {
            _context = context;
        }
        public IQueryable<WatcherData> GetAll()
        {
            return _context.Watchers.AsQueryable();
        }

        public WatcherData GetSingle(Guid id)
        {
            return _context.Watchers.Find(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Create(WatcherData watcherData)
        {
            _context.Watchers.Add(watcherData);
        }

        public void Delete(WatcherData watcherData)
        {
            _context.Watchers.Remove(watcherData);
        }
    }
}