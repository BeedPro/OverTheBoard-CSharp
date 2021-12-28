using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.ObjectModel;

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
                foreach (var player in players.Players)
                {
                    var chessMove = new ChessMove(){ Colour = player.Colour};
                    if (player.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
                    {
                        chessMove.Fen = players.Fen;
                    }

                    await Clients.Client(player.ConnectionId).SendAsync("Initialised", chessMove);
                }

            }
        }

        
        public async Task Send(ChessMove move)
        {
            var userId = GetUserId();
            var clientId = await _gameService.MoveAsync(userId, move);
            await Clients.Client(clientId).SendAsync("Receive", move);
        }

        private string GetUserId()
        {
            return Context.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }

    
}
