using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnmpManager.API.Data;
using SnmpManager.API.Data.Repositories;
using SnmpManager.API.Services;

namespace SnmpManager.API.Controllers
{
    [Produces("application/json")]
    [Route("api/watchers")]
    public class WatchersController : Controller
    {
        private readonly IWatchersRepository _watchersRepository;
        private readonly IWatcherService _watcherService;

        public WatchersController(IWatchersRepository watchersRepository, IWatcherService watcherService)
        {
            _watchersRepository = watchersRepository;
            _watcherService = watcherService;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var watchers = _watchersRepository.GetAll();
            return Ok(watchers);
        }

        [HttpGet("{id}", Name = "GetSingleWatcher")]
        public IActionResult GetSignle(Guid id)
        {
            var watcherData = _watchersRepository.GetSingle(id);

            if (watcherData == null)
                return NotFound();
            
            return Ok(watcherData);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWatcherDto watcherData)
        {
            if (watcherData == null)
                return BadRequest();
            
            var data = new WatcherData
            {
                IpAddress = watcherData.IpAddress,
                Mib = watcherData.Mib,
                Method = watcherData.Method,
                UpdatesEvery = watcherData.UpdatesEvery
            };
            
            _watchersRepository.Create(data);
            _watcherService.StartNewWatcher(data);
            await _watchersRepository.SaveChangesAsync();

            return CreatedAtRoute("GetSingleWatcher", new {Controller = "Watchers", id = data.Id.ToString()}, data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var watcherData = _watchersRepository.GetSingle(id);
            
            if (watcherData == null)
                return NotFound();

            _watcherService.StopWatcher(watcherData.Id);
            _watchersRepository.Delete(watcherData);
            await _watchersRepository.SaveChangesAsync();

            return Accepted();
        }
    }
}