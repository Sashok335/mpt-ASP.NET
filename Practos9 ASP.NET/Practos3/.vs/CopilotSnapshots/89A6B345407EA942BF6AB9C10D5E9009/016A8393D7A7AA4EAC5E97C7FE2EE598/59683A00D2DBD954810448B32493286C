using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCartsController : Controller
    {
        private readonly WebShopDbContext _context;

        public AdminCartsController(WebShopDbContext context)
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
            var carts = await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                .OrderBy(c => c.CartID)
                .ToListAsync();

            return View(carts);
        }

        public async Task<IActionResult> Create()
        {
            PopulateAdminUserInfo();
            ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Login).ToListAsync(), "UserID", "Login");
            return View(new AdminCart());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCart cart)
        {
            PopulateAdminUserInfo();

            if (!ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Login).ToListAsync(), "UserID", "Login", cart.UserID);
                return View(cart);
            }

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Корзина #{cart.CartID} создана.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            PopulateAdminUserInfo();
            if (id == null) return NotFound();
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) return NotFound();

            ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Login).ToListAsync(), "UserID", "Login", cart.UserID);
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminCart cart)
        {
            PopulateAdminUserInfo();
            if (id != cart.CartID) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Login).ToListAsync(), "UserID", "Login", cart.UserID);
                return View(cart);
            }

            _context.Update(cart);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Корзина #{cart.CartID} обновлена.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            PopulateAdminUserInfo();
            if (id == null) return NotFound();
            var cart = await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.CartID == id);
            if (cart == null) return NotFound();

            return View(cart);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            PopulateAdminUserInfo();
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null) return RedirectToAction(nameof(Index));

            var items = _context.CartItems.Where(x => x.CartID == id);
            _context.CartItems.RemoveRange(items);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Корзина #{id} удалена.";
            return RedirectToAction(nameof(Index));
        }
    }
}


