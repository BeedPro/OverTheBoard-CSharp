using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Data.Repositories;
using OverTheBoard.Infrastructure.Extensions;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Services
{
    public interface ITournamentService
    {
        Task<bool> CreateTournamentAsync(string tournamentIdentifier, List<TournamentQueueItem> players, DateTime startDate, DateTime endDate);
    }

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
    }
}