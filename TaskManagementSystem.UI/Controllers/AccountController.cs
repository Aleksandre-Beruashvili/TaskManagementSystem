using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.UI.Models;
using TaskManagementSystem.UI.Services;

namespace TaskManagementSystem.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthApiService _authApiService;
        private readonly ITokenService _tokenService;

        [HttpGet]
        public IActionResult RegistrationConfirmation()
        {
            ViewBag.UseAlternateLayout = true;
            ViewBag.Message = "Registration successful! A confirmation email has been sent to your email address. Please check your email and click the confirmation link to activate your account.";
            return View();  // This looks for Views/Account/RegistrationConfirmation.cshtml
        }

        public AccountController(IAuthApiService authApiService, ITokenService tokenService)
        {
            _authApiService = authApiService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.UseAlternateLayout = true; // Use minimal header for registration page.
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var registerDto = new RegisterUserDto
                {
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };

                try
                {
                    var resultMessage = await _authApiService.RegisterAsync(registerDto);
                    // Optionally, display a message or auto-login
                    return RedirectToAction("RegistrationConfirmation");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Registration failed.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.UseAlternateLayout = true; // Use minimal header for login page.
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var loginDto = new LoginUserDto
                {
                    Email = model.Email,
                    Password = model.Password
                };

                try
                {
                    var token = await _authApiService.LoginAsync(loginDto);
                    if (string.IsNullOrEmpty(token))
                    {
                        ModelState.AddModelError(string.Empty, "Received empty token.");
                        return View(model);
                    }

                    _tokenService.SetToken(token);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Email)
                    };
                    var identity = new ClaimsIdentity(claims, "Cookies");
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("Cookies", principal);

                    return RedirectToAction("Index", "Home");
                }
                catch (System.Exception ex)
                {
                    // This will catch the "Email not confirmed..." error
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _tokenService.SetToken(null);
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }
    }
}
