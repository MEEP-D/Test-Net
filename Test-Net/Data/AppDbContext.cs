using Microsoft.EntityFrameworkCore;
using Test_Net.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductProductType> ProductProductTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductType)
            .WithMany(pt => pt.Products)
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.Restrict); 
        base.OnModelCreating(modelBuilder);
    }


}
