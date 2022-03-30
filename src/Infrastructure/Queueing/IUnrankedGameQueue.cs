using System.Collections.Generic;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Queueing
{
    public interface IUnrankedGameQueue
    {
        List<GameQueueItem> GetQueueGame(GameQueueItem queueItem);
    }
}