using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Tournaments
{
    public class GameOutcomeChecker : IGameBackgroundService
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GameOutcomeChecker> _logger;

        public GameOutcomeChecker(IGameService gameService, ILogger<GameOutcomeChecker> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        public async Task<bool> ExecuteAsync()
        {
            var gamesInProgress = await _gameService.GetGamesInprogressAsync();
            foreach (var game in gamesInProgress)
            {
                _logger.LogInformation("Calculating outcome (InProgress) started for {Identifier}", game.Identifier);
                var outcome = CalculateInprogressGameOutcome(game);
                if (outcome.Precessed)
                {
                    await _gameService.SaveGameOutcomeAsync(game.Identifier, outcome.WhiteOutcome, outcome.BlackOutcome);
                }
                _logger.LogInformation("Calculating outcome (InProgress) ended for {Identifier}", game.Identifier);
            }

            var gameNotStarted = await _gameService.GetGamesNotStartedAndExpiredAsync();
            foreach (var game in gameNotStarted)
            {
                _logger.LogInformation("Calculating outcome (NotStarted) started for {Identifier}", game.Identifier);
                var outcome = CalculateNotStartedGameOutcome(game);
                if (outcome.Precessed)
                {
                    await _gameService.SaveGameOutcomeAsync(game.Identifier, outcome.WhiteOutcome, outcome.BlackOutcome);
                }
                _logger.LogInformation("Calculating outcome (NotStarted) ended for {Identifier}", game.Identifier);
            }

            return true;
        }

       private (bool Precessed, EloOutcomesType WhiteOutcome, EloOutcomesType BlackOutcome) CalculateNotStartedGameOutcome(ChessGame game)
        {

            var whitePlayer = game.Players.FirstOrDefault(f => f.Colour == "white");
            var blackPlayer = game.Players.FirstOrDefault(f => f.Colour == "black");
            var arriveStartTime = game.StartTime.AddMinutes(-5);
            var arriveEndTime = game.StartTime.AddMinutes(game.Period);

            if (string.IsNullOrEmpty(whitePlayer?.ConnectionId) && string.IsNullOrEmpty(blackPlayer?.ConnectionId))
            {
                return (true, EloOutcomesType.Draw, EloOutcomesType.Draw);
            }

            if (IsArrived(blackPlayer, arriveStartTime, arriveEndTime))
            {
                return (true, EloOutcomesType.Lose, EloOutcomesType.Win);
            }

            if (IsArrived(whitePlayer, arriveStartTime, arriveEndTime))
            {
                return (true, EloOutcomesType.Win, EloOutcomesType.Lose);
            }

            return (true, EloOutcomesType.Draw, EloOutcomesType.Draw);
        }
       
       private (bool Precessed, EloOutcomesType WhiteOutcome, EloOutcomesType BlackOutcome) CalculateInprogressGameOutcome(ChessGame game)
        {
            var gameLastMoveAt = game.StartTime;
            var nextMoveColour = "white";
            if (game.LastMoveAt.HasValue)
            {
                gameLastMoveAt = game.LastMoveAt.Value;
                nextMoveColour = game.NextMoveColour;
            }

            var player = game.Players.FirstOrDefault(e => e.Colour == nextMoveColour) ?? new GamePlayer();
            player.TimeRemaining = player.TimeRemaining - (DateTime.Now - gameLastMoveAt);
            var intTimeRemaining = Convert.ToInt32(player.TimeRemaining.TotalSeconds);
            if (player.Colour == "white")
            {
                return (intTimeRemaining <= 0, EloOutcomesType.Lose, EloOutcomesType.Win);
            }
            return (intTimeRemaining <= 0, EloOutcomesType.Win, EloOutcomesType.Lose);


        }

       private bool IsArrived(GamePlayer blackPlayer, DateTime arriveStartTime, DateTime arriveEndTime)
       {
           return !string.IsNullOrEmpty(blackPlayer?.ConnectionId) && (blackPlayer.LastConnectedTime > arriveStartTime && blackPlayer.LastConnectedTime < arriveEndTime);
       }
    }
}