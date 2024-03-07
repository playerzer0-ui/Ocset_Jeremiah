using Jeremiah_SupermarketOnline.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Jeremiah_SupermarketOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Privacy()
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            return View();
        }

        public IActionResult Login()
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            if(loginModel.Name == "admin" && loginModel.Password == "password")
            {
                HttpContext.Session.SetString("UserName", "admin");
                HttpContext.Session.CommitAsync();

                return RedirectToAction("Index", "Customers");
            }
            ViewBag.ErrorMessage = "Invalid credentials. Please try again.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Customers");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}