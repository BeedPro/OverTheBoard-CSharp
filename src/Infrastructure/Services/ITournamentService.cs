using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Services
{
    public interface ITournamentService
    {
        Task<bool> CreateTournamentAsync(string tournamentIdentifier, List<TournamentQueueItem> players, DateTime startDate, DateTime endDate);
        Task<string> GetTournamentIdentifierByUserAsync(string userId);
        Task<Tournament> GetTournamentAsync(string tournamentIdentifier);

    }
}