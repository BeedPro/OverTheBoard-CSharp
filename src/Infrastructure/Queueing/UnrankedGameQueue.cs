using System;
using System.Collections.Generic;
using System.Linq;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Queueing
{
    public class UnrankedGameQueue : IUnrankedGameQueue
    {
        public string Name => GameType.Unranked.ToString();

        public Queue<UnrankedGameQueueItem> Queue = new Queue<UnrankedGameQueueItem>();

        public List<UnrankedGameQueueItem> GetQueueGame(UnrankedGameQueueItem queueItem)
        {
            if(Queue.Any())
            {
                var item = Queue.Dequeue();
                if (!item.UserId.Equals(queueItem.UserId, StringComparison.OrdinalIgnoreCase))
                {
                    return new List<UnrankedGameQueueItem>()
                    {
                        item,
                        queueItem
                    };
                }
            }

            Queue.Enqueue(queueItem);
            return null;
        }
    }
}