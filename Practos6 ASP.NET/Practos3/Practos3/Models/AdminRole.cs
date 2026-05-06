using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Practos3.Models;

public class AdminRole
{
    public int RoleID { get; set; }
    public string RoleName { get; set; } = null!;

    [ValidateNever]
    public ICollection<AdminUser> Users { get; set; } = new List<AdminUser>();
}

