using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementSystem.UI.Services;

namespace TaskManagementSystem.UI.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileApiService _profileApiService;
        private readonly ITokenService _tokenService;

        public ProfileController(IProfileApiService profileApiService, ITokenService tokenService)
        {
            _profileApiService = profileApiService;
            _tokenService = tokenService;
        }

        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            await _profileApiService.DeleteAccountAsync();
            _tokenService.SetToken(null);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["AccountDeletedMessage"] = "Your account has been successfully deleted.";
            return RedirectToAction("Index", "Home");
        }
    }
}
