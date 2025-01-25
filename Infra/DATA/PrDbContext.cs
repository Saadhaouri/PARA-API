using Core.Application.Dto_s;
using Domaine.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.DATA
{
    public class PrDbContext : IdentityDbContext<User>
    {
        public PrDbContext(DbContextOptions<PrDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<ProductPromotion> ProductPromotions { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<DebtProduct> DebtProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().OwnsOne(z => z.Address, navigation =>
            {
                navigation.ToJson();
            });

            modelBuilder.Entity<Client>().OwnsOne(z => z.Address, navigation =>
            {
                navigation.ToJson();
            });
            // Configuring OrderProduct entity
            modelBuilder.Entity<OrderProduct>()
               .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade);  // Or DeleteBehavior.Restrict if you don't want cascading

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent deletion of product if it's in an order

            // Configuring ProductPromotion entity
            modelBuilder.Entity<ProductPromotion>()
               .HasKey(pp => new { pp.ProductId, pp.PromotionId });

            modelBuilder.Entity<ProductPromotion>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.ProductPromotions)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent deletion of product if it's in a promotion

            modelBuilder.Entity<ProductPromotion>()
                .HasOne(pp => pp.Promotion)
                .WithMany(p => p.ProductPromotions)
                .HasForeignKey(pp => pp.PromotionId)
                .OnDelete(DeleteBehavior.Restrict);  // Or use Cascade if you want to delete promotions too when product is deleted

            modelBuilder.Entity<DebtProduct>()
                .HasKey(dp => new { dp.DebtId, dp.ProductId });

            modelBuilder.Entity<DebtProduct>()
                .HasOne(dp => dp.Debt)
                .WithMany(d => d.DebtProducts)
                .HasForeignKey(dp => dp.DebtId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent debt if product exists

            modelBuilder.Entity<DebtProduct>()
                .HasOne(dp => dp.Product)
                .WithMany(p => p.DebtProducts)
                .HasForeignKey(dp => dp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);  
        }
    }
}
