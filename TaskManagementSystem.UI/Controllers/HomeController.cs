using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
