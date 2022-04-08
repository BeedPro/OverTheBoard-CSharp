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

        public async Task<bool> ProcessAsync(GameCompletionQueueItem item)
        {

            return true;
        }
    }
}