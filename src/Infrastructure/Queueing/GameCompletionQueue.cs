using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Data.Repositories;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.Infrastructure.Queueing
{
    public class GameCompletionQueue : IGameCompletionQueue
    {
        private readonly IRepository<GameCompletionQueueEntity> _repositoryCompletionQueue;
        private readonly ILogger<GameCompletionQueue> _logger;

        public GameCompletionQueue(IRepository<GameCompletionQueueEntity> repositoryCompletionQueue, ILogger<GameCompletionQueue> logger)
        {
            _repositoryCompletionQueue = repositoryCompletionQueue;
            _logger = logger;
        }

        public async Task<bool> AddQueueAsync(GameCompletionQueueItem queueItem)
        {
            _repositoryCompletionQueue.Add(new GameCompletionQueueEntity()
            {
                GameId = queueItem.Identifier,
                IsActive = true,
                Level = queueItem.Level,
                CreatedDate = DateTime.Now
            });
            _logger.LogInformation("Saving Game outcome started for {Identifier}", queueItem.Identifier);

            _repositoryCompletionQueue.Save();

            return true;
        }

        public async Task<bool> RemoveQueueAsync(GameCompletionQueueItem queueItem)
        {
            var entity = await _repositoryCompletionQueue.Query()
                .FirstOrDefaultAsync(e => e.GameId == queueItem.Identifier);
            if (entity != null)
            {
                _repositoryCompletionQueue.Remove(entity);
                _repositoryCompletionQueue.Save();
                return true;
            }

            return false;
        }

        public async Task<List<GameCompletionQueueItem>> GetQueuesAsync()
        {
            var entities = await _repositoryCompletionQueue.Query().Where(e => e.IsActive).ToListAsync();
            return entities.Select(e => new GameCompletionQueueItem
            {
                Identifier = e.GameId,
                Level = e.Level
            }).ToList();
        }
    }
}