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
    public class OrdersController : Controller
    {
        private readonly Jeremiah_SupermarketOnlineContext _context;

        public OrdersController(Jeremiah_SupermarketOnlineContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var jeremiah_SupermarketOnlineContext = _context.Order.Include(o => o.Customer).Include(o => o.Product);
            return View(await jeremiah_SupermarketOnlineContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name");
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,Quantity,CustomerId,ProductId")] OrderCreateViewModel order)
        {
            if (ModelState.IsValid)
            {
                var orderEntity = new Order
                {
                    OrderDate = order.OrderDate,
                    Quantity = order.Quantity,
                    CustomerId = order.CustomerId,
                    ProductId = order.ProductId
                };

                orderEntity.Customer = await _context.Customer.FirstOrDefaultAsync(m => m.Id == order.CustomerId);
                if(orderEntity.Customer == null)
                {
                    return View(order);
                }
                orderEntity.Product = await _context.Product.FirstOrDefaultAsync(m => m.Id == order.ProductId);
                if (orderEntity.Product == null)
                {
                    return View(order);
                }

                _context.Add(orderEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", order.ProductId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", order.ProductId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,Quantity,CustomerId,ProductId")] OrderEditViewModel order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingOrder = await _context.Order.FindAsync(id);

                    if (existingOrder == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing order with the values from the view model
                    existingOrder.OrderDate = order.OrderDate;
                    existingOrder.Quantity = order.Quantity;
                    existingOrder.CustomerId = order.CustomerId;
                    existingOrder.ProductId = order.ProductId;

                    _context.Update(existingOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", order.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name", order.ProductId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'Jeremiah_SupermarketOnlineContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
