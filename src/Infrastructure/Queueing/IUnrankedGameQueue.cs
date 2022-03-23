using System.Collections.Generic;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.Infrastructure.Queueing
{
    public interface IUnrankedGameQueue
    {
        List<UnrankedGameQueueItem> GetQueueGame(UnrankedGameQueueItem queueItem);
    }
}