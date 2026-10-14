namespace WebShopApi.Models;

public class AdminProduct
{
    public int ProductID { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public int CategoryID { get; set; }

    public AdminCategory? Category { get; set; }

    public ICollection<AdminCartItem> CartItems { get; set; } = new List<AdminCartItem>();

    public ICollection<AdminOrderItem> OrderItems { get; set; } = new List<AdminOrderItem>();
}
