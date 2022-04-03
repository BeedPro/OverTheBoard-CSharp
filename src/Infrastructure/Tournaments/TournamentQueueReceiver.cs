using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OverTheBoard.Data.Entities;
using OverTheBoard.Infrastructure.Common;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.ObjectModel.Options;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Tournaments
{
    public class TournamentQueueReceiver : IGameBackgroundService
    {
        private readonly ITournamentQueue _tournamentQueue;
        private readonly IGameService _gameService;
        private readonly ITournamentEmailService _emailService;
        private readonly IUserService _userService;
        private readonly TournamentOptions _options;

        public TournamentQueueReceiver(
            ITournamentQueue tournamentQueue, 
            IGameService gameService, 
            ITournamentEmailService emailService, 
            IUserService userService,
            IOptions<TournamentOptions> options)
        {
            _tournamentQueue = tournamentQueue;
            _gameService = gameService;
            _emailService = emailService;
            _userService = userService;
            _options = options.Value;
        }

        public async Task<bool> ExecuteAsync()
        {
            var levels = await _tournamentQueue.GetAvailableLevels();
            foreach (var level in levels)
            {
                var hasLevel = await _tournamentQueue.HasRequiredPlayersInLevel(_options.PlayersPerGroup, level);
                if (hasLevel)
                {
                    var players = await _tournamentQueue.GetGameQueueItems(_options.PlayersPerGroup, level);
                    var robinEngine = new RoundRobinEngine();
                    var divisions = robinEngine.BuildMatches(players, _options.NumberOfIteration);
                    var tournamentIdentifier = await CreateGameAsync(divisions);
                    await SendInitialEmailAsync(players, tournamentIdentifier);
                    await _tournamentQueue.RemoveGameQueueItems(players);
                }
            }

            return true;
        }

        private async Task SendInitialEmailAsync(List<TournamentQueueItem> players, string tournamentIdentifier)
        {
            foreach (var player in players)
            {
                var matches = await _gameService.GetMatchesByTournamentAsync(player.UserId, tournamentIdentifier);
                var user = await _userService.GetUserAsync(player.UserId);
                await _emailService.SendInitialEmailAsync(user.Email, matches);
            }
        }

        private async Task<string> CreateGameAsync(List<DivisionItem> divisions)
        {
            var tournamentIdentifier = Guid.NewGuid().ToString();
            foreach (var division in divisions)
            {
                var gameIdentifier = Guid.NewGuid().ToString();
                await _gameService.CreateGameAsync(gameIdentifier, division.GameQueueItems, GetDate(division.RoundNumber), 15,
                    GameType.Ranked, tournamentIdentifier);
            }

            return tournamentIdentifier;
        }

        private DateTime GetDate(in int roundNumber)
        {
            var today = DateTime.Now;
            var date = new DateTime(today.Year, today.Month, today.Day, 18,0,0);
            return date.AddDays(roundNumber);
        }
    }
}