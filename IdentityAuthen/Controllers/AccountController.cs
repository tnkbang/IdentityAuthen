using IdentityAuthen.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IdentityAuthen.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public AccountController(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View(new LoginModel(_signInManager, _logger));
        }
        [HttpPost]
        public async Task<IActionResult> getLogin(string u, string p)
        {
            var result = await _signInManager.PasswordSignInAsync(u, p, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return Json(new { tt = "User logged in." });
            }
            if (result.RequiresTwoFactor)
            {
                return Json(new { tt = "2fa" });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return Json(new { tt = "User account locked out." });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Json(new { tt = "Invalid login attempt." });
            }
        }
    }
}
