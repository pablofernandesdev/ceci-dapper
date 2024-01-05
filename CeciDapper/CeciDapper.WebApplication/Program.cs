using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CeciDapper.WebApplication
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    //validates whether the "PORT" variable exists in the environment
                    if (System.Environment.GetEnvironmentVariable("PORT") != null)
                    {
                        webBuilder.UseKestrel(options =>
                        {
                            options.ListenAnyIP(Int32.Parse(System.Environment.GetEnvironmentVariable("PORT")));
                        });
                    }
                });
    }
}
