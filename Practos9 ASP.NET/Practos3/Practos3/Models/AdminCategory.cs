using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Practos3.Models;

public class AdminCategory
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = null!;

    [ValidateNever]
    public ICollection<AdminProduct> Products { get; set; } = new List<AdminProduct>();
}

