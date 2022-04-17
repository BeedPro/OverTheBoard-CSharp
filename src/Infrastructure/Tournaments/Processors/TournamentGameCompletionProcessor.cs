using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OverTheBoard.Data.Entities;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.ObjectModel;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Tournaments.Processors
{
    public class TournamentGameCompletionProcessor : IGameCompletionProcessor
    {
        private readonly IGameService _gameService;
        private readonly ITournamentService _tournamentService;
        private readonly ITournamentQueue _tournamentQueue;
        private readonly IUserService _userService;
        private readonly ILogger<TournamentGameCompletionProcessor> _logger;

        public TournamentGameCompletionProcessor(
            IGameService gameService, 
            ITournamentService tournamentService,
            ITournamentQueue tournamentQueue, 
            IUserService userService,
            ILogger<TournamentGameCompletionProcessor> logger)
        {
            _gameService = gameService;
            _tournamentService = tournamentService;
            _tournamentQueue = tournamentQueue;
            _userService = userService;
            _logger = logger;
        }
        public bool CanSelect(GameCompletionQueueItem parameter, bool isDefault)
        {
            return isDefault;
        }

        public async Task<bool> ProcessAsync(GameCompletionQueueItem item)
        {
            var game = await _gameService.GetChessGameWithPlayersAsync(item.Identifier);
            var isFinished = await _gameService.HasAllGameFinishedForTournamentAsync(game.TournamentId);

            if (isFinished)
            {
                var tournament = await _tournamentService.GetTournamentAsync(game.TournamentId);
                var games = await _gameService.GetMatchesByTournamentAsync(game.TournamentId);
                var gamePlayers = GetPlayers(games);
                var stats = GetStatus(tournament.Players, gamePlayers);
                var moveUpPoint = stats.Max(e=>e.point);
                var moveDownPoint = stats.Min(e=>e.point);
                var levelUp = item.Level + 1;
                var levelDown = item.Level > 1 ? item.Level - 1 : 1;


                foreach (var tuple in stats)
                {
                    
                    if(tuple.point == moveDownPoint)
                    {
                        await _tournamentQueue.AddQueueAsync(new TournamentQueueItem()
                        {
                            UserId = tuple.userId,
                            Level = levelDown
                        });
                        await _userService.UpdateUserRankAsync(tuple.userId, (UserRank)levelDown);
                    } 
                    else if (tuple.point == moveUpPoint)
                    {
                        await _tournamentQueue.AddQueueAsync(new TournamentQueueItem()
                        {
                            UserId = tuple.userId,
                            Level = levelUp
                        });
                        await _userService.UpdateUserRankAsync(tuple.userId, (UserRank)levelUp);
                    }
                    else
                    {
                        await _tournamentQueue.AddQueueAsync(new TournamentQueueItem()
                        {
                            UserId = tuple.userId,
                            Level = item.Level
                        });
                    }

                }

                await _tournamentService.FinishTournamentAsync(game.TournamentId);
                return true;
            }

            return false;
        }

        private List<GamePlayer> GetPlayers(List<ChessGame> games)
        {
            var gamePlayers = new List<GamePlayer>();

            foreach (var game in games)
            {
                gamePlayers.AddRange(game.Players);
            }

            return gamePlayers;
        }

        private List<(string userId, int point)> GetStatus(List<TournamentPlayer> players, List<GamePlayer> gamePlayers)
        {
            var points = new List<(string userId, int point)>();
            foreach (var player in players)
            {
                int point = Getpoints(player.UserId, gamePlayers);
                points.Add((player.UserId, point));
            }

            return points;
        }

        private int Getpoints(string userId, List<GamePlayer> gamePlayers)
        {
            int win = GetCount(userId, gamePlayers, EloOutcomesType.Win);
            int draw = GetCount(userId, gamePlayers, EloOutcomesType.Draw);

            return (win * TournamentConstants.WinPoint) + (draw * TournamentConstants.DrawPoint);
        }

        private static int GetCount(string userId, List<GamePlayer> gamePlayers, EloOutcomesType eloOutcomesType)
        {
            return gamePlayers.Count(e => e.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase) &&
                                          (e.Outcome ?? string.Empty).Equals(eloOutcomesType.ToString(),
                                              StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public class TournamentConstants
    {
        public const int WinPoint = 2;
        public const int LosePoint = 0;
        public const int DrawPoint = 1;
    }
}