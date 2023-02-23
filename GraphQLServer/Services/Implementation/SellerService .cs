using GraphQLServer.DbModels;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.Services
{
    public class SellerService
    {
        private readonly PriceTrackerContext _dbContext;

        public SellerService(IDbContextFactory<PriceTrackerContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }
        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }

        public Seller AddSeller(Seller seller)
        {
            _dbContext.Sellers.Add(seller);
            _dbContext.SaveChanges();
            return seller;
        }

        public Seller GetSellerById(long id)
        {
            return _dbContext.Sellers.FirstOrDefault(s => s.SellerId == id);
        }

        public IQueryable<Seller> GetAllSellers()
        {
            return _dbContext.Sellers.AsQueryable();
        }

        public void RemoveSeller(long id)
        {
            var seller = _dbContext.Sellers.FirstOrDefault(s => s.SellerId == id);
            if (seller != null)
            {
                _dbContext.Sellers.Remove(seller);
                _dbContext.SaveChanges();
            }
        }
    }
}
