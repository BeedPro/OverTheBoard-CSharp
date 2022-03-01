using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.ObjectModel;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Data.Repositories;

namespace OverTheBoard.WebUI.SignalR
{
    public class GameMessageHub : Hub
    {
        private readonly IGameService _gameService;
        public GameMessageHub(IGameService gameService)
        {
            _gameService = gameService;

        }


        public async Task Initialise(InitialisationMessage initialisationMessage)
        {
            var userId = GetUserId();
            await _gameService.UpdateConnectionAsync(userId, initialisationMessage.GameId, initialisationMessage.ConnectionId);
            var players = await _gameService.GetPlayersAsync(initialisationMessage.GameId);
            if (players != null)
            {
                var whiteTimer = Convert.ToInt32(players.Players.FirstOrDefault(e => e.Colour == "white").TimeRemaining.TotalSeconds) * 10;
                var blackTimer = Convert.ToInt32(players.Players.FirstOrDefault(e => e.Colour == "black").TimeRemaining.TotalSeconds) * 10;
                foreach (var player in players.Players)
                {
                    var chessMove = new ChessMove(){ Orientation = player.Colour};
                    if (player.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
                    {
                        chessMove.Fen = players.Fen;
                    }
                    chessMove.whiteRemaining = whiteTimer;
                    chessMove.blackRemaining = blackTimer;
                    await Clients.Client(player.ConnectionId).SendAsync("Initialised", chessMove);
                }
                
               
            }
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
