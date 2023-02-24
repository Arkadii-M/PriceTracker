using AutoMapper;
using GraphQLDto.History;
using GraphQLDto.Subscription;
using GraphQLServer.DbModels;
using Microsoft.EntityFrameworkCore;
using static HotChocolate.ErrorCodes;

namespace GraphQLServer.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IMapper _mapper;
        private readonly PriceTrackerContext _dbContext;

        public SubscriptionService(IDbContextFactory<PriceTrackerContext> dbContextFactory, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContextFactory.CreateDbContext();
        }
        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }

        public Subscription_QL AddSubscription(Subscription_QL subscription)
        {
            _dbContext.Subscriptions.Add(_mapper.Map<Subscription>(subscription));
            _dbContext.SaveChanges();
            return subscription;
        }

        public Subscription_QL GetSubscriptionById(long id)
        {
            return _mapper.Map< Subscription_QL >( _dbContext.Subscriptions
                .Include(s => s.User)
                .Include(s => s.Product)
                    .ThenInclude(p => p.Seller)
                .FirstOrDefault(s => s.SubscriptionId == id));
        }

        public IQueryable<Subscription_QL> GetAllSubscriptions()
        {
            return _mapper.ProjectTo<Subscription_QL>(_dbContext.Subscriptions
                .Include(s => s.User)
                .Include(s => s.Product)
                    .ThenInclude(p => p.Seller)
                .AsQueryable());
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
