using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using SnmpManager.API.Controllers;
using SnmpManager.API.Data;
using SnmpManager.API.Data.Repositories;
using SnmpManager.API.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SnmpManager.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();
            
            const string connection = @"Server=localhost;Database=SnmpManager;ConnectRetryCount=0;User Id=sa;password=0@8zmZIh"; 
            services.AddDbContext<SnmpManagerContext>(options => options.UseSqlServer(connection)); 
            
            services.AddSingleton<IHostedService, AgentsDiscoveryService>();
            services.AddSingleton<ActiveAgentsCache>();
            services.AddSingleton<AgentsHub>();
            services.AddSingleton<IWatcherService, WatcherService>(); //TODO to be refactored. Should not be a singleton
                
            services.AddTransient<IWatchersRepository, DbWatchersRepository>();
            services.AddTransient<ISnmpService, SnmpService>();
            
            services.AddCors( 
                options => options.AddPolicy("AllowCors", 
                    builder => 
                    { 
                        builder 
                            .AllowAnyOrigin() 
                            .AllowCredentials() 
                            .AllowAnyHeader() 
                            .AllowAnyMethod(); 
                    }) 
            ); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("AllowCors");
            app.UseSignalR(route =>
            {
                route.MapHub<AgentsHub>("/agents");
                route.MapHub<WatchersHub>("/watchers");
            });
            app.UseMvc();
        }
    }
}