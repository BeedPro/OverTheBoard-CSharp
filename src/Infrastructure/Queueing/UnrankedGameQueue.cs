using System;
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

        public Queue<UnrankedGameQueueItem> Queue = new Queue<UnrankedGameQueueItem>();

        public List<GameQueueItem> GetQueueGame(UnrankedGameQueueItem queueItem)
        {
            if (Queue.Any())
            {
                var item = Queue.Dequeue();
                if (!item.UserId.Equals(queueItem.UserId, StringComparison.OrdinalIgnoreCase))
                {
                    return new List<GameQueueItem>()
                    {
                        item,
                        queueItem
                    };
                }
            }
            Queue.Enqueue(queueItem);
            return new List<GameQueueItem>();
        }
    }
}