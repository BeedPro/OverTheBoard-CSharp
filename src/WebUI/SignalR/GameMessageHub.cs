using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Data.Repositories;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.Infrastructure.Services;

namespace OverTheBoard.WebUI.SignalR
{
    public class GameMessageHub : Hub
    {
        private readonly IGameService _gameService;
        private readonly IUserService _userService;
        private readonly IUnrankedInitialiserQueue _initialiserQueue;

        public GameMessageHub(IGameService gameService, IUserService userService, IUnrankedInitialiserQueue initialiserQueue)
        {
            _gameService = gameService;
            _userService = userService;
            _initialiserQueue = initialiserQueue;
        }
        
        public async Task Initialise(InitialisationMessage initialisationMessage)
        {
            var userId = GetUserId();
            await _gameService.UpdateConnectionAsync(userId, initialisationMessage.GameId, initialisationMessage.ConnectionId);
            var chessGame = await _gameService.GetChessGameWithPlayersAsync(initialisationMessage.GameId);

            if (chessGame.StartTime > DateTime.Now)
            {
                _initialiserQueue.AddQueue(new UnrankedInitialiserQueueItem()
                {
                    GameId = initialisationMessage.GameId,
                    StartDate = chessGame.StartTime
                });
                return;
            }

            var chessMoves = await _gameService.InitialiseChessGameAsync(initialisationMessage.GameId, userId);
            foreach (var move in chessMoves)
            {
                await Clients.Client(move.Key).SendAsync("Initialised", move.Value);
            }
           
        }

        
        public async Task SendGameStatus(GameOverStatus gameOverStatus)
        {
            var gameId = gameOverStatus.GameId;
            var game = await _gameService.GetChessGameWithPlayersAsync(gameOverStatus.GameId);
            await _gameService.SaveGameOutcomeAsync(gameId, gameOverStatus.WhiteOutcome, gameOverStatus.BlackOutcome);
            var whitePlayer = await _userService.GetUserAsync(game.Players.FirstOrDefault(e => e.Colour == "white")?.UserId);
            var blackPlayer = await _userService.GetUserAsync(game.Players.FirstOrDefault(e => e.Colour == "black")?.UserId);
            var gameRatings = new GamePlayerRatings()
            {
                WhitePlayerRating = whitePlayer.Rating,
                BlackPlayerRating = blackPlayer.Rating
            };
            await Clients.Client(gameOverStatus.ConnectionId).SendAsync("ReceiveRatings", gameRatings);
            
        }
        
        public async Task Send(ChessMove move)
        {
            var userId = GetUserId();
            var clientId = await _gameService.SaveGameMoveAsync(userId, move);
            await Clients.Client(clientId).SendAsync("Receive", move);
        }

       

        private string GetUserId()
        {
            return Context.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }

    

}
