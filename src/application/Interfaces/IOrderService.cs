using System.Collections.Generic;
using vlaaienslag.Models;

namespace vlaaienslag.Application.Interfaces
{
    public interface IOrderService
    {
        void RegisterOrder(string buyerId, string sellerId, IDictionary<string, uint> selection);
        void RegisterOrder(OrderRequest request);
    }
}