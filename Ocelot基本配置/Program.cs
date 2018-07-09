﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ocelot基本配置
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://127.0.0.1:8888")
                .ConfigureAppConfiguration((hostingContext, builder) =>
                {
                    builder.AddJsonFile("MyConfiguration.json", false, true);
                })
                .UseStartup<Startup>()
                .Build();
    }
}
