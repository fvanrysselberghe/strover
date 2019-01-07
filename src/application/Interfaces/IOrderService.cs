using System.Collections.Generic;
using Strover.Models;

namespace Strover.Application.Interfaces
{
    public interface IOrderService
    {
        void RegisterOrder(string buyerId, string sellerId, IDictionary<string, uint> selection);
        void RegisterOrder(OrderRequest request);
    }
}