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
        }
        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Customer> Buyer { get; set; }
        public DbSet<SalesPersonWrapper> Seller { get; set; }
    }
}