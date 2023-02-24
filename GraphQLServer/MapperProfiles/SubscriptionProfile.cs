using AutoMapper;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class SubscriptionProfile: Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<Subscription, GraphQLDto.Subscription.Subscription_QL>().ReverseMap();
        }

    }
}
