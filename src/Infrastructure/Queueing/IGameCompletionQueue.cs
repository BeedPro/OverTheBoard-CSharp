using System.Collections.Generic;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Queueing
{
    public interface IGameCompletionQueue
    {
        Task<bool> AddQueueAsync(GameCompletionQueueItem queueItem);
        Task<bool> DeActivateAsync(GameCompletionQueueItem queueItem);
        Task<List<GameCompletionQueueItem>> GetQueuesAsync();
    }
}