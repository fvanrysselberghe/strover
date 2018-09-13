using vlaaienslag.Application.Interfaces;
using vlaaienslag.Models;

namespace vlaaienslag.Infrastructure.Data
{
    public class SellerRepository : ISellerRepository
    {
        private readonly DataStoreContext _storage;

        public SellerRepository(DataStoreContext store)
        {
            _storage = store;
        }

        public void Add(SalesPerson seller)
        {
            _storage.Seller.Add(seller);
            _storage.SaveChangesAsync();
        }
    }

}