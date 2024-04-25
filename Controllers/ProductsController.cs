using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jeremiah_SupermarketOnline.Data;
using Jeremiah_SupermarketOnline.Models;
using RestSharp;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;

namespace Jeremiah_SupermarketOnline.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Jeremiah_SupermarketOnlineContext _context;
        private RestClient client;
        private RestRequest request;
        private RestResponse response;

        public ProductsController(Jeremiah_SupermarketOnlineContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? price, string? SearchString)
        {
           
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");
            return _context.Product != null ?
                         View(await _context.Product.ToListAsync()) :
                         Problem("Entity set 'Jeremiah_SupermarketOnlineContext.Product'  is null.");

        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
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

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Customers");
            }
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Customers");
            }
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'Jeremiah_SupermarketOnlineContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Recipe()
        {
            client = new RestClient("https://www.themealdb.com/api/json/v1/1/");
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");

            var randomProduct = await _context.Product
            .OrderBy(p => Guid.NewGuid())
            .Select(p => p.Name)
            .FirstOrDefaultAsync();

            request = new RestRequest("filter.php?i=" + randomProduct);
            response = client.Execute(request);

            RootobjectFilter rootFilter = JsonConvert.DeserializeObject<RootobjectFilter>(response.Content);
            FilterMeal[] filterMeals = rootFilter.meals;
            Random rg = new Random();
            int rand = rg.Next(0, filterMeals.Length);
            string id = filterMeals[rand].idMeal;

            request = new RestRequest("lookup.php?i=" + id);
            response = client.Execute(request);

            Rootobject root = JsonConvert.DeserializeObject<Rootobject>(response.Content);
            Recipe[] meals = root.meals;


            return View(meals[0]);
        }
   

   public async Task<IActionResult> Filter(int? price, string? allProducts)
    {
            ViewData["name"] = HttpContext.Session.GetString("UserName");
            ViewData["userType"] = HttpContext.Session.GetInt32("UserType");

            if (_context.Product == null)
        {
            return Problem("Entity set 'Jeremiah_SupermarketOnlineContext.Product'  is null.");
        }
        var products = from p in _context.Product
                      select p;

            Filter f =new Filter();
            List<Product> allProduct = new List<Product>();
            List<Product> filteredProducts = new List<Product>();
            foreach (var item in products)
            {
                allProduct.Add(item);
            }
            f.allProducts = allProduct;
                if (!string.IsNullOrEmpty(allProducts))
        {
            products = products.Where(s => s.Name!.Contains(allProducts));
        }

        if (price >= 0)
        {
            products = products.Where(x => x.Price <= price);

        }
            foreach (var item in products)
            {
                filteredProducts.Add(item);
            }
            f.filteredProducts = filteredProducts;


            return View(f);

    }
}

}
