using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practos3.Models;

public class AdminUser
{
    public int UserID { get; set; }
    public string Login { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int RoleID { get; set; }

    // Display name for the user (e.g. full name)
    [NotMapped]
    public string? DisplayName { get; set; }

    // Whether account is active
    [NotMapped]
    public bool IsActive { get; set; } = true;

    [ValidateNever]
    public AdminRole? Role { get; set; }

    [ValidateNever]
    public ICollection<AdminCart> Carts { get; set; } = new List<AdminCart>();

    [ValidateNever]
    public ICollection<AdminOrder> Orders { get; set; } = new List<AdminOrder>();
}


