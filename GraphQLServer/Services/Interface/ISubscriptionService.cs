using GraphQLDto.Subscription;
using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface ISubscriptionService
    {
        Subscription_QL AddSubscription(Subscription_QL subscription);
        Subscription_QL GetSubscriptionById(long id);
        IQueryable<Subscription_QL> GetAllSubscriptions();
        void RemoveSubscription(long id);
    }
}
