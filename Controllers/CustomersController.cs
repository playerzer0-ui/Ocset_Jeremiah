using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jeremiah_SupermarketOnline.Data;
using Jeremiah_SupermarketOnline.Models;

namespace Jeremiah_SupermarketOnline.Controllers
{
    public class CustomersController : Controller
    {
        private readonly Jeremiah_SupermarketOnlineContext _context;

        public CustomersController(Jeremiah_SupermarketOnlineContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");

            if (HttpContext.Session.GetInt32("UserType") == 1)
            {
                return _context.Customer != null ? 
                          View(await _context.Customer.ToListAsync()) :
                          Problem("Entity set 'Jeremiah_SupermarketOnlineContext.Customer'  is null.");
            }
            else if(HttpContext.Session.GetInt32("UserType") == 0)
            {
                return RedirectToAction("Index", "Products");
            }
            else
            {
                return RedirectToAction("Login", "Customers");
            }

        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Customers");
            }
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Password,Address")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Customers");
            }
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Password,Address")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Customers");
            }
            if (id == null || _context.Customer == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customer == null)
            {
                return Problem("Entity set 'Jeremiah_SupermarketOnlineContext.Customer'  is null.");
            }
            var customer = await _context.Customer.FindAsync(id);
            if (customer != null)
            {
                _context.Customer.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customer?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with the provided username already exists
                var existingUser = await _context.Customer.FirstOrDefaultAsync(c => c.Name == registerModel.Name);

                if (existingUser != null)
                {
                    // User with the same username already exists, return to the registration view with an error message
                    ViewBag.ErrorMessage = "A user with the same username already exists. Please choose a different username.";
                    return View();
                }

                // Create a new customer entity with the provided data
                var newCustomer = new Customer
                {
                    Name = registerModel.Name,
                    Password = registerModel.Password,
                    Address = registerModel.Address
                };

                // Add the new customer to the database
                _context.Customer.Add(newCustomer);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("UserName", newCustomer.Name);
                HttpContext.Session.SetInt32("UserId", newCustomer.Id);
                HttpContext.Session.SetInt32("UserType", newCustomer.UserType);
                await HttpContext.Session.CommitAsync();

                // Redirect to the appropriate action based on user type or requirement
                return RedirectToAction("Index", "Customers");
            }

            // ModelState is not valid, return to the registration view
            return View(registerModel);
        }


        public IActionResult Login()
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                // Check if a customer with the provided username exists in the database
                var customer = _context.Customer.FirstOrDefault(c => c.Name == loginModel.Name && c.Password == loginModel.Password);

                if (customer != null)
                {
                    // Customer exists, set session variables or perform any other required actions
                    HttpContext.Session.SetString("UserName", customer.Name);
                    HttpContext.Session.SetInt32("UserId", customer.Id);
                    HttpContext.Session.SetInt32("UserType", customer.UserType);
                    HttpContext.Session.CommitAsync();

                    // Redirect to the appropriate action based on user type or requirement
                    return RedirectToAction("Index", "Customers");
                }
                else
                {
                    // Customer does not exist or credentials are invalid
                    ViewBag.ErrorMessage = "Invalid credentials. Please try again.";
                    return View();
                }
            }

            // ModelState is not valid, return to the login view
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Customers");
        }
    }
}
