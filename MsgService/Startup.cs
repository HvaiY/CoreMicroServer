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

namespace MsgService
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            string ip = Configuration["ip"];
            string port = Configuration["port"];
            string serviceName = "MsgService";
            string serviceId = serviceName + Guid.NewGuid();//唯一
            using (var consulClient = new ConsulClient(consulConfig)) //Consul对象
            //using (var consulClient = new ConsulClient(c=> { c.Address = new Uri("http://127.0.0.1:8500");c.Datacenter = "dc1"; }))
            {
                AgentServiceRegistration asr = new AgentServiceRegistration();
                asr.Address = ip;//当前服务地址 能被外界访问的地址
                asr.Port = Convert.ToInt32(port);//端口
                asr.ID = serviceId;//需要唯一
                asr.Name = serviceName;
                asr.Check = new AgentServiceCheck() { //健康检查
                    //服务停止多久后反注册(注销服务)
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    //健康检查地址/就是一个服务地址 
                    HTTP = $"http://{ip}:{port}/api/Health",
                    //健康检查时间间隔(心跳间隔)
                    Interval = TimeSpan.FromSeconds(10),
                    //超时 健康检查多久没响应就是超时
                    Timeout = TimeSpan.FromSeconds(5)
                };
                consulClient.Agent.ServiceRegister(asr).Wait();//注册该服务到Consul(异步的哦 )
            };
            //程序正常退出的时候从Consul注销服务
            //要通过方法参数注入 IApplicationLifetime
            appLifetime.ApplicationStopped.Register(()=> {
                using (var consulClient = new ConsulClient(consulConfig)) 
                {
                    Console.WriteLine("应用退出，开始从consul注销");
                    //注销指定编号的服务
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
