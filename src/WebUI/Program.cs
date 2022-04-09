using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OverTheBoard.Infrastructure;
using Serilog;
using Serilog.Events;

namespace OverTheBoard.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseLoggingCore()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
