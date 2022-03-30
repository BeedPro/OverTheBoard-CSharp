using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Data.Repositories;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Queueing
{
    public class GameCompletionQueue : IGameCompletionQueue
    {
        private readonly IRepository<GameCompletionQueueEntity> _repositoryCompletionQueue;

        public GameCompletionQueue(IRepository<GameCompletionQueueEntity> repositoryCompletionQueue)
        {
            _repositoryCompletionQueue = repositoryCompletionQueue;
        }

        public async Task<bool> AddQueueAsync(GameCompletionQueueItem queueItem)
        {
            _repositoryCompletionQueue.Add(new GameCompletionQueueEntity()
            {
                GameId = queueItem.GameId,
                IsActive = true,
                CreatedDate = DateTime.Now
            });

            _repositoryCompletionQueue.Save();

            return true;
        }

        public async Task<bool> DeActivateAsync(GameCompletionQueueItem queueItem)
        {
            var entity = await _repositoryCompletionQueue.Query()
                .FirstOrDefaultAsync(e => e.GameId == queueItem.GameId);
            if (entity != null)
            {
                entity.IsActive = false;
                _repositoryCompletionQueue.Save();
                return true;
            }

            return false;
        }

        public async Task<List<GameCompletionQueueItem>> GetQueuesAsync()
        {
            var entities = await _repositoryCompletionQueue.Query().Where(e => e.IsActive == true).ToListAsync();
            return entities.Select(e => new GameCompletionQueueItem
            {
                GameId = e.GameId
            }).ToList();
        }
    }
}