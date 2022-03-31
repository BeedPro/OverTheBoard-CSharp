using System;
using System.Threading.Tasks;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Tournaments.Processors
{
    public class TournamentGameCompletionProcessor : IGameCompletionProcessor
    {
        public bool CanSelect(GameCompletionQueueItem parameter, bool isDefault)
        {
            return isDefault;
        }

        public Task<bool> ProcessAsync(GameCompletionQueueItem item)
        {
            throw new NotImplementedException();
        }
    }
}