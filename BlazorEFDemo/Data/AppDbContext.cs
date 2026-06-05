using BlazorEFDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorEFDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("categories");
                e.HasKey(c => c.Id);
                e.Property(c => c.Id).UseIdentityByDefaultColumn();
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);
                e.HasIndex(c => c.Name).IsUnique();
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("products");
                e.HasKey(p => p.Id);
                e.Property(p => p.Id).UseIdentityByDefaultColumn();
                e.Property(p => p.Name).IsRequired().HasMaxLength(200);
                e.Property(p => p.Price).HasColumnType("decimal(18,2)");
                e.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
                e.HasOne(p => p.Category)
                 .WithMany(c => c.Products)
                 .HasForeignKey(p => p.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Gadgets" },
                new Category { Id = 2, Name = "Clothing", Description = "Apparel" },
                new Category { Id = 3, Name = "Food", Description = "Groceries" }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Price = 85000,
                    Stock = 10,
                    CategoryId = 1,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 2,
                    Name = "T-Shirt",
                    Price = 450,
                    Stock = 100,
                    CategoryId = 2,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 3,
                    Name = "Rice 5kg",
                    Price = 600,
                    Stock = 200,
                    CategoryId = 3,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
