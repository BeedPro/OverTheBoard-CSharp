using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OverTheBoard.Data.Entities;
using OverTheBoard.Data.Repositories;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.ObjectModel.Options;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Tournaments.Processors
{
    public class UnrankingGameCompletionProcessor : IGameCompletionProcessor
    {
        private readonly IGameService _gameService;
        private readonly ITournamentQueue _tournamentQueue;
        private readonly IUserService _userService;
        private readonly TournamentOptions _tournamentOption;

        public UnrankingGameCompletionProcessor(IGameService gameService, ITournamentQueue tournamentQueue, IUserService userService, IOptions<TournamentOptions> tournamentOptions)
        {
            _gameService = gameService;
            _tournamentQueue = tournamentQueue;
            _userService = userService;
            _tournamentOption = tournamentOptions.Value;
        }

        public bool CanSelect(GameCompletionQueueItem parameter, bool isDefault)
        {
            return parameter.Level == 0;
        }

        public async Task<bool> ProcessAsync(GameCompletionQueueItem item)
        {
            var game = await _gameService.GetChessGameWithPlayersAsync(item.Identifier);
            foreach (var player in game.Players)
            {
                var user = await _userService.GetUserAsync(player.UserId);
                if (user.Rank == UserRank.None && user.Rating >= _tournamentOption.ScoreForRanking)
                {
                    await _tournamentQueue.AddQueueAsync(new TournamentQueueItem()
                    {
                        UserId = user.Id,
                        Level = 1
                    });
                    await _userService.UpdateUserRankAsync(user.Id, UserRank.Level01);
                }
            }

            return true;
        }
    }
}