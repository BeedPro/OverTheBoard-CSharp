using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OverTheBoard.Core.Security.Data;
using OverTheBoard.WebUI.Models;

namespace OverTheBoard.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<OverTheBoardUser> _signInManager;
        private readonly UserManager<OverTheBoardUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            SignInManager<OverTheBoardUser> signInManager,
            UserManager<OverTheBoardUser> userManager,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {

            return View();
        }
        
        
        

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            returnUrl ??= Url.Content("~/Dashboard/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress) ?? await _userManager.FindByNameAsync(model.EmailAddress);
                if (user != null)
                {
                    var checkResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
                    if (checkResult.Succeeded)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: true);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation(1, "User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                    }
                }

                ModelState.AddModelError("Password", "Invalid login attempt.");
            }

            return View(model);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Register(string returnUrl)
        {
            return View(new RegistrationViewModel());
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress) ?? await _userManager.FindByNameAsync(model.EmailAddress);

                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, "This email account is already registered.");
                }
                else
                {
                    // Defines a new user
                    var userId = Guid.NewGuid().ToString();
                    user = new OverTheBoardUser()
                    {
                        Id = userId,
                        DisplayName = model.DisplayName,
                        UserName = model.EmailAddress,
                        Email = model.EmailAddress,
                        DisplayImagePath = "defaultDisplayPic.jpeg",
                        LockoutEnabled = true,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Success");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            var safeErrorCode = error.Code ?? "";
                            ModelState.AddModelError(safeErrorCode, error.Description);
                        }
                    }
                }
            }
            return View("Register", model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Success()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}