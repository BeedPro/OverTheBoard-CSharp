using System.Collections.Generic;
using OverTheBoard.ObjectModel;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Queueing
{
    public interface IUnrankedGameQueue
    {
        List<GameQueueItem> GetQueueGame(UnrankedGameQueueItem queueItem);
    }
    
    public interface IRankedGameQueue
    {
        List<GameQueueItem> GetQueueGame(RankedGameQueueItem queueItem);
    }
}