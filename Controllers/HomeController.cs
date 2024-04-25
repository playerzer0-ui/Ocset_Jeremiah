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
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");
            return View();
        }

        public IActionResult Fallback()
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}