namespace WebShopApi.Models;

public class AdminCartItem
{
    public int CartItemID { get; set; }
    public int CartID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }

    public AdminCart? Cart { get; set; }

    public AdminProduct? Product { get; set; }
}

