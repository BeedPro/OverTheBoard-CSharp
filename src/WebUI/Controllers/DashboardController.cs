using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Core.Security.Data;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OverTheBoard.WebUI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IFileUploader _fileUploader;
        private readonly UserManager<OverTheBoardUser> _userManager;
        private readonly SignInManager<OverTheBoardUser> _signInManager;
        private readonly SecurityDbContext _securityDbContext;
        public DashboardController(IFileUploader fileUploader, UserManager<OverTheBoardUser> userManager, SignInManager<OverTheBoardUser> signInManager, SecurityDbContext securityDbContext)
        {
            _fileUploader = fileUploader;
            _userManager = userManager;
            _signInManager = signInManager;
            _securityDbContext = securityDbContext;
        }


        [HttpGet]
        public async Task<IActionResult> Index(DashboardViewModel model)
        {
            // Gets the users DisplayName and DisplayImage and returns the model to output it on the view
            string path = "";
            var user = await _userManager.GetUserAsync(User);
            model.DisplayName = user.DisplayName;
            model.DisplayImagePath = $"{path}{user.DisplayImagePath}";
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
        public IActionResult GameHistory()
        {
            return View();
        }
        public IActionResult Leaderboard()
        {
            return View();
        }

        public IActionResult Analysis()
        {
            return View();
        }


        private async Task<bool> UserClash(string displayName, string displayNameId)
        {
            var user = await _securityDbContext.Users.FirstOrDefaultAsync(e => e.DisplayName == displayName && e.DisplayNameId == displayNameId);
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}
