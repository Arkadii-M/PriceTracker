using GraphQLServer.DbModels;
using Microsoft.EntityFrameworkCore;
using static HotChocolate.ErrorCodes;

namespace GraphQLServer.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly PriceTrackerContext _dbContext;

        public SubscriptionService(IDbContextFactory<PriceTrackerContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }
        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }

        public Subscription AddSubscription(Subscription subscription)
        {
            _dbContext.Subscriptions.Add(subscription);
            _dbContext.SaveChanges();
            return subscription;
        }

        public Subscription GetSubscriptionById(long id)
        {
            return _dbContext.Subscriptions
                .Include(s => s.User)
                .Include(s => s.Product)
                    .ThenInclude(p => p.Seller)
                .FirstOrDefault(s => s.SubscriptionId == id);
        }

        public IQueryable<Subscription> GetAllSubscriptions()
        {
            return _dbContext.Subscriptions
                .Include(s => s.User)
                .Include(s => s.Product)
                    .ThenInclude(p => p.Seller)
                .AsQueryable();
        }

        public void RemoveSubscription(long id)
        {
            var subscription = _dbContext.Subscriptions.FirstOrDefault(s => s.SubscriptionId == id);
            if (subscription != null)
            {
                _dbContext.Subscriptions.Remove(subscription);
                _dbContext.SaveChanges();
            }
        }



    }
}
