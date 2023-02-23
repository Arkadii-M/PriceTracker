using GraphQLServer.DbModels;
using GraphQLServer.Services;

namespace GraphQLServer.GraphQL.Queries
{
    public class SubscriptionQuery
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionQuery(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        public Subscription GetSubscriptionById(long id)
        {
            return _subscriptionService.GetSubscriptionById(id);
        }

        public IQueryable<Subscription> GetAllSubscriptions()
        {
            return _subscriptionService.GetAllSubscriptions();
        }
    }
}
