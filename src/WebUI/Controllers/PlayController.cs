using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverTheBoard.WebUI.Controllers
{
    public class PlayController : Controller
    {
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
