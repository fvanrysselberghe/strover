using Microsoft.EntityFrameworkCore;

namespace vlaaienslag.Models
{
    public class DataStoreContext : DbContext
    {
        public DataStoreContext(DbContextOptions<DataStoreContext> options)
        : base(options)
        { }

        public DbSet<Order> Order { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}