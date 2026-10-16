using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Controllers
{
    public class AdminCartsController : Controller
    {
        private readonly WebShopDbContext _context;

        public AdminCartsController(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var carts = await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                .OrderBy(c => c.CartID)
                .ToListAsync();

            return View(carts);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Login).ToListAsync(), "UserID", "Login");
            return View(new AdminCart());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCart cart)
        {
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

