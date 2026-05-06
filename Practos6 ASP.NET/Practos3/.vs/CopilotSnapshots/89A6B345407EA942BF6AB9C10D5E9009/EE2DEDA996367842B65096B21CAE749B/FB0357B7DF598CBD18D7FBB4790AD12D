using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Controllers
{
    public class AdminCartItemsController : Controller
    {
        private readonly WebShopDbContext _context;

        public AdminCartItemsController(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.CartItems
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .OrderBy(c => c.CartItemID)
                .ToListAsync();

            return View(items);
        }

        public async Task<IActionResult> Create()
        {
            await LoadLists();
            return View(new AdminCartItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCartItem cartItem)
        {
            if (!ModelState.IsValid)
            {
                await LoadLists(cartItem.CartID, cartItem.ProductID);
                return View(cartItem);
            }

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Элемент корзины успешно создан.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null) return NotFound();

            await LoadLists(cartItem.CartID, cartItem.ProductID);
            return View(cartItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminCartItem cartItem)
        {
            if (id != cartItem.CartItemID) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadLists(cartItem.CartID, cartItem.ProductID);
                return View(cartItem);
            }

            _context.Update(cartItem);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Элемент корзины успешно обновлен.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cartItem = await _context.CartItems
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.CartItemID == id);

            if (cartItem == null) return NotFound();

            return View(cartItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null) return RedirectToAction(nameof(Index));

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Элемент корзины удален.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLists(int? cartId = null, int? productId = null)
        {
            ViewBag.Carts = new SelectList(await _context.Carts.OrderBy(c => c.CartID).ToListAsync(), "CartID", "CartID", cartId);
            ViewBag.Products = new SelectList(await _context.Products.OrderBy(p => p.ProductName).ToListAsync(), "ProductID", "ProductName", productId);
        }
    }
}

