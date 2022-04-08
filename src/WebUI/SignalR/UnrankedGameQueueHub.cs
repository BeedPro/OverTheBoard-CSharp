using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using OverTheBoard.Data.Entities;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.ObjectModel;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.ObjectModel.Options;
using OverTheBoard.ObjectModel.Queues;

namespace OverTheBoard.WebUI.SignalR
{
    public class UnrankedGameQueueHub : Hub
    {
        private readonly IUnrankedGameQueue _gameQueue;
        private readonly IGameService _gameService;
        private readonly IUserService _userService;
        private readonly GameSettingOptions _options;
        
        public UnrankedGameQueueHub(IUnrankedGameQueue gameQueue, IGameService gameService, IUserService userService, IOptions<GameSettingOptions> options)
        {
            _gameQueue = gameQueue;
            _gameService = gameService;    
            _userService = userService;
            _options = options.Value;
        }
        
        public async Task Queue(string connectionId)
        {
            var queueItems = _gameQueue.GetQueueGame(
                new UnrankedGameQueueItem()
                {
                    UserId = GetUserId(), 
                    ConnectionId = connectionId,
                    Rating = await GetUserRating()
                });

            if (queueItems?.Count == 2)
            {
                var gameId = Guid.NewGuid().ToString();
                SetColour(ref queueItems);
                await _gameService.CreateGameAsync(gameId, queueItems, DateTime.Now, _options.UnrankTimeDuration, GameType.Unranked, 0,0,string.Empty);

                foreach (UnrankedGameQueueItem item in queueItems)
                {
                    await Clients.Client(item.ConnectionId).SendAsync("Play", gameId);
                }
                
            }
        }

        List<string> colours = new List<string> { "white", "black" };
        private void SetColour(ref List<GameQueueItem> queueItems)
        {
            Random rand = new Random();
            int index = rand.Next(colours.Count);
            foreach (var item in queueItems)
            {
                item.Colour = colours[index];
                index = (index + 1) % 2;
            }
           
        }

        private string GetUserId()
        {
            return Context.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        private async Task<int> GetUserRating()
        {
            var userId = GetUserId();
            var user = await _userService.GetUserAsync(userId);
            return user.Rating;
        }
    }
}
