using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Controllers
{
    public class AdminUsersController : Controller
    {
        private readonly WebShopDbContext _context;

        public AdminUsersController(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Carts)
                .Include(u => u.Orders)
                .OrderBy(u => u.UserID)
                .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = new SelectList(await _context.Roles.OrderBy(r => r.RoleName).ToListAsync(), "RoleID", "RoleName");
            return View(new AdminUser());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminUser user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(await _context.Roles.OrderBy(r => r.RoleName).ToListAsync(), "RoleID", "RoleName", user.RoleID);
                return View(user);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Пользователь \"{user.Login}\" создан.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            ViewBag.Roles = new SelectList(await _context.Roles.OrderBy(r => r.RoleName).ToListAsync(), "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminUser user)
        {
            if (id != user.UserID) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(await _context.Roles.OrderBy(r => r.RoleName).ToListAsync(), "RoleID", "RoleName", user.RoleID);
                return View(user);
            }

            _context.Update(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Пользователь \"{user.Login}\" обновлен.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Carts)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users
                .Include(u => u.Carts)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null) return RedirectToAction(nameof(Index));

            if (user.Carts.Any() || user.Orders.Any())
            {
                TempData["ErrorMessage"] = $"Пользователь \"{user.Login}\" связан с корзинами или заказами.";
                return RedirectToAction(nameof(Index));
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Пользователь \"{user.Login}\" удален.";
            return RedirectToAction(nameof(Index));
        }
    }
}

