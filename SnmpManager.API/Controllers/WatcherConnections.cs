using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SnmpManager.API.Data.Repositories;
using SnmpManager.API.Services;

namespace SnmpManager.API.Controllers
{
    [Produces("application/json")]
    [Route("api/watcherConnections")]
    public class WatcherConnections : Controller
    {
        private readonly IWatcherService _watcherService;
        private readonly IWatchersRepository _watchersRepository;

        public WatcherConnections(IWatcherService watcherService, IWatchersRepository watchersRepository)
        {
            _watcherService = watcherService;
            _watchersRepository = watchersRepository;
        }

        [HttpGet]
        public IActionResult GetActiveConnections()
        {
            var connections = _watcherService.GetActiveConnections();
            return Ok(connections);
        }
        
        [HttpPost("{id}")]
        public IActionResult Connect(Guid id)
        {
            var watcher = _watchersRepository.GetSingle(id);
            _watcherService.StartNewWatcher(watcher);
            
            return Accepted();
        }
        
        [HttpDelete("{id}")]
        public IActionResult Disconnect(Guid id)
        {
            var watcher = _watchersRepository.GetSingle(id);
            _watcherService.StopWatcher(watcher.Id);

            return Accepted();
        }
    }
}