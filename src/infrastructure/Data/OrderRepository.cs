using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IList<Order>> GetAsync() => await _context.Order.ToListAsync();

        public async Task<IList<Order>> GetAsync(string sellerId) => await _context.Order.Where(order => order.SellerId == sellerId).ToListAsync();
    }
}