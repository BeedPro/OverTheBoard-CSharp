using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Queueing
{
    public interface IGameService
    {
        Task<bool> CreateGameAsync(string gameId, List<UnrankedGameQueueItem> queueItems);
        Task<ChessGame> GetPlayersAsync(string gameId);
        Task<bool> UpdateConnectionAsync(string userId, string gameId, string connectionId);
        Task<string> MoveAsync(string userId, ChessMove move);
    }

    public class GameService : IGameService
    {
        List<ChessGame> _chessGames = new List<ChessGame>();
        public async Task<bool> CreateGameAsync(string gameId, List<UnrankedGameQueueItem> queueItems)
        {
            ChessGame game = new ChessGame();
            game.GameId = gameId;

            var colour = "white";
            foreach (var item in queueItems)
            {
                var player = new GamePlayer()
                {
                    UserId = item.UserId,
                    Colour = colour,
                    ConnectionId = item.ConnectionId
                };

                colour = "black";
                game.Players.Add(player);
            }
            _chessGames.Add(game);
            return true;
        }
        

        public async Task<ChessGame> GetPlayersAsync(string gameId)
        {
            var game = _chessGames.FirstOrDefault(e => e.GameId.Equals(gameId, StringComparison.OrdinalIgnoreCase));
            return game;
        }

        public async Task<bool> UpdateConnectionAsync(string userId, string gameId, string connectionId)
        {
            var player = _chessGames.FirstOrDefault(e => e.GameId.Equals(gameId, StringComparison.OrdinalIgnoreCase))?.Players.FirstOrDefault(u=>u.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));
            if (player != null)
            {
                player.ConnectionId = connectionId;
            }
            
            return true;
        }

        public async Task<string> MoveAsync(string userId, ChessMove move)
        {
            var game = _chessGames.FirstOrDefault(e => e.GameId.Equals(move.GameId, StringComparison.OrdinalIgnoreCase));
            var player = game?.Players.FirstOrDefault(u => !u.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));
            game.Fen = move.Fen;
            return player.ConnectionId;
        }
    }
}
