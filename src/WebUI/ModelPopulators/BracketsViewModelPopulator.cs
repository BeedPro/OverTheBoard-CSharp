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

            return bracketsViewModel;
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
