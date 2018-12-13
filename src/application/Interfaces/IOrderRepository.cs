using System.Collections.Generic;
using System.Threading.Tasks;
using vlaaienslag.Models;

namespace vlaaienslag.Application.Interfaces
{
    public interface IOrderRepository
    {
        void Add(vlaaienslag.Models.Order order);

        Task<IList<Order>> GetAsync();
        Task<IList<Order>> GetAsync(string user);
    }
}