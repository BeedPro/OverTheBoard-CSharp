using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public DashboardController(IFileUploader fileUploader, UserManager<OverTheBoardUser> userManager, SignInManager<OverTheBoardUser> signInManager)
        {
            _fileUploader = fileUploader;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(DashboardViewModel model)
        {
            //string path = "C:/Users/Beed/source/repos/BeedPro/OverTheBoard/src/WebUI/Uploads/DisplayImages/";
            string path = "";
            var user = await _userManager.GetUserAsync(User);
            model.DisplayName = user.DisplayName;
            model.DisplayImagePath = $"{path}{user.DisplayImagePath}";
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Settings(SettingsViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            model.DisplayImagePath = user.DisplayImagePath;
            model.Email = user.Email;
            model.DisplayName = user.DisplayName;
            model.OldPassword = "lock";
            model.NewPassword = "lock";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(SettingsViewModel model, IFormFile file)
        {
            var filename = await _fileUploader.UploadImage(file);
            var user = await _userManager.GetUserAsync(User);
            
            //Checks if the display image pic has been uploaded to the site
            if (filename != null)
            {
                user.DisplayImagePath = filename;
                model.DisplayImagePath = filename;

            }

            else
            {
                model.DisplayImagePath = user.DisplayImagePath;
            }

            // TO-DO: Check if the username is in database
            //Checks and then changes the DisplayName
            if (model.DisplayName != null)
            {
                user.DisplayName = model.DisplayName;
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                if (user != null)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return View(model);
                }
            }

            model.Email = user.Email;
            model.DisplayName = user.DisplayName;
            await _userManager.UpdateAsync(user);
            return View(model);
            
        }
    }
}
