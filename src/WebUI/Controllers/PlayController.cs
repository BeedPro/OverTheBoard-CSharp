using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverTheBoard.WebUI.Controllers
{
    //TODO: [Authorize]
    public class PlayController : Controller
    {
        public IActionResult Index()
        {
            // Checks if player is ranked or not and redirects to the corresponding page
            // Not Implemented Properly
            if (true)
            {
                return LocalRedirect("~/Play/Unranked");
            }
            else
            {
                return LocalRedirect("~/Play/Brackets");
            }
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
