using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Data.Repositories;
using OverTheBoard.Infrastructure.Extensions;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IRepository<TournamentEntity> _repositoryTournament;

        public TournamentService(IRepository<TournamentEntity> repositoryTournament)
        {
            _repositoryTournament = repositoryTournament;
        }

        public async Task<bool> CreateTournamentAsync(string tournamentIdentifier, List<TournamentQueueItem> players, DateTime startDate, DateTime endDate)
        {
            var entityTournament = new TournamentEntity()
            {
                TournamentIdentifier = tournamentIdentifier.ToGuid(),
                StartDate = startDate,
                EndDate = endDate,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            entityTournament.Players = new List<TournamentUserEntity>();
            foreach (var player in players)
            {
                entityTournament.Players.Add(new TournamentUserEntity()
                {
                    UserId = player.UserId.ToGuid(),
                });
            }

            _repositoryTournament.Context.Add(entityTournament);
            var status = _repositoryTournament.Save();
            return status;
        }

        public async Task<string> GetTournamentIdentifierByUserAsync(string userId)
        {
            var hasActiveTournament = await _repositoryTournament.Query()
                .Include(i => i.Players)
                .FirstOrDefaultAsync(e =>
                    e.Players.Any(f => f.UserId == userId.ToGuid()) && e.IsActive);

            return hasActiveTournament?.TournamentIdentifier.ToString();
        }

    }
}