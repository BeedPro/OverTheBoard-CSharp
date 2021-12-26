using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverTheBoard.WebUI.SignalR
{
    public class GameMessageHub : Hub
    {

        public async Task Register(string instanceId)
        {
            await Clients.Caller.SendAsync("Registered", $"Instance is {instanceId}");
        }

        public async Task Send(string user, string message)
        {
            await Clients.Others.SendAsync("Receive", user, message);
        }
    }
}
