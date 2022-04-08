using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OverTheBoard.Data.Entities;
using OverTheBoard.ObjectModel;
using OverTheBoard.WebUI.Models;

namespace OverTheBoard.WebUI.ModelPopulators
{
    public class BracketsViewModelPopulator : IBracketsViewModelPopulator
    {
        public BracketsViewModel Populate(string currentUserId, Tournament tournament, List<ChessGame> games, Dictionary<string, OverTheBoardUser> users)
        {
            var bracketsViewModel = new BracketsViewModel() { Items = new Dictionary<int, BracketsRoundItemModel>() };

            foreach (var game in games.OrderBy(e => e.RoundNumber))
            {
                var bracketsGameModel = GetGame(game, users, currentUserId);
                if (bracketsViewModel.Items.ContainsKey(game.RoundNumber))
                {
                    bracketsViewModel.Items[game.RoundNumber].Games.Add(bracketsGameModel);
                }
                else
                {
                    bracketsViewModel.Items.Add(game.RoundNumber, new BracketsRoundItemModel(){Games = new List<BracketsGameModel>(){ bracketsGameModel } });
                }
            }

            bracketsViewModel.StatModels = GetStats(currentUserId, tournament, games, users);


            return bracketsViewModel;
        }

        private List<BracketsPlayerStatsModel> GetStats(string currentUserId, Tournament tournament, List<ChessGame> games, Dictionary<string, OverTheBoardUser> users)
        {
            var players = GetPlayers(games);
            var statsModels = new List<BracketsPlayerStatsModel>();
            foreach (var player in tournament.Players)
            {
                var user = users.ContainsKey(player.UserId) ? users[player.UserId] : new OverTheBoardUser() { DisplayName = "No User" };
                var stats = new BracketsPlayerStatsModel();
                stats.DisplayName = user.DisplayName;
                stats.Win = StatsForOutcome(players, player, EloOutcomesType.Win);
                stats.Lose = StatsForOutcome(players, player, EloOutcomesType.Lose);
                stats.Draw = StatsForOutcome(players, player, EloOutcomesType.Draw);
                stats.Point = GetPoints(stats);
                stats.Matches = stats.Win + stats.Lose + stats.Draw;
                stats.TotalMatches = StatsForTotalMatches(players, player);
                statsModels.Add(stats);
            }

            return statsModels.OrderByDescending(e => $"{(e.Point * 10) + e.Matches}").ToList();
        }

        private decimal GetPoints(BracketsPlayerStatsModel stats)
        {
            decimal statsWin = stats.Win * 2;
            decimal statsDraw = stats.Draw * 1;
            return statsWin + statsDraw;
        }

        private static int StatsForOutcome(List<GamePlayer> players, TournamentPlayer player, EloOutcomesType eloOutcomesType)
        {
            return players.Count(e =>
                e.UserId.Equals(player.UserId, StringComparison.InvariantCultureIgnoreCase) &&
                (e.Outcome??string.Empty).Equals(eloOutcomesType.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }

        private static int StatsForTotalMatches(List<GamePlayer> players, TournamentPlayer player)
        {
            return players.Count(e =>
                e.UserId.Equals(player.UserId, StringComparison.InvariantCultureIgnoreCase));
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

        private BracketsGameModel GetGame(ChessGame game, Dictionary<string, OverTheBoardUser> users, string currentUserId)
        {
            var bracketsGameModel = new BracketsGameModel()
            {
                Identifier = game.Identifier,
                Status = game.Status,
                Players = new List<BracketsPlayerModel>(),
                StartDate = game.StartTime.ToString("yyyy/MM/dd"),
                StartAt = game.StartTime.ToString("HH:mm")
            };

            var canPlay = false;
            foreach (var player in game.Players)
            {
                var user = users.ContainsKey(player.UserId) ? users[player.UserId] : new OverTheBoardUser(){DisplayName = "No User"};
                var bracketsPlayerModel = new BracketsPlayerModel()
                {
                    DisplayName = user.DisplayName,
                    Outcome = player.Outcome,
                    Colour = GetColour(player.Colour)
                };

                if (player.UserId.Equals(currentUserId, StringComparison.InvariantCultureIgnoreCase))
                {
                    bracketsPlayerModel.IsSelf = true;
                    canPlay = game.StartTime < DateTime.Now.AddMinutes(5) && game.StartTime > DateTime.Now.AddMinutes(game.Period * -1);
                }

                bracketsGameModel.Players.Add(bracketsPlayerModel);
            }

            bracketsGameModel.CanPlay = canPlay;
            //bracketsGameModel.CanPlay = game.StartTime < DateTime.Now.AddMinutes(5); 

            return bracketsGameModel;
        }

        private string GetColour(string playerColour)
        {
            if ((playerColour?.Length ?? 0) > 1)
            {
                return playerColour.Substring(0,1).ToUpper();
            }

            return string.Empty;
        }
    }
}
