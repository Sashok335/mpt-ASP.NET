using System.ComponentModel.DataAnnotations.Schema;

namespace WebShopApi.Models;

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

    public AdminRole? Role { get; set; }

    public ICollection<AdminCart> Carts { get; set; } = new List<AdminCart>();

    public ICollection<AdminOrder> Orders { get; set; } = new List<AdminOrder>();
}


