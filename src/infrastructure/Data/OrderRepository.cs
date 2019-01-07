using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Strover.Application.Interfaces;
using Strover.Models;

namespace Strover.Infrastructure.Data
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

        public async Task<IList<Order>> GetAsync()
        {
            return await _context.Order
                .Include(order => order.OrderedItems)
                .Include(order => order.Buyer)
                .ToListAsync();
        }

        public async Task<IList<Order>> GetAsync(string sellerId)
        {
            return await _context.Order
                .Where(order => order.SellerId == sellerId)
                .Include(order => order.OrderedItems)
                .Include(order => order.Buyer)
                .ToListAsync();
        }

    }
}