using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Controllers
{
    public class AdminOrdersController : Controller
    {
        private readonly WebShopDbContext _context;

        public AdminOrdersController(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .OrderBy(o => o.OrderID)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Login).ToListAsync(), "UserID", "Login");
            return View(new AdminOrder { OrderDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminOrder order)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Login).ToListAsync(), "UserID", "Login", order.UserID);
                return View(order);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Заказ #{order.OrderID} создан.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Login).ToListAsync(), "UserID", "Login", order.UserID);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminOrder order)
        {
            if (id != order.OrderID) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(await _context.Users.OrderBy(u => u.Login).ToListAsync(), "UserID", "Login", order.UserID);
                return View(order);
            }

            _context.Update(order);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Заказ #{order.OrderID} обновлен.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderID == id);
            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return RedirectToAction(nameof(Index));

            var items = _context.OrderItems.Where(x => x.OrderID == id);
            _context.OrderItems.RemoveRange(items);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Заказ #{id} удален.";
            return RedirectToAction(nameof(Index));
        }
    }
}

