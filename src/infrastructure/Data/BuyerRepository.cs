using Strover.Application.Interfaces;
using Strover.Models;

namespace Strover.Infrastructure.Data
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly DataStoreContext _storage;

        public BuyerRepository(DataStoreContext dbStore)
        {
            _storage = dbStore;
        }

        public void Add(Customer buyer)
        {
            _storage.Buyer.Add(buyer);
            _storage.SaveChangesAsync();
        }
    }
}