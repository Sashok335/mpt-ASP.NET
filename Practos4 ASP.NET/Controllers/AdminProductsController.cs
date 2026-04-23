using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Controllers
{
    public class AdminProductsController : Controller
    {
        private readonly WebShopDbContext _context;

        public AdminProductsController(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.ProductID)
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(
                await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync(),
                "CategoryID",
                "CategoryName");

            return View(new AdminProduct());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminProduct product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync(),
                    "CategoryID",
                    "CategoryName",
                    product.CategoryID);

                return View(product);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Товар \"{product.ProductName}\" успешно создан.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(
                await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync(),
                "CategoryID",
                "CategoryName",
                product.CategoryID);

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminProduct product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync(),
                    "CategoryID",
                    "CategoryName",
                    product.CategoryID);

                return View(product);
            }

            _context.Update(product);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Товар \"{product.ProductName}\" успешно обновлен.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var cartItems = _context.CartItems.Where(x => x.ProductID == id);
            var orderItems = _context.OrderItems.Where(x => x.ProductID == id);
            _context.CartItems.RemoveRange(cartItems);
            _context.OrderItems.RemoveRange(orderItems);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Товар \"{product.ProductName}\" удален.";
            return RedirectToAction(nameof(Index));
        }
    }
}
