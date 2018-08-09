using Microsoft.EntityFrameworkCore;

namespace vlaaienslag.Models
{
    class ProductSelectionContext : DbContext
    {
        public ProductSelectionContext(DbContextOptions<ProductSelectionContext> options)
        : base(options)
        { }

        public DbSet<OrderedItem> ProductSelection { get; set; }
    }
}