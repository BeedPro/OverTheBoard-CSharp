using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OverTheBoard.Data;
using OverTheBoard.Data.Entities;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using OverTheBoard.Data.Entities.Applications;
using OverTheBoard.Infrastructure.Common;

namespace OverTheBoard.WebUI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IFileUploader _fileUploader;
        private readonly UserManager<OverTheBoardUser> _userManager;
        private readonly SignInManager<OverTheBoardUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IGameService _gameService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IFileUploader fileUploader, 
            UserManager<OverTheBoardUser> userManager, 
            SignInManager<OverTheBoardUser> signInManager,
            IUserService userService,
            IGameService gameService,
            ILogger<DashboardController> logger
            )
        {
            _fileUploader = fileUploader;
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _gameService = gameService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Gets the users DisplayName and DisplayImage and returns the model to output it on the view
            var model = new DashboardViewModel();
            var user = await _userManager.GetUserAsync(User);
            model.DisplayName = user.DisplayName;
            model.DisplayImagePath = $"{user.DisplayImagePath}";
            model.Rating = user.Rating;

            var chartData = await _gameService.GetChartsDataAsync(user.Id);

            model.Charts = new ChessDataEngine(chartData).Build().ToString();
            return View(model);
        }
        

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            SettingsViewModel model = new SettingsViewModel();
            // Gets the users DisplayName and DisplayImage and returns the model to output it on the view
            var user = await _userManager.GetUserAsync(User);
            model.DisplayImagePath = user.DisplayImagePath;
            model.Email = user.Email;
            model.DisplayName = user.DisplayName;
            model.DisplayNameId = user.DisplayNameId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(SettingsViewModel model, IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);
            var filename = await _fileUploader.UploadImage(file, user.Id);
            //Checks if the display image pic has been uploaded to the site
            if (filename != null)
            {
                user.DisplayImagePath = $"Users\\{filename}";
                model.DisplayImagePath = $"Users\\{filename}";

            }

            else
            {
                model.DisplayImagePath = user.DisplayImagePath;
            }

            // TO-DO: Check if the username is in database
            //Checks and then changes the DisplayName
            if (model.DisplayName != null &&  !await UserClash(model.DisplayName, user.DisplayNameId))
            {
                user.DisplayName = model.DisplayName;
            }
            else
            {
                ModelState.AddModelError("DisplayName", "Username clashes with an existing user please enter another username" );
            }
            if (model.OldPassword != null && model.NewPassword != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    if (user != null)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return View(model);
                    }
                }
            }

            // Sets the Email an DisplayName of the model to be output in the SettingsView
            model.Email = user.Email;
            model.DisplayName = user.DisplayName;
            model.DisplayNameId = user.DisplayNameId;
            await _userManager.UpdateAsync(user);
            return View(model);
            
        }
        public async Task<IActionResult> GameHistory()
        {
            var model = new GameHistoryViewModel();
            var userId = GetUserId();
            var games = await _gameService.GetGameByUserIdAsync(userId);
            var gamesCompleted = games.Where(e => e.Status == GameStatus.Completed).ToList();
            model.gamesCompleted = gamesCompleted;
            return View(model);
        }
        public async Task<IActionResult> Leaderboard()
        {
            var model = new LeaderboardViewModel();
            
            model.YourDetails = await GetYourDetailsAsync();
            model.TopRanks = await GetTopRanksAsync();
            return View(model);
        }

        private async Task<List<GamePlayerStatsModel>> GetTopRanksAsync()
        {
            var statsModels = new List<GamePlayerStatsModel>();
            var users = await _userService.GetTopRatingUsersAsync(10);
            foreach (var user in users)
            {
                var stats = await _gameService.GetStatsByUserAsync(user.Id);
                statsModels.Add(new GamePlayerStatsModel() { DisplayName =user.DisplayName, Matches = stats.TotalGame, Win = stats.TotalWin, Lose = stats.TotalLose, Draw = stats.TotalDraw, Point = user.Rating });
            }

            return statsModels;
        }

        private async Task<List<KeyValuePair<string,string>>> GetYourDetailsAsync()
        {
            
            var user = await _userManager.GetUserAsync(User);
            var stats = await _gameService.GetStatsByUserAsync(user.Id);
            var keyValuePairs = new List<KeyValuePair<string, string>>();
            keyValuePairs.Add(new KeyValuePair<string, string>("Rating", user.Rating.ToString()));
            keyValuePairs.Add(new KeyValuePair<string, string>("Rank", user.Rank.ToString()));
            keyValuePairs.Add(new KeyValuePair<string, string>("Total Games", stats.TotalGame.ToString()));
            keyValuePairs.Add(new KeyValuePair<string, string>("Total Win", stats.TotalWin.ToString()));
            keyValuePairs.Add(new KeyValuePair<string, string>("Total Lose", stats.TotalLose.ToString()));
            keyValuePairs.Add(new KeyValuePair<string, string>("Total Draw", stats.TotalDraw.ToString()));
            return keyValuePairs;
        }

        public IActionResult Analysis()
        {
            return View();
        }


        private async Task<bool> UserClash(string displayName, string displayNameId)
        {
            var user = await _userService.GetUserDisplayNameAndyNameIdAsync(displayName, displayNameId);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        private string GetUserId()
        {
            return User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
