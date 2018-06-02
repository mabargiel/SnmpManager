using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SnmpManager.API.Controllers;
using SnmpManager.API.Models;
using SnmpManager.API.Models.Repositories;

namespace SnmpManager.API.Services
{
    public class AgentsDiscoveryService : IHostedService
    {
        private readonly ActiveAgentsCache _activeAgentsCache;
        private readonly AgentsHub _agentsHubContext;

        public AgentsDiscoveryService(ActiveAgentsCache activeAgentsCache, AgentsHub agentsHubContext)
        {
            _activeAgentsCache = activeAgentsCache;
            _agentsHubContext = agentsHubContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var discoverer = new Discoverer();
            
            if(cancellationToken.IsCancellationRequested)
                return;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var agents = new List<string>();
            
                    void OnAgentFound(object sender, AgentFoundEventArgs args)
                    {
                        agents.Add(args.Agent.Address.ToString());
                    };
            
                    discoverer.AgentFound += OnAgentFound;
                    discoverer.AgentFound += OnAgentFound;
                    await discoverer.DiscoverAsync(VersionCode.V3, new IPEndPoint(IPAddress.Broadcast, 161), new OctetString("public"), 5000);

                    var discoveredAgents = agents.Select(x =>
                        new AgentDeviceInfo(x, VersionCode.V3.ToString())).ToArray();
                
                    var currentAgents = _activeAgentsCache.GetActiveDevices();
                    var newAgents = discoveredAgents.Except(currentAgents);
                    var inactiveAgents = currentAgents.Except(discoveredAgents);

                    foreach (var newAgent in newAgents)
                    {
                        await _agentsHubContext.NewAgentFound(newAgent, cancellationToken);
                        _activeAgentsCache.AddOrUpdateDevice(newAgent);
                        Console.WriteLine($"New agent discovered: {newAgent}");
                    }

                    foreach (var inactiveAgent in inactiveAgents)
                    {
                        await _agentsHubContext.Clients.All.SendAsync("AGENT_DISCONNECTED", inactiveAgent, cancellationToken);
                        _activeAgentsCache.DeleteDevice(inactiveAgent);
                        Console.WriteLine($"Agent ({inactiveAgent}) became inactive");
                    }
                
                    await Task.Delay(5000, cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {
                _activeAgentsCache.Clear();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}