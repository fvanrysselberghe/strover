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

        public async Task Remove(string orderId)
        {
            var order = await GetAsync(orderId);

            if (order != null)
            {
                _context.Order.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IList<Order>> AllAsync()
        {
            return await _context.Order
                .Include(order => order.OrderedItems)
                    .ThenInclude(orderedItem => orderedItem.Product)
                .Include(order => order.Buyer)
                .Include(order => order.Delivery)
                    .ThenInclude(delivery => delivery.DeliveryAddress)
                .ToListAsync();
        }

        public async Task<IList<Order>> AllForSellerAsync(string sellerId)
        {
            return await _context.Order
                .Where(order => order.SellerId == sellerId)
                .Include(order => order.OrderedItems)
                    .ThenInclude(orderedItem => orderedItem.Product)
                .Include(order => order.Buyer)
                .Include(order => order.Delivery)
                    .ThenInclude(delivery => delivery.DeliveryAddress)
                .ToListAsync();
        }

        public async Task<Order> GetAsync(string orderId)
        {
            return await _context.Order
                .Where(order => order.OrderId == orderId)
                .Include(order => order.OrderedItems)
                   .ThenInclude(orderedItem => orderedItem.Product)
                .Include(order => order.Buyer)
                .Include(order => order.Delivery)
                    .ThenInclude(delivery => delivery.DeliveryAddress)
                .SingleOrDefaultAsync();
        }

    }
}