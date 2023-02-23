using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface ISubscriptionService
    {
        Subscription AddSubscription(Subscription subscription);
        Subscription GetSubscriptionById(long id);
        IQueryable<Subscription> GetAllSubscriptions();
        void RemoveSubscription(long id);
    }
}
