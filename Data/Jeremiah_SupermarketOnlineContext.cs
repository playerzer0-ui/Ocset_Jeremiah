using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Jeremiah_SupermarketOnline.Models;

namespace Jeremiah_SupermarketOnline.Data
{
    public class Jeremiah_SupermarketOnlineContext : DbContext
    {
        public Jeremiah_SupermarketOnlineContext (DbContextOptions<Jeremiah_SupermarketOnlineContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
