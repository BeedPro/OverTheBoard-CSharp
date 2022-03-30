using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Data.Repositories;
using OverTheBoard.Infrastructure.Extensions;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Queueing
{
    public class RankedGameQueue : IRankedGameQueue
    {
        private readonly IRepository<RankedGameQueueEntity> _repository;
        //private ConcurrentDictionary<Guid, RankedGameQueueItem> _queue = new ConcurrentDictionary<Guid, RankedGameQueueItem>();

        public RankedGameQueue(IRepository<RankedGameQueueEntity> repository)
        {
            _repository = repository;
        }

        public List<GameQueueItem> GetQueueGame(RankedGameQueueItem queueItem)
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
                        new RankedGameQueueItem(){Identifier = item.Identifier, UserId = item.UserId.ToString(), Level = item.Level},
                        queueItem
                    };
                }
            }
            _repository.Add(new RankedGameQueueEntity(){Identifier = queueItem.Identifier, UserId = queueItem.UserId.ToGuid(), Level = queueItem.Level});
            _repository.Save();
            return new List<GameQueueItem>();
        }
    }
}