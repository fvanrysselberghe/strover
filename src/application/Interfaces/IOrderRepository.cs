using System.Collections.Generic;
using System.Threading.Tasks;
using Strover.Models;

namespace Strover.Application.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Strover.Models.Order order);

        Task<IList<Order>> GetAsync();
        Task<IList<Order>> GetAsync(string user);
    }
}