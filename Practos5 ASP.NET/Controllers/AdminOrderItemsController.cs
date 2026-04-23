using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Controllers
{
    public class AdminOrderItemsController : Controller
    {
        private readonly WebShopDbContext _context;

        public AdminOrderItemsController(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.OrderItems
                .Include(x => x.Order)
                .Include(x => x.Product)
                .OrderBy(x => x.OrderItemID)
                .ToListAsync();

            return View(items);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLists();
            return View(new AdminOrderItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminOrderItem item)
        {
            if (!ModelState.IsValid)
            {
                await LoadLists(item.OrderID, item.ProductID);
                return View(item);
            }

            _context.OrderItems.Add(item);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Позиция заказа #{item.OrderItemID} создана.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null) return NotFound();

            await LoadLists(item.OrderID, item.ProductID);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminOrderItem item)
        {
            if (id != item.OrderItemID) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadLists(item.OrderID, item.ProductID);
                return View(item);
            }

            _context.Update(item);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Позиция заказа #{item.OrderItemID} обновлена.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.OrderItems
                .Include(x => x.Order)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.OrderItemID == id);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null) return RedirectToAction(nameof(Index));

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Позиция заказа #{id} удалена.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLists(int? orderId = null, int? productId = null)
        {
            ViewBag.Orders = new SelectList(await _context.Orders.OrderBy(o => o.OrderID).ToListAsync(), "OrderID", "OrderID", orderId);
            ViewBag.Products = new SelectList(await _context.Products.OrderBy(p => p.ProductName).ToListAsync(), "ProductID", "ProductName", productId);
        }
    }
}

