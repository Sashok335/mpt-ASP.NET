using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Practos3.Models;

public class AdminOrderItem
{
    public int OrderItemID { get; set; }
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    [ValidateNever]
    public AdminOrder? Order { get; set; }

    [ValidateNever]
    public AdminProduct? Product { get; set; }
}

