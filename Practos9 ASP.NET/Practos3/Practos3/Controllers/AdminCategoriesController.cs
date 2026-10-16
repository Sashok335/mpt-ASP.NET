using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCategoriesController : Controller
    {
        private readonly WebShopDbContext _context;

        public AdminCategoriesController(WebShopDbContext context)
        {
            _context = context;
        }

        private void PopulateAdminUserInfo()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var name = User.Identity.Name ?? "";
                var id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";
                ViewBag.AdminUserName = name;
                ViewBag.AdminUserId = id;
                ViewBag.AdminUserAvatar = string.IsNullOrEmpty(name) ? "#" : name.Substring(0, 1).ToUpperInvariant();
            }
        }

        public async Task<IActionResult> Index()
        {
            PopulateAdminUserInfo();
            var categories = await _context.Categories
                .Include(c => c.Products)
                .OrderBy(c => c.CategoryID)
                .ToListAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            PopulateAdminUserInfo();
            return View(new AdminCategory());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCategory category)
        {
            PopulateAdminUserInfo();

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Категория \"{category.CategoryName}\" успешно создана.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            PopulateAdminUserInfo();

            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminCategory category)
        {
            PopulateAdminUserInfo();

            if (id != category.CategoryID)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Update(category);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Категория \"{category.CategoryName}\" успешно обновлена.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            PopulateAdminUserInfo();

            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            PopulateAdminUserInfo();

            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (category.Products.Any())
            {
                TempData["ErrorMessage"] = $"Категория \"{category.CategoryName}\" содержит товары. Сначала перенесите или удалите их.";
                return RedirectToAction(nameof(Index));
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Категория \"{category.CategoryName}\" удалена.";
            return RedirectToAction(nameof(Index));
        }
    }
}
