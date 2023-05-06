using AutoMapper;
using DTO.GraphQL;
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

        public SubscriptionQLPayload AddSubscription(SubscriptionQLInput subscription)
        {
            _dbContext.Subscriptions.Add(_mapper.Map<Subscription>(subscription));
            _dbContext.SaveChanges();
            return _mapper.Map<SubscriptionQLPayload>(subscription);
        }

        public SubscriptionQLPayload GetSubscriptionById(long id)
        {
            return _mapper.Map<SubscriptionQLPayload>( _dbContext.Subscriptions
                .Include(s => s.User)
                .Include(s => s.Product)
                    .ThenInclude(p => p.Seller)
                .FirstOrDefault(s => s.SubscriptionId == id));
        }

        public IQueryable<SubscriptionQLPayload> GetAllSubscriptions()
        {
            return _mapper.ProjectTo<SubscriptionQLPayload>(_dbContext.Subscriptions
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

        public SubscriptionQLPayload UpdateSubscription(SubscriptionQLUpdate subscription)
        {
            var sub_db = _dbContext.Subscriptions.FirstOrDefault(sub => sub.SubscriptionId == subscription.SubscriptionId);

            if(sub_db != null)
            {
                sub_db.ProductId = subscription.ProductId;
                sub_db.CheckMinutes = subscription.CheckMinutes;
                _dbContext.SaveChanges();
            }
            return _mapper.Map< SubscriptionQLPayload>(sub_db);
        }

        public IQueryable<SubscriptionQLPayload> GetAllSubscriptionsForUserId(long id)
        {
            return _mapper.ProjectTo<SubscriptionQLPayload>(_dbContext.Subscriptions.Where(s => s.UserId == id)
                .Include(s => s.User)
                .Include(s => s.Product)
                .ThenInclude(p => p.Seller).AsQueryable());
        }
    }
}
