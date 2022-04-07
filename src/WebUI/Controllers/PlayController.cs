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

        public PlayController(IGameService gameService, ITournamentService tournamentService,
            IUserService userService, IEloService eloService)
        {
            _gameService = gameService;
            _tournamentService = tournamentService;
            _userService = userService;
            _eloService = eloService;
        }
        public async Task<IActionResult> Index()
        {
            var model = new PlayViewModel();
            var userId = GetUserId();
            var games = await _gameService.GetGameByUserIdAsync(userId);
            var gamesInProgress = games.Where(e => e.Status == GameStatus.InProgress).ToList();
            model.gameInProgress = gamesInProgress;
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

            BracketsViewModel model = PopulateTournament(userId, tournament, games);
            return View(model);
        }

        private BracketsViewModel PopulateTournament(string userId, Tournament tournament, List<ChessGame> games)
        {

            return new BracketsViewModel();
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
