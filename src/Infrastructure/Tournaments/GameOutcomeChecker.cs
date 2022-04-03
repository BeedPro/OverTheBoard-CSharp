using System;
using System.Linq;
using System.Threading.Tasks;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Tournaments
{
    public class GameOutcomeChecker : IGameBackgroundService
    {
        private readonly IGameService _gameService;

        public GameOutcomeChecker(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<bool> ExecuteAsync()
        {
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

            return true;
        }
    }
}