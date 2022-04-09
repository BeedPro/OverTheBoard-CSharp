using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OverTheBoard.Infrastructure.Common;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.Infrastructure.Tournaments;
using OverTheBoard.Infrastructure.Tournaments.Processors;
using Serilog;
using Serilog.Events;

namespace OverTheBoard.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        private const string logging_section_name = "OverTheBoardLogging";

        public static IHostBuilder UseLoggingCore(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, collection) =>
            {
                collection.Configure<LoggingOption>(context.Configuration.GetSection(logging_section_name));
            });

            hostBuilder.UseSerilogConfig((context, services, configuration, option) =>
                configuration
                    .MinimumLevel.Is(option.LogLevel)
                    .Enrich.FromLogContext()
                    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            );

            return hostBuilder;
        }

        public static IHostBuilder UseSerilogConfig(
            this IHostBuilder builder,
            Action<HostBuilderContext, IServiceProvider, LoggerConfiguration, LoggingOption> configureLogger,
            bool preserveStaticLogger = false,
            bool writeToProviders = false)
        {

            Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> loggerAction =
                (context, services, configuration) =>
                {
                    var option = services.GetService<IOptions<LoggingOption>>()?.Value ?? new LoggingOption();
                    configureLogger(context, services, configuration, option);
                };

            builder.UseSerilog(loggerAction, preserveStaticLogger, writeToProviders);

            return builder;
        }

      
        public class LoggingOption
        {
            public LogEventLevel LogLevel { get; set; }
           
        }

    }
}
