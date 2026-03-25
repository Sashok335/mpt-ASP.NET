using System;
using System.Collections.Generic;

namespace Practos3.Models;

public partial class CartItem
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public string CartId { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
