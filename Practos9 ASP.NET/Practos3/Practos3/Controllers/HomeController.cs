using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;

namespace Practos3.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebShopDbContext _context;

        public HomeController(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.CategoryName).ToListAsync();

            var productsQuery = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryID == categoryId.Value);
                ViewBag.CurrentCategory = categoryId;
            }

            var products = await productsQuery.OrderBy(p => p.ProductID).ToListAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

