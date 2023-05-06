using DTO.GraphQL;
using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface ISubscriptionService
    {
        SubscriptionQLPayload AddSubscription(SubscriptionQLInput subscription);
        SubscriptionQLPayload GetSubscriptionById(long id);
        SubscriptionQLPayload UpdateSubscription(SubscriptionQLUpdate subscription);
        IQueryable<SubscriptionQLPayload> GetAllSubscriptions();
        IQueryable<SubscriptionQLPayload> GetAllSubscriptionsForUserId(long id);
        void RemoveSubscription(long id);
    }
}
