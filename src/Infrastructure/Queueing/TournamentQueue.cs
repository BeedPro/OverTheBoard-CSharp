using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            _repository.Add(new TournamentQueueEntity() { Identifier = queueItem.Identifier, UserId = queueItem.UserId.ToGuid(), Level = queueItem.Level });
            _repository.Save();

            return await Task.FromResult(true);
        }

        public List<GameQueueItem> GetQueueGame(TournamentQueueItem queueItem)
        {
            var item = _repository.Query().FirstOrDefault(e => e.Level == queueItem.Level);
            if (item != null)
            {
                if (!item.UserId.ToString().Equals(queueItem.UserId, StringComparison.OrdinalIgnoreCase))
                {
                    _repository.Remove(item);
                    _repository.Save();
                    return new List<GameQueueItem>()
                    {
                        new TournamentQueueItem(){Identifier = item.Identifier, UserId = item.UserId.ToString(), Level = item.Level},
                        queueItem
                    };
                }
            }
            _repository.Add(new TournamentQueueEntity(){Identifier = queueItem.Identifier, UserId = queueItem.UserId.ToGuid(), Level = queueItem.Level});
            _repository.Save();
            return new List<GameQueueItem>();
        }
    }
}