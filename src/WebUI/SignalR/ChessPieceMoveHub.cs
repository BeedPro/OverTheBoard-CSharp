using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverTheBoard.WebUI.SignalR
{
    public class ChessPieceMoveHub : Hub
    {
        public async Task Send(string user, string message)
        {
            await Clients.Others.SendAsync("Receive", user, message);
        }
    }
}
