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
            //if (!ModoExecusao.IsDebug)
            //{
            //    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
            //}
            //else
            //{
            //    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            //}

#if DEBUG
         Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
#else
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
#endif

        CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
