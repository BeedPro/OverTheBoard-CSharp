using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.ObjectModel;
using OverTheBoard.Infrastructure.Services;

namespace OverTheBoard.WebUI.SignalR
{
    public class QueueHub : Hub
    {
        private readonly IUnrankedGameQueue _gameQueue;
        private readonly IGameService _gameService;

        public QueueHub(IUnrankedGameQueue gameQueue, IGameService gameService)
        {
            _gameQueue = gameQueue;
            _gameService = gameService;
        }
        public async Task Queue(string connectionId)
        {
            var queueItems = _gameQueue.GetQueueGame(
                new UnrankedGameQueueItem()
                {
                    UserId = GetUserId(), 
                    ConnectionId = connectionId
                });

            if (queueItems?.Count == 2)
            {
                var gameId = Guid.NewGuid().ToString();
                await _gameService.CreateGameAsync(gameId, queueItems, DateTime.Now, 1);

                foreach (UnrankedGameQueueItem item in queueItems)
                {
                    await Clients.Client(item.ConnectionId).SendAsync("Play", gameId);
                }
                
            }
        }

        private string GetUserId()
        {
            return Context.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
