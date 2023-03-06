using AutoMapper;
using DTO.GraphQL;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class SubscriptionProfile: ProfileCreator
    {
        public SubscriptionProfile() :
            base(typeof(Subscription),
                typeof(SubscriptionQL),
                typeof(SubscriptionQLInput),
                typeof(SubscriptionQLPayload),
                typeof(SubscriptionQLUpdate))
        {
        }

    }
}
