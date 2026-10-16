using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Practos3.Models;

public class AdminCart
{
    public int CartID { get; set; }
    public int UserID { get; set; }

    public AdminUser? User { get; set; }

    public ICollection<AdminCartItem> CartItems { get; set; } = new List<AdminCartItem>();
}

