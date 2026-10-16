using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Practos3.Models;

public class AdminOrder
{
    public int OrderID { get; set; }
    public int UserID { get; set; }
    public DateTime? OrderDate { get; set; }
    public string? Status { get; set; }
    public decimal TotalAmount { get; set; }

    [ValidateNever]
    public AdminUser? User { get; set; }

    [ValidateNever]
    public ICollection<AdminOrderItem> OrderItems { get; set; } = new List<AdminOrderItem>();
}

