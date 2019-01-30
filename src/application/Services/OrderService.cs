using Strover.Application.Interfaces;
using Strover.Models;
using System.Collections.Generic;

namespace Strover.Application.Services
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

        public void RegisterOrder(string buyerId, string sellerId, IDictionary<string, uint> selection, DeliveryMethod delivery)
        {
            var newOrder = new Order();

            newOrder.BuyerId = buyerId;
            newOrder.SellerId = sellerId;
            newOrder.Delivery = delivery;

            // #todo_rewrite move to order
            var itemList = new List<OrderedItem>();
            foreach (var item in selection)
            {
                var orderedItem = new OrderedItem();
                orderedItem.ProductId = item.Key;
                orderedItem.Quantity = item.Value;
                itemList.Add(orderedItem);
            }
            newOrder.OrderedItems = itemList;

            orderRepository.Add(newOrder);
        }

        public void RegisterOrder(OrderRequest request)
        {
            // #TODO error handling, e.g. network failure
            buyerRepository.Add(request.Buyer);

            RegisterOrder(request.Buyer.ID, request.Seller.ID, request.Items, request.DeliveryMethod);
        }
    }
}