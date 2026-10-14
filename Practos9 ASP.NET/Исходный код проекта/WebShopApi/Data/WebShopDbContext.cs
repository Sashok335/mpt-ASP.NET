using Microsoft.EntityFrameworkCore;
using WebShopApi.Models;

namespace WebShopApi.Data;

public class WebShopDbContext : DbContext
{
    public WebShopDbContext(DbContextOptions<WebShopDbContext> options)
        : base(options)
    {
    }

    public DbSet<AdminCart> Carts => Set<AdminCart>();
    public DbSet<AdminCartItem> CartItems => Set<AdminCartItem>();
    public DbSet<AdminCategory> Categories => Set<AdminCategory>();
    public DbSet<AdminOrder> Orders => Set<AdminOrder>();
    public DbSet<AdminOrderItem> OrderItems => Set<AdminOrderItem>();
    public DbSet<AdminProduct> Products => Set<AdminProduct>();
    public DbSet<AdminRole> Roles => Set<AdminRole>();
    public DbSet<AdminUser> Users => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminCart>(entity =>
        {
            entity.ToTable("Carts");
            entity.HasKey(e => e.CartID);
            entity.Property(e => e.CartID).ValueGeneratedOnAdd();

            entity.HasOne(d => d.User)
                .WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserID);
        });

        modelBuilder.Entity<AdminCartItem>(entity =>
        {
            entity.ToTable("CartItems");
            entity.HasKey(e => e.CartItemID);
            entity.Property(e => e.CartItemID).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Cart)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartID);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductID);
        });

        modelBuilder.Entity<AdminCategory>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(e => e.CategoryID);
            entity.Property(e => e.CategoryID).ValueGeneratedOnAdd();
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<AdminOrder>(entity =>
        {
            entity.ToTable("Orders");
            entity.HasKey(e => e.OrderID);
            entity.Property(e => e.OrderID).ValueGeneratedOnAdd();
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserID);
        });

        modelBuilder.Entity<AdminOrderItem>(entity =>
        {
            entity.ToTable("OrderItems");
            entity.HasKey(e => e.OrderItemID);
            entity.Property(e => e.OrderItemID).ValueGeneratedOnAdd();
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderID);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductID);
        });

        modelBuilder.Entity<AdminProduct>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.ProductID);
            entity.Property(e => e.ProductID).ValueGeneratedOnAdd();
            entity.Property(e => e.ProductName).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryID);
        });

        modelBuilder.Entity<AdminRole>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(e => e.RoleID);
            entity.Property(e => e.RoleID).ValueGeneratedOnAdd();
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.UserID);
            entity.Property(e => e.UserID).ValueGeneratedOnAdd();
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleID);
        });
    }
}

