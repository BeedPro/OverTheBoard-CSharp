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

namespace OverTheBoard.Infrastructure.BackgroundServices
{
    public class MatchMakingUnrankedQueue : BackgroundService
    {
        private readonly ILogger<MatchMakingUnrankedQueue> _logger;
        private readonly IServiceProvider _services;


        private readonly int _refreshIntervalInSeconds;

        public MatchMakingUnrankedQueue(ILogger<MatchMakingUnrankedQueue> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
             _refreshIntervalInSeconds = 60;
        }

   


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(_refreshIntervalInSeconds), stoppingToken);
        }



    }
}
