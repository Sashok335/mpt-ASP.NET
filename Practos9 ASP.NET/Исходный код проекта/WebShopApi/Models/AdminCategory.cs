namespace WebShopApi.Models;

public class AdminCategory
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = null!;

    public ICollection<AdminProduct> Products { get; set; } = new List<AdminProduct>();
}

