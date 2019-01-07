using Strover.Models;

namespace Strover.Application.Interfaces
{
    public interface IBuyerRepository
    {
        void Add(Customer buyer);
    }
}