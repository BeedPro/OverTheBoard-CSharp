using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverTheBoard.Infrastructure.Common;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.Infrastructure.Tournaments.Processors;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Tournaments
{
    public class GameCompletionQueueReceiver : IGameBackgroundService
    {
        private readonly IGameCompletionQueue _completionQueue;
        private readonly ILocatorService<IGameCompletionProcessor, GameCompletionQueueItem> _processor;

        public GameCompletionQueueReceiver(IGameCompletionQueue completionQueue, ILocatorService<IGameCompletionProcessor, GameCompletionQueueItem> processor)
        {
            _completionQueue = completionQueue;
            _processor = processor;
        }

        public async Task<bool> ProcessAsync()
        {
            var queueItem = await _completionQueue.GetQueuesAsync();

            foreach (var item in queueItem)
            {
                var status = await _processor.Get(item).ProcessAsync(item);
                await _completionQueue.RemoveQueueAsync(item);
            }

            return true;
        }
    }
    
}
