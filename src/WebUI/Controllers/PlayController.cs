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
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.WebUI.Models;
using OverTheBoard.Data.Entities.Applications;

namespace OverTheBoard.WebUI.Controllers
{
    [Authorize]
    [Route("play")]
    public class PlayController : Controller
    {
        private readonly IHubContext<GameMessageHub> _gameMessageHubContext;
        private readonly IGameService _gameService;
        private readonly IUserService _userService;

        public PlayController(IHubContext<GameMessageHub> gameMessageHubContext, IGameService gameService,
            IUserService userService)
        {
            _gameMessageHubContext = gameMessageHubContext;
            _gameService = gameService;
            _userService = userService;
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
        public IActionResult Brackets()
        {
            return View();
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
            var game =  await _gameService.GetPlayersAsync(gameId);
            var currentPlayer = game.Players.FirstOrDefault(e => e.UserId.Equals(userId));
            var opponentPlayer = game.Players.FirstOrDefault(e => !e.UserId.Equals(userId));

            var opponentUser = await _userService.GetUserAsync(opponentPlayer.UserId);
            var currentUser = await _userService.GetUserAsync(userId);

            model.CurrentDisplayName = currentUser.DisplayName;
            model.OpponentDisplayName = opponentUser.DisplayName;

            model.CurrentColour = currentPlayer.Colour;
            model.OpponentColour = opponentPlayer.Colour;

            model.CurrentRating = currentUser.Rating;
            model.OpponentRating = opponentUser.Rating;

            return View(model);
        }
        
        
        [HttpGet("game/send/message")]
        public async Task<IActionResult> Game1()
        {
            await _gameMessageHubContext.Clients.All.SendAsync("Registered", $"Instance is {DateTime.Now.ToString()}");
            return Ok(true);
        }

        private string GetUserId()
        {
            return User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }

    
}
