namespace WebShopApi.Models;

public class AdminRole
{
    public int RoleID { get; set; }
    public string RoleName { get; set; } = null!;

    public ICollection<AdminUser> Users { get; set; } = new List<AdminUser>();
}

