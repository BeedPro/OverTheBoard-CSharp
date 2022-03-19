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
    public class GameOutcomeChecker : BackgroundService
    {
        private readonly ILogger<GameOutcomeChecker> _logger;
        private readonly IServiceProvider _services;


        private readonly int _refreshIntervalInSeconds;

        public GameOutcomeChecker(ILogger<GameOutcomeChecker> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
             _refreshIntervalInSeconds = 60;
        }

   


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var _gameService = scope.ServiceProvider.GetService<IGameService>();
                    var gamesInProgress = await _gameService.GetGamesInProgress();
                    foreach (var game in gamesInProgress)
                    {
                        if (game.LastMoveAt.HasValue)
                        {
                            var player = game.Players.FirstOrDefault(e => e.Colour == game.NextMoveColour);
                            player.TimeRemaining = player.TimeRemaining - (DateTime.Now - game.LastMoveAt.Value);
                            var intTimeRemaining = Convert.ToInt32(player.TimeRemaining.TotalSeconds);
                            if (intTimeRemaining <= 0)
                            {
                                player.TimeRemaining = new TimeSpan(0, 0, 0);
                                if (player.Colour == "white")
                                {
                                    await _gameService.SaveGameOutcomeAsync(game.Identifier, EloOutcomesType.Lose, EloOutcomesType.Win);
                                }
                                else
                                {
                                    await _gameService.SaveGameOutcomeAsync(game.Identifier, EloOutcomesType.Win,
                                        EloOutcomesType.Lose);
                                }
                            }
                        }
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(_refreshIntervalInSeconds), stoppingToken);
            }
        }



    }
}
