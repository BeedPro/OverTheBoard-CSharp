using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OverTheBoard.Core.Security.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverTheBoard.WebUI.Controllers
{
    [Authorize]
    public class PlayController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Unranked()
        {
            return View();
        }

        public IActionResult Brackets()
        {
            return View();
        }
        public IActionResult Game()
        {
            return View();
        }
    }
}
