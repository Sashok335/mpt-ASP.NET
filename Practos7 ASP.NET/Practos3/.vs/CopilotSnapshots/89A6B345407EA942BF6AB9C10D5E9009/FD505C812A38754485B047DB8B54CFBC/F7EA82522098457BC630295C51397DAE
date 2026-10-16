using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Practos3.Models;

public class AdminUser
{
    public int UserID { get; set; }
    public string Login { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int RoleID { get; set; }

    [ValidateNever]
    public AdminRole? Role { get; set; }

    [ValidateNever]
    public ICollection<AdminCart> Carts { get; set; } = new List<AdminCart>();

    [ValidateNever]
    public ICollection<AdminOrder> Orders { get; set; } = new List<AdminOrder>();
}

