using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Strover.Infrastructure.Data;

namespace Strover.Models
{
    public class DataStoreContext : IdentityDbContext<SalesPerson>
    {
        public DataStoreContext(DbContextOptions<DataStoreContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<OrderPayments>().HasKey(joining => new { joining.OrderId, joining.PaymentId });

            // EntityFrameworkCore < 3.0 compatibility)
            builder.Entity<Address>().Property(a => a.ID).ValueGeneratedOnAdd();
            builder.Entity<Customer>().Property(c => c.ID).ValueGeneratedOnAdd();
            builder.Entity<DeliveryMethod>().Property(d => d.DeliveryMethodId).ValueGeneratedOnAdd();
            builder.Entity<Order>().Property(o => o.OrderId).ValueGeneratedOnAdd();
            builder.Entity<OrderedItem>().Property(i => i.OrderedItemId).ValueGeneratedOnAdd();
            builder.Entity<Payment>().Property(p => p.ID).ValueGeneratedOnAdd();
            builder.Entity<Product>().Property(p => p.ProductId).ValueGeneratedOnAdd();
        }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Customer> Buyer { get; set; }
        public DbSet<SalesPersonWrapper> Seller { get; set; }

        public DbSet<Payment> Payment { get; set; }
        public DbSet<OrderPayments> OrderPayment { get; set; }
    }
}