using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ProductService
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
            services.AddAuthentication("Bearer")
       .AddIdentityServerAuthentication(options =>
       {
           options.Authority = "http://localhost:9500";
           options.RequireHttpsMetadata = false;
       });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();

            string ip = Configuration["ip"];
            string port = Configuration["port"];
            string serviceName = "ProductService";
            string serviceId = serviceName + Guid.NewGuid();//唯一
            using (var consulClient = new ConsulClient(consulConfig))
            //using (var consulClient = new ConsulClient(c=> { c.Address = new Uri("http://127.0.0.1:8500");c.Datacenter = "dc1"; }))
            {
                AgentServiceRegistration asr = new AgentServiceRegistration();
                asr.Address = ip;
                asr.Port = Convert.ToInt32(port);
                asr.ID = serviceId;
                asr.Name = serviceName;
                asr.Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    HTTP = $"http://{ip}:{port}/api/Health",
                    Interval = TimeSpan.FromSeconds(10),
                    Timeout = TimeSpan.FromSeconds(5)
                };
                consulClient.Agent.ServiceRegister(asr).Wait();
            };
            appLifetime.ApplicationStopped.Register(() => {
                using (var consulClient = new ConsulClient(consulConfig))
                {
                    Console.WriteLine("应用退出，开始从consul注销");
                    consulClient.Agent.ServiceDeregister(serviceId).Wait();
                }
            });
        }

        private void consulConfig(ConsulClientConfiguration c)
        {
            c.Address = new Uri("http://127.0.0.1:8500");
            c.Datacenter = "dc1";
        }
    }
}
