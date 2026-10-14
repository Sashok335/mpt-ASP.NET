namespace WebShopApi.Models;

public class AdminCart
{
    public int CartID { get; set; }
    public int UserID { get; set; }

    public AdminUser? User { get; set; }

    public ICollection<AdminCartItem> CartItems { get; set; } = new List<AdminCartItem>();
}

