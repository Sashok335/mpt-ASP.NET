using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ShoppingListApp.Controllers
{
    public class HomeController : Controller
    {
        private static Dictionary<string, string> _userData = new Dictionary<string, string>();
        public IActionResult Index() => View();

        public IActionResult Profile()
        {
            ViewBag.UserData = _userData;
            return View();
        }

        [HttpPost]
        public IActionResult SaveProfile(string name, string age, string city, string phone)
        {
            _userData.Clear();

            if (!string.IsNullOrWhiteSpace(name))
                _userData["name"] = name.Trim();

            if (!string.IsNullOrWhiteSpace(age))
                _userData["age"] = age.Trim();

            if (!string.IsNullOrWhiteSpace(city))
                _userData["city"] = city.Trim();

            if (!string.IsNullOrWhiteSpace(phone))
                _userData["phone"] = phone.Trim();

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult ClearProfile()
        {
            _userData.Clear();
            return RedirectToAction("Profile");
        }

        public IActionResult About() => View();
    }
}