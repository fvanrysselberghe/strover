using System.Collections.Generic;
using System.Threading.Tasks;
using Strover.Models;

namespace Strover.Application.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Strover.Models.Order order);

        Task<IList<Order>> AllAsync();
        Task<IList<Order>> AllForSellerAsync(string user);

        Task<Order> GetAsync(string orderId);

        Task Remove(string orderId);
    }
}