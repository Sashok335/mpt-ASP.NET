using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practos3.Models;
using Practos3.Models;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        private readonly ShoppContext _context;

        public CartController(ShoppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.CartItems.Include(c => c.Product).ToListAsync();
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return NotFound();

            var item = await _context.CartItems.FirstOrDefaultAsync(c => c.ProductId == productId);
            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                _context.CartItems.Add(new CartItem { ProductId = productId, Quantity = 1, CartId = "SessionCart" });
            }
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Товар «{product.Name}» успешно добавлен в корзину!";

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int amount)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item != null)
            {
                item.Quantity += amount;
                if (item.Quantity <= 0)
                {
                    _context.CartItems.Remove(item); 
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index"); 
        }

        public async Task<IActionResult> Remove(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var cartItems = _context.CartItems.Where(c => c.CartId == "SessionCart");

            if (cartItems.Any())
            {
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Корзина успешно очищена";
            }

            return RedirectToAction("Index");
        }
        public IActionResult Checkout()
        {
            var cartItems = _context.CartItems.Where(c => c.CartId == "SessionCart").ToList();
            if (!cartItems.Any()) return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment()
        {
            var cartItems = _context.CartItems.Where(c => c.CartId == "SessionCart");
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Оплата прошла успешно, микроайм оформлен автоматически :)";
            return RedirectToAction("Index", "Home");
        }
    }
}