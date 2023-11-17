using Microsoft.AspNetCore.Mvc;

namespace QuickPaste.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
