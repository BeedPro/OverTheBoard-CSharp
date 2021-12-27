using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.ObjectModel;

namespace OverTheBoard.WebUI.SignalR
{
    public class GameMessageHub : Hub
    {
        private readonly IQueueSelector _queueSelector;

        public GameMessageHub(IQueueSelector queueSelector)
        {
            _queueSelector = queueSelector;
        }


        public async Task Initialise(InitialisationMessage initialisationMessage)
        {
            var userId = GetUserId();
            var players = _queueSelector.GetQueue(initialisationMessage.Type).GetQueueGame(userId, initialisationMessage);
            if (players != null)
            {
                foreach (var player in players)
                {
                    await Clients.Client(player.ConnectionId).SendAsync("Initialised", $"{player.Colour}");
                }

            }
        }

        
        public async Task Send(string user, string message)
        {
            await Clients.Others.SendAsync("Receive", user, message);
        }

        private string GetUserId()
        {
            return Context.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }

}
