using Microsoft.AspNetCore.Mvc;
using SnmpManager.API.Models;
using SnmpManager.API.Models.Repositories;

namespace SnmpManager.API.Controllers
{
    [Produces("application/json")]
    [Route("api/activeDevices")]
    public class ActiveDevicesController : Controller
    {
        private readonly ActiveAgentsCache _activeAgentsCache;

        public ActiveDevicesController(ActiveAgentsCache activeAgentsCache)
        {
            _activeAgentsCache = activeAgentsCache;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var devices = _activeAgentsCache.GetActiveDevices();
            return Ok(devices);
        }
    }
}