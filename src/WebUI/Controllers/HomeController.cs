using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OverTheBoard.Core.Security.Data;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.WebUI.Models;

namespace OverTheBoard.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileUploader _fileUploader;
        private readonly UserManager<OverTheBoardUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IFileUploader fileUploader, UserManager<OverTheBoardUser> userManager)
        {
            _logger = logger;
            _fileUploader = fileUploader;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Temp()
        {
            return View();
        }


        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Settings(SettingsViewModel model, IFormFile file)
        {
            var filename = await _fileUploader.UploadImage(file);
            var user = await _userManager.GetUserAsync(User);
            user.DisplayImagePath = filename;
            await _userManager.UpdateAsync(user);
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
