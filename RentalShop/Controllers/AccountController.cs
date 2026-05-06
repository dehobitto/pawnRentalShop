using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RentalShop.Domain.Entities;
using RentalShop.Infrastructure.Persistence;
using RentalShop.Models;

namespace RentalShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly RentalDbContext _db;
        private readonly ILogger<AccountController> _logger;

        public AccountController(RentalDbContext db, ILogger<AccountController> logger)
        {
            _db     = db;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Item");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == vm.Username);

            if (user is null || !VerifyPassword(vm.Password, user.PasswordHash))
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(vm);
            }

            await SignInAsync(user.Username, vm.RememberMe);
            _logger.LogInformation("User {Username} signed in", user.Username);
            TempData["SuccessMessage"] = $"Welcome back, {user.Username}!";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Item");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Item");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var taken = await _db.Users.AnyAsync(u => u.Username == vm.Username);
            if (taken)
            {
                ModelState.AddModelError(nameof(vm.Username), "Username is already taken.");
                return View(vm);
            }

            var user = new AppUser
            {
                Username     = vm.Username,
                PasswordHash = HashPassword(vm.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            await SignInAsync(user.Username, isPersistent: false);
            _logger.LogInformation("User {Username} registered and signed in", user.Username);
            TempData["SuccessMessage"] = $"Account created. Welcome, {vm.Username}!";

            return RedirectToAction("Index", "Item");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User {Username} signed out", User.Identity?.Name);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["SuccessMessage"] = "You have been signed out.";
            return RedirectToAction("Login");
        }

        private Task SignInAsync(string username, bool isPersistent)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Staff")
            };
            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props     = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc   = isPersistent ? DateTimeOffset.UtcNow.AddDays(30) : (DateTimeOffset?)null
            };
            return HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
        }

        private static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, 100_000, HashAlgorithmName.SHA256, 32);
            return $"{Convert.ToHexString(salt)}:{Convert.ToHexString(hash)}";
        }

        private static bool VerifyPassword(string password, string stored)
        {
            var parts = stored.Split(':');
            if (parts.Length != 2) return false;
            var salt         = Convert.FromHexString(parts[0]);
            var expectedHash = Convert.FromHexString(parts[1]);
            var actualHash   = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, 100_000, HashAlgorithmName.SHA256, 32);
            return CryptographicOperations.FixedTimeEquals(expectedHash, actualHash);
        }
    }
}
