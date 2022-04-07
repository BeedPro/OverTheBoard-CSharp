using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Data.Repositories;
using OverTheBoard.Infrastructure.Extensions;
using OverTheBoard.ObjectModel;
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

            entityTournament.Players = new List<TournamentPlayerEntity>();
            foreach (var player in players)
            {
                entityTournament.Players.Add(new TournamentPlayerEntity()
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
                .OrderByDescending(e=>e.TournamentId)
                .FirstOrDefaultAsync(e =>
                    e.Players.Any(f => f.UserId == userId.ToGuid()) && e.IsActive);

            return hasActiveTournament?.TournamentIdentifier.ToString();
        }

        public async Task<Tournament> GetTournamentAsync(string tournamentIdentifier)
        {
            var tournamentEntity = await _repositoryTournament.Query()
                .Include(i => i.Players)
                .OrderByDescending(e => e.TournamentId)
                .FirstOrDefaultAsync(e => e.TournamentIdentifier == tournamentIdentifier.ToGuid());

            return GetTournament(tournamentEntity);
        }

        private Tournament GetTournament(TournamentEntity entity)
        {
            return new Tournament()
            {
                TournamentId = entity.TournamentId,
                Identifier = entity.TournamentIdentifier.ToString(),
                IsActive = entity.IsActive,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                CreatedDate = entity.CreatedDate,
                Players = entity.Players.Select(p => new TournamentPlayer()
                {
                    TournamentPlayerId = p.TournamentPlayerId,
                    TournamentId = p.TournamentId,
                    UserId = p.UserId.ToString(),
                }).ToList()
            };
        }
    }
}