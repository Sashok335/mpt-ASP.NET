using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Practos3.Models;

public class AdminCartItem
{
    public int CartItemID { get; set; }
    public int CartID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }

    [ValidateNever]
    public AdminCart? Cart { get; set; }

    [ValidateNever]
    public AdminProduct? Product { get; set; }
}

