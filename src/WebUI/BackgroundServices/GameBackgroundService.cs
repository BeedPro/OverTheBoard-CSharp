using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OverTheBoard.Infrastructure.Tournaments;

namespace OverTheBoard.WebUI.BackgroundServices
{
    public class GameBackgroundService : BackgroundService
    {
        private readonly ILogger<GameBackgroundService> _logger;
        private readonly IServiceProvider _services;


        private readonly int _refreshIntervalInSeconds;

        public GameBackgroundService(IServiceProvider services, ILogger<GameBackgroundService> logger)
        {
            _logger = logger;
            _services = services;
             _refreshIntervalInSeconds = 60;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var services = scope.ServiceProvider.GetService<IEnumerable<IGameBackgroundService>>();
                    foreach (var service in services)
                    {
                        await service.ExecuteAsync();
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(_refreshIntervalInSeconds), stoppingToken);
            }
        }



    }
}
