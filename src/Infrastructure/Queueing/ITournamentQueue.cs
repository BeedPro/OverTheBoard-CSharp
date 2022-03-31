using System.Collections.Generic;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Queueing
{
    public interface ITournamentQueue
    {
        Task<bool> AddQueueAsync(TournamentQueueItem queueItem);
        List<GameQueueItem> GetQueueGame(TournamentQueueItem queueItem);
    }
}