namespace WebShopApi.Models;

public class AdminOrderItem
{
    public int OrderItemID { get; set; }
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public AdminOrder? Order { get; set; }

    public AdminProduct? Product { get; set; }
}

