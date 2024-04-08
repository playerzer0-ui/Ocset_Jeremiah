using Jeremiah_SupermarketOnline.Data;
using Jeremiah_SupermarketOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Jeremiah_SupermarketOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Jeremiah_SupermarketOnlineContext _context;

        public HomeController(ILogger<HomeController> logger, Jeremiah_SupermarketOnlineContext context)
        {
            _logger = logger;
            _context = context;
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
            var user = _context.Users.FirstOrDefault(u => u.Username == loginModel.Name && u.Password == loginModel.Password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.Username);
                HttpContext.Session.SetInt32("UserType", user.UserType);
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