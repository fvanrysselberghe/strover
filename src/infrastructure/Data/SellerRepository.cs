using Strover.Application.Interfaces;
using Strover.Models;

namespace Strover.Infrastructure.Data
{
    public class SellerRepository : ISellerRepository
    {
        private readonly DataStoreContext _storage;

        public SellerRepository(DataStoreContext store)
        {
            _storage = store;
        }

        public void Add(SalesPersonWrapper seller)
        {
            _storage.Seller.Add(seller);
            _storage.SaveChangesAsync();
        }
    }

}