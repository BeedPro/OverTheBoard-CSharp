using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Queueing
{
    public interface IGameService
    {
        Task<bool> CreateGameAsync(string identifier, List<UnrankedGameQueueItem> queueItems, DateTime startTime, int periodInMinutes);
        Task<ChessGame> GetPlayersAsync(string gameId);
        Task<bool> UpdateConnectionAsync(string userId, string gameId, string connectionId);
        Task<string> SaveGameMoveAsync(string userId, ChessMove move);
        Task<List<GameInfo>> GetGameByUserIdAsync(string userId);
        Task<List<ChessGame>> GetGamesInProgress();
        Task<bool> SaveGameOutcomeAsync(string gameId, GameStatus status);
    }
}
