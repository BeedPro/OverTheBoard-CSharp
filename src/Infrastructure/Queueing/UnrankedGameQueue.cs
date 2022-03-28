using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Queueing
{
    //TODO: Test this new queue system and make method async or revamp it.
    public class UnrankedGameQueue : IUnrankedGameQueue
    {
        public string Name => GameType.Unranked.ToString();

        public Queue<UnrankedGameQueueItem> Queue = new();
        //
        //TODO: This is the new queue system that has been removed
        //public List<UnrankedGameQueueItem> UserQueue = new List<UnrankedGameQueueItem>();
        //public List<UnrankedGameQueueItem> GetQueueGame(UnrankedGameQueueItem queueItem)
        //{
        //    var foundMatch = false;
        //    if(UserQueue.Any())
        //    {
        //        var nearestUpperVal = Math.Ceiling(queueItem.Rating / 50.0) * 50.0;
        //        var upperBound = Convert.ToInt32(nearestUpperVal + 50.0);
        //        var lowerBound = Convert.ToInt32(nearestUpperVal - 50.0);
        //        while (!foundMatch)
        //        {
        //            upperBound += 50;
        //            lowerBound -= 50;
        //            foreach (var itemQueue in UserQueue)
        //            {
        //                var inRange = itemQueue.Rating <= upperBound && itemQueue.Rating >= lowerBound;
        //                if (inRange)
        //                {
        //                    foundMatch = true;
        //                    var itemIndex = UserQueue.IndexOf(itemQueue);
        //                    var item = UserQueue[itemIndex];
        //                    UserQueue.RemoveAt(itemIndex);
        //                    if (!item.UserId.Equals(queueItem.UserId, StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        return new List<UnrankedGameQueueItem>()
        //                        {
        //                            item,
        //                            queueItem
        //                        };
        //                    }
        //                }
        //            }

        //        }
        //    }

        //    UserQueue.Add(queueItem);
        //    return null;
        //}

        //TODO: Implement new Queueing Logic
        public List<UnrankedGameQueueItem> GetQueueGame(UnrankedGameQueueItem queueItem)
        {
            if (Queue.Any())
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