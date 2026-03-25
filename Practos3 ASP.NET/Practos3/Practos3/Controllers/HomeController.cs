using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practos3.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShoppContext _context;

        public HomeController(ShoppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            
            var productsQuery = _context.Products.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId);
                ViewBag.CurrentCategory = categoryId;
            }

            var products = await productsQuery.ToListAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}