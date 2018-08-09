using vlaaienslag.Application.Interfaces;
using vlaaienslag.Models;
using System.Collections.Generic;

namespace vlaaienslag.Application.Services
{

    public class OrderService : IOrderService
    {
        private IOrderRepository orderRepository = null;

        public OrderService(IOrderRepository repository)
        {
            orderRepository = repository;
        }

        public void RegisterOrder(string buyerId, string sellerId, Dictionary<string, uint> selection)
        {
            var newOrder = new Order();

            newOrder.BuyerId = buyerId;
            newOrder.SellerId = sellerId;

            orderRepository.Add(newOrder);
            //await _context.SaveChangesAsync();
        }
    }
}