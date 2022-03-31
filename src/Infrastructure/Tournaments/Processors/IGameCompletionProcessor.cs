using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverTheBoard.Infrastructure.Common;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Tournaments.Processors
{

    public interface IGameCompletionProcessor : ILocatorInterface<GameCompletionQueueItem>
    {
        Task<bool> ProcessAsync(GameCompletionQueueItem item);
    }
}
