using System.Collections.Generic;
using Strover.Models;

namespace Strover.Application.Interfaces
{
    public interface IOrderService
    {
        void RegisterOrder(OrderRequest request);
    }
}