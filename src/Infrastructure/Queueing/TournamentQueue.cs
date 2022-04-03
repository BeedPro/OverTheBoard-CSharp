using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Data.Repositories;
using OverTheBoard.Infrastructure.Extensions;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Queueing
{
    public class TournamentQueue : ITournamentQueue
    {
        private readonly IRepository<TournamentQueueEntity> _repository;

        public TournamentQueue(IRepository<TournamentQueueEntity> repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddQueueAsync(TournamentQueueItem queueItem)
        {
            _repository.Add(new TournamentQueueEntity() { UserId = queueItem.UserId.ToGuid(), Level = queueItem.Level, CreatedDate = DateTime.Now});
            var result =  _repository.Save();
            return await Task.FromResult(result);
        }

        public async Task<List<int>> GetAvailableLevels()
        {
            var results = _repository.Query().GroupBy(e => e.Level).Select(e => e.Key);
            return await results.ToListAsync();
        }

        public async Task<bool> HasRequiredPlayersInLevel(int playersPerGroup, int level)
        {
            var count = await _repository.Query().CountAsync(e=>e.Level == level);
            return count >= playersPerGroup;
        }

        public async Task<List<TournamentQueueItem>> GetGameQueueItems(int playersPerGroup, int level)
        {
            var results = _repository.Query()
                    .OrderByDescending(o=>o.TournamentQueueId)
                    .Where(e=>e.Level == level)
                    .Take(playersPerGroup)
                    .Select(s=> new TournamentQueueItem(){UserId =  s.UserId.ToString(), Level = s.Level, TournamentQueueId = s.TournamentQueueId});

            var items = await results.ToListAsync();

            if (items.Count() == playersPerGroup)
            {
                return items;
            }

            return new List<TournamentQueueItem>();
        }

        public async Task<bool> RemoveGameQueueItems(List<TournamentQueueItem> items)
        {
            foreach (var item in items)
            {
                var entity = _repository.Query().FirstOrDefault(e => e.TournamentQueueId == item.TournamentQueueId);
                if (entity != null)
                {
                    _repository.Remove(entity);
                }
            }
            var result = _repository.Save();
            return result;
        }
        
    }
}