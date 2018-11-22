using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace vlaaienslag.Models
{
    public class DataStoreContext : IdentityDbContext<IdentityUser>
    {
        public DataStoreContext(DbContextOptions<DataStoreContext> options)
        : base(options)
        { }

        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Customer> Buyer { get; set; }
        public DbSet<SalesPerson> Seller { get; set; }
    }
}