using vlaaienslag.Application.Interfaces;
using vlaaienslag.Models;

namespace vlaaienslag.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataStoreContext _context = null;

        public OrderRepository(DataStoreContext dbContext)
        {
            _context = dbContext;
        }

        public void Add(Order order)
        {
            _context.Order.Add(order);
            _context.SaveChangesAsync();
        }
    }
}