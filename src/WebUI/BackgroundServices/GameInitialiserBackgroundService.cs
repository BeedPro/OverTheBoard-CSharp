using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.Infrastructure.Tournaments;
using OverTheBoard.ObjectModel;
using OverTheBoard.WebUI.SignalR;

namespace OverTheBoard.WebUI.BackgroundServices
{
    public class GameInitialiserBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly IUnrankedInitialiserQueue _initialiserQueue;
        private readonly IHubContext<GameMessageHub> _hubContext;

        public GameInitialiserBackgroundService(IServiceProvider services, IUnrankedInitialiserQueue initialiserQueue, IHubContext<GameMessageHub> hubContext, ILogger<GameBackgroundService> logger)
        {
            _services = services;
            _initialiserQueue = initialiserQueue;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var gameService = scope.ServiceProvider.GetService<IGameService>();

                    var nextItem = _initialiserQueue.GetNextItem();
                    while (nextItem != null)
                    {
                        var chessMoves = await gameService.InitialiseChessGameAsync(nextItem.GameId, string.Empty);
                        foreach (var move in chessMoves)
                        {
                            await _hubContext.Clients.Client(move.Key).SendAsync("Initialised", move.Value);
                        }
                        nextItem = _initialiserQueue.GetNextItem();
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}