using Microsoft.AspNetCore.Mvc;
using SnmpManager.API.Models;
using SnmpManager.API.Models.Repositories;

namespace SnmpManager.API.Controllers
{
    [Produces("application/json")]
    [Route("api/agents")]
    public class AgentsController : Controller
    {
        private readonly ActiveAgentsCache _activeAgentsCache;

        public AgentsController(ActiveAgentsCache activeAgentsCache)
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