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
            _context.SaveChanges();
        }

        public async Task<IList<Order>> AllAsync()
        {
            return await _context.Order
                .Include(order => order.OrderedItems)
                    .ThenInclude(orderedItem => orderedItem.Product)
                .Include(order => order.Buyer)
                .ToListAsync();
        }

        public async Task<IList<Order>> AllForSellerAsync(string sellerId)
        {
            return await _context.Order
                .Where(order => order.SellerId == sellerId)
                .Include(order => order.OrderedItems)
                    .ThenInclude(orderedItem => orderedItem.Product)
                .Include(order => order.Buyer)
                .ToListAsync();
        }

        public async Task<Order> GetAsync(string orderId)
        {
            return await _context.Order
                .Where(order => order.OrderId == orderId)
                .Include(order => order.OrderedItems)
                   .ThenInclude(OrderedItem => OrderedItem.Product)
                .Include(order => order.Buyer)
                .SingleOrDefaultAsync();
        }

    }
}