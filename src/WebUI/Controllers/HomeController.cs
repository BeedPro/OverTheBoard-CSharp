using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OverTheBoard.Data.Entities;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.ObjectModel;
using OverTheBoard.WebUI.Models;

namespace OverTheBoard.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<OverTheBoardUser> _signInManager;
        private readonly IGameService _gameService;

        public HomeController(ILogger<HomeController> logger, SignInManager<OverTheBoardUser> signInManager, IGameService gameService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _gameService = gameService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //await _gameService.CreateGameAsync(Guid.NewGuid().ToString(), new List<UnrankedGameQueueItem>()
            //{
            //    new UnrankedGameQueueItem(){UserId = Guid.NewGuid().ToString(), ConnectionId = "ewtqwt1"},
            //    new UnrankedGameQueueItem(){UserId = Guid.NewGuid().ToString(), ConnectionId = "ewtqwt2"}
            //});

            // Checking if user is logged on and redirecting to Dashboard
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect("~/dashboard");
            }
            return View();
        }    

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
