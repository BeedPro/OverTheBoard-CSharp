using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using OverTheBoard.Data.Entities;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.WebUI.Models;

namespace OverTheBoard.WebUI.Controllers
{
    public class AccountController : Controller
    {
        // TODO Use Repos instead of dependancy injection
        private readonly SignInManager<OverTheBoardUser> _signInManager;
        private readonly UserManager<OverTheBoardUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AccountController(
            SignInManager<OverTheBoardUser> signInManager,
            UserManager<OverTheBoardUser> userManager,
            ILogger<AccountController> logger,
            IUserService userService,
            IEmailService emailService
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _userService = userService;
            _emailService = emailService;
        }
        [HttpGet]
        public IActionResult Login()
        {

            // Checking if user is logged on and redirecting to Dashboard
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect("~/dashboard");
            }
            return View();
        }
        
        
        

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (returnUrl == null)
            {
                returnUrl = Url.Content("~/dashboard/");
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress) ?? await _userManager.FindByNameAsync(model.EmailAddress);
                var emailVerified = await _userManager.IsEmailConfirmedAsync(user);
                if (user != null)
                {
                    var checkResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
                    if (checkResult.Succeeded)
                    {
                        if (emailVerified)
                        {
                            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: true);
                            if (result.Succeeded)
                            {
                                _logger.LogInformation(1, "User logged in.");
                                return LocalRedirect(returnUrl);
                            }
                        }
                        else
                        {
                            //TODO: Add message to show that email is not verified and implement resend email verification
                            ModelState.AddModelError("Email", "Email not Verified");
                            return RedirectToAction("VerifyEmailMsg", "Account", new { email = user.Email, userId = user.Id });
                        }
                    }
                }

                ModelState.AddModelError("Password", "Invalid login attempt.");
            }

            return View(model);
        }

        public IActionResult EmailSent()
        {
            //Checking if user is logged on and redirecting to Dashboard
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect("~/dashboard");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmailMsg(VerifyEmailMsgViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress) ?? await _userManager.FindByNameAsync(model.EmailAddress);
                if (user != null)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var tokenLink = Url.Action("EmailVerification", "Account",
                        new { token, idToken = user.Id, returnUrl }, "https");
                    await _emailService.SendRegistrationEmailAsync(user.Email, user.DisplayName,tokenLink);
                    return View("EmailSent");
                }
            }
            
            return View("ErrorVerify");
        }

        [HttpGet]
        //TODO: Have a resend verification email on the page
        public IActionResult VerifyEmailMsg(string email)
        {
            var model = new VerifyEmailMsgViewModel();
            model.EmailAddress = email;
            //Checking if user is logged on and redirecting to Dashboard
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect("~/dashboard");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            //Checking if user is logged on and redirecting to Dashboard
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect("~/dashboard");
            }
            return View(new RegistrationViewModel());
        }


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
                    var uniqueDisplayNameAndId = await GenerateUniqueDisplayName(model.DisplayName);
                    user = new OverTheBoardUser
                    {
                        Id = userId,
                        DisplayName = uniqueDisplayNameAndId.Item1,
                        DisplayNameId = uniqueDisplayNameAndId.Item2,
                        UserName = model.EmailAddress,
                        Email = model.EmailAddress,
                        DisplayImagePath = "defaultDisplayPic.jpeg",
                        Rank = UserRank.Unranked,
                        Rating = 1200,
                        LockoutEnabled = true,
                        EmailConfirmed = false
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var tokenLink = Url.Action("EmailVerification", "Account",
                        new {token, idToken = userId, returnUrl }, "https");

                    //TODO: Add implementation of Visitor Role for unverified accounts [Maybe]
                    await _emailService.SendRegistrationEmailAsync(user.Email, user.DisplayName,tokenLink);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Success");
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        var safeErrorCode = error.Code ?? "";
                        ModelState.AddModelError(safeErrorCode, error.Description);
                    }
                }
            }
            return View("Register", model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.EmailAddress);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, "https");
                    await _emailService.SendPasswordResetEmailAsync(user.Email, user.DisplayName,token, callback);
                }

                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel() { Token = token, EmailAddress = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));
            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EmailVerification(string token, string idToken, string returnUrl)
        {
            //TODO: implement verification
            var userId = idToken;
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                return View(result.Succeeded ? nameof(EmailVerification) : "ErrorVerify");
            }

            //Checks if user is signed in and redirects to dashboard
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect("~/dashboard");
            }
            return View("ErrorVerify");
            
        }

        public IActionResult ErrorVerify()
        {
            //Checks if user is signed in and redirects to dashboard
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect("~/dashboard");
            }
            return View();
        }

        private async Task<Tuple<string, string>> GenerateUniqueDisplayName(string displayName, string number=null)
        {
            Random random = new Random();
            if (string.IsNullOrEmpty(number))
            {
                number = random.Next(0, 9999).ToString().PadLeft(4, '0');
            }
            var user = await _userService.GetUserDisplayNameAndyNameIdAsync(displayName, number);

            while ( user != null)
            {
                number = random.Next(0, 9999).ToString().PadLeft(4, '0');
                user = await _userService.GetUserDisplayNameAndyNameIdAsync(displayName, number);
            }
            return Tuple.Create(displayName, number);
        }

        public IActionResult Success()
        {
            //Checking if user is logged on and redirecting to Dashboard
            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect("~/dashboard");
            }
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