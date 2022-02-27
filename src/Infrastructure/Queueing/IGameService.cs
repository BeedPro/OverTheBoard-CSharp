using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Queueing
{
    public interface IGameService
    {
        Task<bool> CreateGameAsync(string identifier, List<UnrankedGameQueueItem> queueItems);
        Task<ChessGame> GetPlayersAsync(string gameId);
        Task<bool> UpdateConnectionAsync(string userId, string gameId, string connectionId);
        Task<string> SaveGameMoveAsync(string userId, ChessMove move);
    }
}
