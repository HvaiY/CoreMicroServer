using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RuPeng.HystrixCore;

namespace MyWeb1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            RegisterServices(this.GetType().Assembly, services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            return services.BuildAspectCoreServiceProvider();
        }

        private static void RegisterServices(Assembly asm, IServiceCollection services)
        {
            //遍历程序集中的所有public类型
            foreach (Type type in asm.GetExportedTypes())
            {
                //判断类中是否有标注了CustomInterceptorAttribute的方法
                bool hasHystrixAttr = type.GetMethods()
                  .Any(m => m.GetCustomAttribute(typeof(HystrixCommandAttribute)) != null);
                if (hasHystrixAttr)
                {
                    services.AddSingleton(type);
                }
            }
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
