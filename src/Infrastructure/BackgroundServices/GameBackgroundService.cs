using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Data.Repositories;
using OverTheBoard.Infrastructure.Extensions;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OverTheBoard.Infrastructure.Tournaments;

namespace OverTheBoard.Infrastructure.BackgroundServices
{
    public class GameBackgroundService : BackgroundService
    {
        private readonly ILogger<GameBackgroundService> _logger;
        private readonly IServiceProvider _services;


        private readonly int _refreshIntervalInSeconds;

        public GameBackgroundService(ILogger<GameBackgroundService> logger, IServiceProvider services)
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
                        await service.ProcessAsync();
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(_refreshIntervalInSeconds), stoppingToken);
            }
        }



    }
}
