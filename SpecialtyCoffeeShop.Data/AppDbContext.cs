using Microsoft.EntityFrameworkCore;
using SpecialtyCoffeeShop.Data.Entities;

namespace SpecialtyCoffeeShop.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductsOrderDetail> ProductsOrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Products
        modelBuilder.Entity<Product>()
                    .ToTable(t =>
                    {
                        t.HasCheckConstraint(
                            "CK_Product_Price_Positive",
                            $"[{nameof(Product.Price)}] > 0");

                        t.HasCheckConstraint(
                            "CK_Product_CurrentDiscount_Valid",
                            $"[{nameof(Product.CurrentDiscount)}] >= 0 " +
                            $"AND [{nameof(Product.CurrentDiscount)}] <= [{nameof(Product.Price)}]");
                    });

        // ProductOrderDetails
        modelBuilder.Entity<ProductsOrderDetail>()
                    .HasKey(po => new {po.OrderId, po.ProductId});

        modelBuilder.Entity<ProductsOrderDetail>()
                    .HasOne(po => po.Order)
                    .WithMany(o => o.ProductsDetails)
                    .HasForeignKey(po => po.OrderId);

        modelBuilder.Entity<ProductsOrderDetail>()
                    .HasOne(po => po.Product)
                    .WithMany() // no navigation on Product
                    .HasForeignKey(po => po.ProductId);

        modelBuilder.Entity<ProductsOrderDetail>()
                    .ToTable(t =>
                    {
                        t.HasCheckConstraint(
                            "CK_ProductsOrderDetail_Quantity_Positive",
                            $"[{nameof(ProductsOrderDetail.Quantity)}] > 0");
                    });
        
        // Orders
        modelBuilder.Entity<Order>()
                    .ToTable(t =>
                    {
                        t.HasCheckConstraint(
                            "CK_Order_Email_Valid",
                            $"[{nameof(Order.Email)}] LIKE '%@%.%'" +
                            $"OR [{nameof(Order.Email)}] IS NULL");

                        t.HasCheckConstraint(
                            "CK_Order_ContactInfo_Valid",
                            $"[{nameof(Order.Email)}] IS NOT NULL" +
                            $"OR [{nameof(Order.PhoneNumber)}] IS NOT NULL");
                    });
    }
}