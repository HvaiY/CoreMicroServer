using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ID4.Ocelot
{
    public class Startup
    {
        private IConfiguration iconfiguration;
        public Startup(IConfiguration configuration)
        {
            iconfiguration = configuration;
        }
        //获取包  IdentityServer4  和Ocelot
        public void ConfigureServices(IServiceCollection services)
        {//指定Identity Server的信息
            Action<IdentityServerAuthenticationOptions> isaOptMsg = o =>
            {
                o.Authority = "http://127.0.0.1:9500";
                o.ApiName = "MsgAPI";//要连接的应用的名字
                o.RequireHttpsMetadata = false;
                o.SupportedTokens = SupportedTokens.Both;
                o.ApiSecret = "123321";//秘钥
            };
            Action<IdentityServerAuthenticationOptions> isaOptProduct = o =>
            {
                o.Authority = "http://127.0.0.1:9500";
                o.ApiName = "ProductAPI";//要连接的应用的名字
                o.RequireHttpsMetadata = false;
                o.SupportedTokens = SupportedTokens.Both;
                o.ApiSecret = "123321";//秘钥
            };
            services.AddAuthentication()
            //对配置文件中使用ChatKey配置了AuthenticationProviderKey=MsgKey
            //的路由规则使用如下的验证方式
            .AddIdentityServerAuthentication("MsgKey", isaOptMsg)
            .AddIdentityServerAuthentication("ProductKey", isaOptProduct);
            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOcelot().Wait();

            /*
             注意：在获取token和Ocelot中配置identityServer的时候的域名一定要一致，不能一个是
http://127.0.0.1:9500，一个是http://localhost:9500，否则会报401错误。
启动 Ocelot 服务器，然后向 ocelot 请求/MsgService/SMS/Send_MI （报文体还是要传 json
数据），在请求头（不是报文体）里加上：
Authorization="Bearer "+上面 identityserver 返回的 accesstoken
             */
             /*需要启动msg和Product 服务 还有 consul  以及Identity服务 和这个检验的Ocelot.ID4*/
        }
    }
}
