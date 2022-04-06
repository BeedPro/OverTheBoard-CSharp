using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Queueing
{
    public interface IUnrankedInitialiserQueue
    {
        bool AddQueue(UnrankedInitialiserQueueItem queueItem);
        UnrankedInitialiserQueueItem GetNextItem();
    }

    public class UnrankedInitialiserQueueItem
    {
        public string GameId { get; set; }
        public DateTime StartDate { get; set; }
        //public Func<IHubCallerClients, ChessGame, string, Task> ToProcess { get; set; }
    }


    public class UnrankedInitialiserQueue : IUnrankedInitialiserQueue
    {
        ConcurrentDictionary<string, UnrankedInitialiserQueueItem> queue = new ConcurrentDictionary<string, UnrankedInitialiserQueueItem>();

        public bool AddQueue(UnrankedInitialiserQueueItem queueItem)
        {
            if (queue.ContainsKey(queueItem.GameId))
            {
                queue[queueItem.GameId] = queueItem;
            }
            else
            {
                queue.TryAdd(queueItem.GameId, new UnrankedInitialiserQueueItem());
            }

            return true;
        }

        public UnrankedInitialiserQueueItem GetNextItem()
        {
            var retValue = queue.FirstOrDefault(e => e.Value.StartDate <= DateTime.Now);
            if (!string.IsNullOrEmpty(retValue.Key))
            {
                queue.TryRemove(retValue);
                return retValue.Value;
            }

            return null;
        }
    }
}