using Strover.Models;

namespace Strover.Application.Interfaces
{
    public interface ISellerRepository
    {
        void Add(SalesPersonWrapper seller);
    }
}