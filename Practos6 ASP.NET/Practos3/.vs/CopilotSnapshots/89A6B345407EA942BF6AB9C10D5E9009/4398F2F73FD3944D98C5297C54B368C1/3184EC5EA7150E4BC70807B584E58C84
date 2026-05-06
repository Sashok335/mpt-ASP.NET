using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Controllers
{
    public class CartController : Controller
    {
        private const string SiteGuestLogin = "site_guest";
        private const string SiteGuestRole = "Guest";

        private readonly WebShopDbContext _context;

        public CartController(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await GetOrCreateSiteCartAsync();
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.CartID == cart.CartID)
                .OrderBy(c => c.CartItemID)
                .ToListAsync();

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = await GetOrCreateSiteCartAsync();
            var item = await _context.CartItems.FirstOrDefaultAsync(c => c.CartID == cart.CartID && c.ProductID == productId);

            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                _context.CartItems.Add(new AdminCartItem
                {
                    CartID = cart.CartID,
                    ProductID = productId,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Товар «{product.ProductName}» успешно добавлен в корзину!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int amount)
        {
            var cart = await GetOrCreateSiteCartAsync();
            var item = await _context.CartItems.FirstOrDefaultAsync(c => c.CartItemID == id && c.CartID == cart.CartID);
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
            var cart = await GetOrCreateSiteCartAsync();
            var item = await _context.CartItems.FirstOrDefaultAsync(c => c.CartItemID == id && c.CartID == cart.CartID);
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
            var cart = await GetOrCreateSiteCartAsync();
            var cartItems = _context.CartItems.Where(c => c.CartID == cart.CartID);

            if (await cartItems.AnyAsync())
            {
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Корзина успешно очищена";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = await GetOrCreateSiteCartAsync();
            var cartItems = await _context.CartItems.Where(c => c.CartID == cart.CartID).ToListAsync();
            if (!cartItems.Any())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment()
        {
            var cart = await GetOrCreateSiteCartAsync();
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.CartID == cart.CartID)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index");
            }

            var order = new AdminOrder
            {
                UserID = cart.UserID,
                OrderDate = DateTime.Now,
                Status = "Оплачен",
                TotalAmount = cartItems.Sum(x => x.Quantity * (x.Product?.Price ?? 0))
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cartItems)
            {
                _context.OrderItems.Add(new AdminOrderItem
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product?.Price ?? 0
                });
            }

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Оплата прошла успешно, заказ сохранен в новой базе.";
            return RedirectToAction("Index", "Home");
        }

        private async Task<AdminCart> GetOrCreateSiteCartAsync()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == SiteGuestRole);
            if (role == null)
            {
                role = new AdminRole { RoleName = SiteGuestRole };
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == SiteGuestLogin);
            if (user == null)
            {
                user = new AdminUser
                {
                    Login = SiteGuestLogin,
                    PasswordHash = "site-managed",
                    RoleID = role.RoleID
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserID == user.UserID);
            if (cart == null)
            {
                cart = new AdminCart { UserID = user.UserID };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }
    }
}

