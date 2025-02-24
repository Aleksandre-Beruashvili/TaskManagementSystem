using Microsoft.AspNetCore.Mvc;
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

        public AccountController(IAuthApiService authApiService, ITokenService tokenService)
        {
            _authApiService = authApiService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public IActionResult Register()
        {
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
                    // Optionally log in automatically or show a confirmation message.
                    return RedirectToAction("Index", "Home");
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
                    _tokenService.SetToken(token);
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials.");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _tokenService.SetToken(null);
            return RedirectToAction("Index", "Home");
        }
    }
}
