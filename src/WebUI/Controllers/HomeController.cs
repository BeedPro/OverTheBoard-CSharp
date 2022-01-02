using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OverTheBoard.Data.Entities;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.WebUI.Models;

namespace OverTheBoard.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<OverTheBoardUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<OverTheBoardUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            // Checking if user is logged on and redirecting to Dashboard
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect("~/Dashboard");
            }
            return View();
        }
        public IActionResult Temp()
        {
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
