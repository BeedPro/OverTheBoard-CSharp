using System.Collections.Generic;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Queueing
{
    public interface ITournamentQueue
    {
        Task<bool> AddQueueAsync(TournamentQueueItem queueItem);
        Task<List<int>> GetAvailableLevels();
        Task<bool> HasRequiredPlayersInLevel(int playersPerGroup, int level);
        Task<List<TournamentQueueItem>> GetGameQueueItems(int playersPerGroup, int level);
        Task<bool> RemoveGameQueueItems(List<TournamentQueueItem> items);
    }
}