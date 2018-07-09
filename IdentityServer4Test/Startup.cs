using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4Test
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //使用IdentityServer4 服务
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //使用ID4
            app.UseIdentityServer();
            /*然后在 9500 端口启动
在 postman 里发出请求，获取 token
http://localhost:9500/connect/token，发 Post 请求，表单请求内容（注意不是报文头）：
client_id=clientPC1 client_secret=123321 grant_type=client_credentials*/

            /*返回值
             {
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjA0YTcyYjY1ZGE5OTVkZjA3M2JhNWE2OGUzMDc0ZGRjIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MzExMzkxNjIsImV4cCI6MTUzMTE0Mjc2MiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NTAwMC9yZXNvdXJjZXMiLCJNc2dBUEkiLCJQcm9kdWN0QVBJIl0sImNsaWVudF9pZCI6ImNsaWVudFBDMSIsInNjb3BlIjpbIk1zZ0FQSSIsIlByb2R1Y3RBUEkiXX0.fw0zqkohsfhp_kwDlel5kcLncUN7lWTcYNHi1J5B7cAX_PTVdv7GkTIX-4MO4PUyBR9hJc9CB5plakS2uOAsdxgBAqysN2TLe2JmGFKwsU6co-9uvBw_4hQ0qxK-wE4VoyjGBvxoPw9YgtUVtJxmdsxIyKQ_ssgr1hpaGT19PjZGfI1ypxUmv9T_5OBwyMfKsDpV9nBKU83Gs_itOxVcpv-NGIl_2iIYzfH1fzORcLRugAL3WlcwyFA-DncbO-LDqFnPmdy4fGM6Z7BWVdPmdxAh9zUqf7lvggaw5zSexK5rlbsYxE-qTUhExHH_el1TZwmIKmD5ZTQr_5mwXTTzxg",
    "expires_in": 3600,
    "token_type": "Bearer"
}
             */
        }
    }
}
