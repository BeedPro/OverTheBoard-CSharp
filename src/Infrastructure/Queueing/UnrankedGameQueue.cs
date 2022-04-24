using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OverTheBoard.Data.Entities;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.ObjectModel;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Queueing
{
    //TODO: Test this new queue system and make method async or revamp it.
    public class UnrankedGameQueue : IUnrankedGameQueue
    {
        public string Name => GameType.Unranked.ToString();

        public ConcurrentDictionary<string, UnrankedGameQueueItem> Queue = new ConcurrentDictionary<string, UnrankedGameQueueItem>();

        public List<GameQueueItem> GetQueueGame(UnrankedGameQueueItem queueItem)
        {
            if (Queue.Any())
            {
                var item = Queue.FirstOrDefault().Value;
                Queue.TryRemove(item.ConnectionId, out item);

                if (!item.UserId.Equals(queueItem.UserId, StringComparison.OrdinalIgnoreCase))
                {
                    return new List<GameQueueItem>()
                    {
                        item,
                        queueItem
                    };
                }
            }

            Queue.TryAdd(queueItem.ConnectionId, queueItem);
            return new List<GameQueueItem>();
        }

        public bool RemoveQueueGame(string connectionId)
        {
            var item = Queue.FirstOrDefault(e=>e.Key == connectionId).Value;
            if (item != null)
            {
                return Queue.TryRemove(item.ConnectionId, out item);
            }

            return false;
        }
    }
}