using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Shouldly;
using SnmpManager.API.Controllers;
using SnmpManager.API.Models;
using SnmpManager.API.Models.Repositories;
using SnmpManager.API.Services;
using Xunit;

namespace SnmpManager.API.UnitTests
{
    public class AgentsDiscoveryServiceTests
    {
        [Fact]
        public async Task Start_cancelled_cache_should_be_empty()
        {
            //arrange
            var cache = new ActiveAgentsCache();
            cache.AddOrUpdateDevice(new AgentDeviceInfo(string.Empty, string.Empty));
            var hub = new AgentsHub();
            var clientsMock = new Mock<IHubCallerClients>();
            var all = new Mock<IClientProxy>();
            clientsMock.Setup(x => x.All).Returns(all.Object);
            hub.Clients = clientsMock.Object;
            var service = new AgentsDiscoveryService(cache, hub);
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);
           
            //run
            await service.StartAsync(cts.Token);
            
            //assert
            cache.GetActiveDevices().ShouldBeEmpty();
        }
    }
}