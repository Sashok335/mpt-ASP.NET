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
            var cart = await GetOrCreateCartAsync();
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

            var cart = await GetOrCreateCartAsync();
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
            var cart = await GetOrCreateCartAsync();
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
            var cart = await GetOrCreateCartAsync();
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
            var cart = await GetOrCreateCartAsync();
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
            var cart = await GetOrCreateCartAsync();
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
            var cart = await GetOrCreateCartAsync();
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

        private async Task<AdminCart> GetOrCreateCartAsync()
        {
            // If user is authenticated, attempt to get user's cart
            if (User?.Identity?.IsAuthenticated == true)
            {
                var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(idClaim, out var userId))
                {
                    var foundUserById = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
                    if (foundUserById != null)
                    {
                        var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserID == foundUserById.UserID);
                        if (cart == null)
                        {
                            cart = new AdminCart { UserID = foundUserById.UserID };
                            _context.Carts.Add(cart);
                            await _context.SaveChangesAsync();
                        }

                        return cart;
                    }
                }

                // fallback: try to match by login name
                var login = User.Identity.Name;
                if (!string.IsNullOrEmpty(login))
                {
                    var foundUserByLogin = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
                    if (foundUserByLogin != null)
                    {
                        var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserID == foundUserByLogin.UserID);
                        if (cart == null)
                        {
                            cart = new AdminCart { UserID = foundUserByLogin.UserID };
                            _context.Carts.Add(cart);
                            await _context.SaveChangesAsync();
                        }

                        return cart;
                    }
                }
            }

            // anonymous / guest flow
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == SiteGuestRole);
            if (role == null)
            {
                role = new AdminRole { RoleName = SiteGuestRole };
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
            }

            var guestUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == SiteGuestLogin);
            if (guestUser == null)
            {
                guestUser = new AdminUser
                {
                    Login = SiteGuestLogin,
                    PasswordHash = "site-managed",
                    RoleID = role.RoleID
                };

                _context.Users.Add(guestUser);
                await _context.SaveChangesAsync();
            }

            var guestCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserID == guestUser.UserID);
            if (guestCart == null)
            {
                guestCart = new AdminCart { UserID = guestUser.UserID };
                _context.Carts.Add(guestCart);
                await _context.SaveChangesAsync();
            }

            return guestCart;
        }
    }
}


