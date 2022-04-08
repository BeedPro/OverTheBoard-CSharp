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
        private readonly ITournamentService _tournamentService;
        private readonly ITournamentEmailService _emailService;
        private readonly IUserService _userService;
        private readonly TournamentOptions _options;

        public TournamentQueueReceiver(
            ITournamentQueue tournamentQueue, 
            IGameService gameService,
            ITournamentService tournamentService,
            ITournamentEmailService emailService, 
            IUserService userService,
            IOptions<TournamentOptions> options)
        {
            _tournamentQueue = tournamentQueue;
            _gameService = gameService;
            _tournamentService = tournamentService;
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
                    var tournamentIdentifier = await CreateGameAsync(divisions, players, level);
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
                var matches = await _gameService.GetMatchesByTournamentAndUserAsync(player.UserId, tournamentIdentifier);
                var user = await _userService.GetUserAsync(player.UserId);
                await _emailService.SendInitialEmailAsync(user.Email, matches);
            }
        }

        private async Task<string> CreateGameAsync(List<DivisionItem> divisions, List<TournamentQueueItem> players, int level)
        {
            var tournamentIdentifier = Guid.NewGuid().ToString();
            DateTime startDate = DateTime.Now;
            var numberOfRound = divisions.GroupBy(e => e.RoundNumber).Count();
            DateTime endDate = DateTime.Now.AddDays(numberOfRound + 2);
            await _tournamentService.CreateTournamentAsync(tournamentIdentifier, players, startDate, endDate);

            foreach (var division in divisions)
            {
                var divisionRoundNumber = division.RoundNumber + 1;
                var gameIdentifier = Guid.NewGuid().ToString();
                await _gameService.CreateGameAsync(gameIdentifier, division.GameQueueItems, GetDate(divisionRoundNumber), 15,
                    GameType.Ranked, divisionRoundNumber, level, tournamentIdentifier);
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