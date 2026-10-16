using Microsoft.AspNetCore.Mvc;

namespace Practos3.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "AdminProducts");
        }
    }
}
