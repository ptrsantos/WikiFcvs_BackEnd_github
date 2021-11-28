using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using WikiFCVS.Api.Configuration;

namespace WikiFCVS.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (ModoExecusao.IsDebug)
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            }
            else
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
            }

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
