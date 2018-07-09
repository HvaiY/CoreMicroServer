using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Ocelot_Consul
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Ocelot 文档地址： http://ocelot.readthedocs.io/en/latest/features/servicediscovery.html#consul

            //访问地址 类型于 ： http://localhost:5536/ProductService/Product/1
            /**有多个服务就 ReRoutes 下面配置多组即可
            访 问 http://localhost:8888/MsgService/SMS/Send_MI 即 可 ， 请 求 报 文 体
            { phoneNum: "110",msg: "aaaaaaaaaaaaa"}。
            表示只要是 / MsgService / 开头的都会转给后端的服务名为" MsgService "的一台服务器，转
                发的路径是"/api/{url}"。LoadBalancerOptions 中"LeastConnection"表示负载均衡算法是“选
                择当前最少连接数的服务器”，如果改为 RoundRobin 就是“轮询”。ServiceDiscoveryProvider
                是 Consul 服务器的配置。
            Ocelot 因为是流量中枢，也是可以做集群的。**/

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOcelot().Wait();

        }
    }
}
