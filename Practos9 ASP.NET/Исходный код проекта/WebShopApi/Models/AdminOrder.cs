namespace WebShopApi.Models;

public class AdminOrder
{
    public int OrderID { get; set; }
    public int UserID { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }

    public AdminUser? User { get; set; }

    public ICollection<AdminOrderItem> OrderItems { get; set; } = new List<AdminOrderItem>();
}

