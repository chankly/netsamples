using HSoft.NetSamples.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HSoft.NetSamples.Api.Data
{
    public class MyShopDbContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderDetailEntity> OrderDetails { get; set; }

        public MyShopDbContext(DbContextOptions<MyShopDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<ProductEntity>().Property(p => p.Name).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<ProductEntity>().Property(p => p.Stock).IsRequired();

            modelBuilder.Entity<CustomerEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<CustomerEntity>().Property(p => p.Name).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<CustomerEntity>().HasMany(p => p.Orders).WithOne(p => p.Customer).HasForeignKey(p => p.CustomerId);

            modelBuilder.Entity<OrderEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<OrderEntity>().OwnsOne(p => p.Address);
            modelBuilder.Entity<OrderEntity>().HasMany(p => p.OrderDetails).WithOne(p => p.Order).HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<OrderDetailEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<OrderDetailEntity>().HasOne(p => p.Product).WithMany().HasForeignKey(p => p.ProductId);
        }
    }
}
