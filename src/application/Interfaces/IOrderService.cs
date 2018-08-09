using System.Collections.Generic;

namespace vlaaienslag.Application.Interfaces
{
    public interface IOrderService
    {
        void RegisterOrder(string buyerId, string sellerId, Dictionary<string, uint> selection);
    }
}