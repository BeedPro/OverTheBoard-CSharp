using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OverTheBoard.Data.Entities;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.ObjectModel;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Services
{
    public interface IGameService
    {
        Task<bool> CreateGameAsync(string identifier, List<GameQueueItem> queueItems, DateTime startTime, int periodInMinutes, GameType type, string tournamentIdentifier);
        Task<ChessGame> GetPlayersAsync(string gameId);
        Task<bool> UpdateConnectionAsync(string userId, string gameId, string connectionId);
        Task<string> SaveGameMoveAsync(string userId, ChessMove move);
        Task<List<GameInfo>> GetGameByUserIdAsync(string userId);
        Task<List<ChessGame>> GetGamesInProgress();
        Task<bool> SaveGameOutcomeAsync(string gameId, EloOutcomesType whitePlayerOutcome,
            EloOutcomesType blackPlayerOutcome);
        Task<List<ChessGame>> GetMatchesByTournamentAsync(string playerUserId, string tournamentIdentifier);
        string GetTournamentIdByUserAsync(string userId);
    }
}
