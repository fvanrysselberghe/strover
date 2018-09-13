using vlaaienslag.Application.Interfaces;
using vlaaienslag.Models;
using System.Collections.Generic;

namespace vlaaienslag.Application.Services
{

    public class OrderService : IOrderService
    {
        private IOrderRepository orderRepository = null;
        private IBuyerRepository buyerRepository = null;
        private ISellerRepository sellerRepository = null;

        public OrderService(IOrderRepository orders, IBuyerRepository buyers, ISellerRepository sellers)
        {
            orderRepository = orders;
            buyerRepository = buyers;
            sellerRepository = sellers;
        }

        public void RegisterOrder(string buyerId, string sellerId, IDictionary<string, uint> selection)
        {
            var newOrder = new Order();

            newOrder.BuyerId = buyerId;
            newOrder.SellerId = sellerId;

            orderRepository.Add(newOrder);
        }

        public void RegisterOrder(OrderRequest request)
        {
            buyerRepository.Add(request.Buyer);

            sellerRepository.Add(request.Seller);

            RegisterOrder(request.Buyer.ID, request.Seller.ID, request.Items);

        }
    }
}