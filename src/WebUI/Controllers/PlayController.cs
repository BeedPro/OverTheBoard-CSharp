using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OverTheBoard.ObjectModel;
using OverTheBoard.WebUI.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data;
using OverTheBoard.Data.Entities;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.WebUI.Models;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.WebUI.ModelPopulators;

namespace OverTheBoard.WebUI.Controllers
{
    [Authorize]
    [Route("play")]
    public class PlayController : Controller
    {
        private readonly IGameService _gameService;
        private readonly ITournamentService _tournamentService;
        private readonly IUserService _userService;
        private readonly IEloService _eloService;
        private readonly IBracketsViewModelPopulator _bracketsViewModelPopulator;

        public PlayController(IGameService gameService, ITournamentService tournamentService,
            IUserService userService, IEloService eloService, IBracketsViewModelPopulator bracketsViewModelPopulator)
        {
            _gameService = gameService;
            _tournamentService = tournamentService;
            _userService = userService;
            _eloService = eloService;
            _bracketsViewModelPopulator = bracketsViewModelPopulator;
        }
        public async Task<IActionResult> Index()
        {
            var model = new PlayViewModel();
            var userId = GetUserId();
            var games = await _gameService.GetGameByUserIdAsync(userId, GameStatus.InProgress);
            model.gameInProgress = games;
            return View(model);
        }

        [HttpGet("unranked")]
        public IActionResult Unranked()
        {
            return View();
        }

        [HttpGet("brackets")]
        public async Task<IActionResult> Brackets()
        {
            var userId = GetUserId();
            var activeTournamentIdentifier = await _tournamentService.GetTournamentIdentifierByUserAsync(userId);
            var tournament = await _tournamentService.GetTournamentAsync(activeTournamentIdentifier);
            var games = await _gameService.GetMatchesByTournamentAsync(activeTournamentIdentifier);

            Dictionary<string, OverTheBoardUser> users = new Dictionary<string, OverTheBoardUser>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var player in tournament.Players)
            {
                var user = await _userService.GetUserAsync(player.UserId);
                users.Add(player.UserId, user);
            }

            BracketsViewModel model = _bracketsViewModelPopulator.Populate(userId, tournament, games, users);
            return View(model);
        }

        
        [HttpGet("game/start-unranked")]
        public IActionResult StartUnranked(string sessionId)
        {
            return RedirectToAction("Game", new { sessionId = Guid.NewGuid().ToString(), type = GameType.Unranked.ToString().ToLower()});
        }

        //TODO Rename sessionId to gameId
        [HttpGet("game/{gameId}")]
        public async Task<IActionResult> Game(string gameId)
        {
            
            var model = new GameViewModel();
            var userId = GetUserId();
            var game =  await _gameService.GetChessGameWithPlayersAsync(gameId);
            var currentPlayer = game.Players.FirstOrDefault(e => e.UserId.Equals(userId));
            if (currentPlayer == null)
            {
                throw new Exception("User does not exists");
            }
            var opponentPlayer = game.Players.FirstOrDefault(e => !e.UserId.Equals(userId));

            if (opponentPlayer != null && currentPlayer != null)
            {
                var opponentUser = await _userService.GetUserAsync(opponentPlayer.UserId);
                var currentUser = await _userService.GetUserAsync(userId);

                model.CurrentDisplayName = currentUser.DisplayName;
                model.OpponentDisplayName = opponentUser.DisplayName;

                model.CurrentColour = currentPlayer.Colour;
                model.OpponentColour = opponentPlayer.Colour;

                model.CurrentRating = currentUser.Rating;
                model.OpponentRating = opponentUser.Rating;

                var currentEloOutcomes = await _eloService.CalculateEloRatingChangeAsync(currentUser.Rating, opponentUser.Rating);

                foreach (var outcome in currentEloOutcomes)
                {
                    switch (outcome.Type)
                    {
                        case EloOutcomesType.Win:
                            model.CurrentEloChangeOnWin = outcome.Value;
                            break;
                        case EloOutcomesType.Lose:
                            model.CurrentEloChangeOnLose = outcome.Value;
                            break;
                        case EloOutcomesType.Draw:
                            model.CurrentEloChangeOnDraw = outcome.Value;
                            break;
                    }
                }
            }

            return View(model);
        }
        
        
        private string GetUserId()
        {
            return User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }

    
}
