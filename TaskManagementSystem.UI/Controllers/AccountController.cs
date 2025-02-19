using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Infrastructure.Identity;
using TaskManagementSystem.Infrastructure.Services;
using TaskManagementSystem.UI.Models;

namespace TaskManagementSystem.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
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
                // Check if the username (email) already exists.
                var existingUser = await _userManager.FindByNameAsync(model.Email);
                if (existingUser != null)
                {
                    // Throw an exception (or you can return a specific error result).
                    throw new Exception("Username is already taken.");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} created successfully.", model.Email);

                    // Generate email confirmation token.
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                    try
                    {
                        await _emailService.SendEmailAsync(user.Email, "Confirm your email",
                            $"Please confirm your account by clicking <a href='{confirmationLink}'>here</a>.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error sending confirmation email to {Email}", user.Email);
                        return StatusCode(500, "Registration failed: unable to send confirmation email.");
                    }

                    return View("RegistrationPending");
                }
                else
                {
                    // Check if one of the errors indicates a duplicate username.
                    var duplicateError = result.Errors.FirstOrDefault(e => e.Code == "DuplicateUserName");
                    if (duplicateError != null)
                    {
                        throw new Exception("Username is already taken.");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        _logger.LogError("Error creating user {Email}: {Error}", model.Email, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid email confirmation request.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("ConfirmEmailSuccess");
            }
            else
            {
                return View("Error");
            }
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
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Please confirm your email before logging in.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} logged in successfully.", model.Email);
                    if (!string.IsNullOrEmpty(returnUrl))
                        return LocalRedirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Projects");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                _logger.LogWarning("Invalid login attempt for {Email}.", model.Email);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }
    }
}
