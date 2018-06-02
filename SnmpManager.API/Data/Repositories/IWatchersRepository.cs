using System;
using System.Linq;
using System.Threading.Tasks;

namespace SnmpManager.API.Models.Repositories
{
    public interface IWatchersRepository
    {
        IQueryable<WatcherData> GetAll();
        WatcherData GetSingle(Guid id);
        Task SaveChangesAsync();
        void Create(WatcherData watcherData);
        void Delete(WatcherData watcherData);
    }
}