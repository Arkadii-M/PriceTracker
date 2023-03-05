using AutoMapper;
using GraphQLDto;
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
        //public SubscriptionProfile()
        //{
        //    CreateMap<Subscription, SubscriptionQL>().ReverseMap();
        //    CreateMap<SubscriptionQLInput, Subscription>().ReverseMap();
        //    CreateMap<SubscriptionQLPayload, SubscriptionQL>().ReverseMap();
        //    CreateMap<SubscriptionQLPayload, SubscriptionQLInput>().ReverseMap();
        //    CreateMap<SubscriptionQLPayload, Subscription>().ReverseMap();
        //}

    }
}
