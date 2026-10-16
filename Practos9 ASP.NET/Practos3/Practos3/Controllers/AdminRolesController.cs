using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Controllers
{
    public class AdminRolesController : Controller
    {
        private readonly WebShopDbContext _context;

        public AdminRolesController(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles
                .Include(r => r.Users)
                .OrderBy(r => r.RoleID)
                .ToListAsync();

            return View(roles);
        }

        public IActionResult Create()
        {
            return View(new AdminRole());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminRole role)
        {
            if (!ModelState.IsValid) return View(role);

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Роль \"{role.RoleName}\" создана.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminRole role)
        {
            if (id != role.RoleID) return NotFound();
            if (!ModelState.IsValid) return View(role);

            _context.Update(role);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Роль \"{role.RoleName}\" обновлена.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var role = await _context.Roles.Include(r => r.Users).FirstOrDefaultAsync(r => r.RoleID == id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.Include(r => r.Users).FirstOrDefaultAsync(r => r.RoleID == id);
            if (role == null) return RedirectToAction(nameof(Index));

            if (role.Users.Any())
            {
                TempData["ErrorMessage"] = $"Роль \"{role.RoleName}\" используется пользователями.";
                return RedirectToAction(nameof(Index));
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Роль \"{role.RoleName}\" удалена.";
            return RedirectToAction(nameof(Index));
        }
    }
}

