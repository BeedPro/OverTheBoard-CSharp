using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OverTheBoard.Core.Security.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using OverTheBoard.ObjectModel;
using OverTheBoard.WebUI.SignalR;

namespace OverTheBoard.WebUI.Controllers
{
    [Authorize]
    [Route("play")]
    public class PlayController : Controller
    {
        private readonly IHubContext<GameMessageHub> _gameMessageHubContext;

        public PlayController(IHubContext<GameMessageHub> gameMessageHubContext)
        {
            _gameMessageHubContext = gameMessageHubContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("unranked")]
        public IActionResult Unranked()
        {
            return View();
        }

        [HttpGet("brackets")]
        public IActionResult Brackets()
        {
            return View();
        }

        [HttpGet("game/start-unranked")]
        public IActionResult StartUnranked(string sessionId)
        {
            return RedirectToAction("Game", new { sessionId = Guid.NewGuid().ToString(), type = GameType.Unranked.ToString().ToLower()});
        }

        [HttpGet("game/{sessionId}")]
        public IActionResult Game(string sessionId)
        {
            return View();
        }
        
        
        [HttpGet("game/send/message")]
        public async Task<IActionResult> Game1()
        {
            await _gameMessageHubContext.Clients.All.SendAsync("Registered", $"Instance is {DateTime.Now.ToString()}");
            return Ok(true);
        }
    }

    
}
