using AutoMapper;
using GraphQLDto;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class SellerProfile : ProfileCreator
    {
        public SellerProfile() :
            base(typeof(Seller),
                typeof(SellerQL),
                typeof(SellerQLInput),
                typeof(SellerQLPayload))
        {
        }
        //public SellerProfile()
        //{
        //    CreateMap<Seller, SellerQL>().ReverseMap();
        //    CreateMap<SellerQLInput, Seller>().ReverseMap();
        //    CreateMap<SellerQLInput, SellerQL>().ReverseMap();
        //    CreateMap<SellerQLPayload, SellerQL>().ReverseMap();
        //    CreateMap<SellerQLPayload, Seller>().ReverseMap();
        //    CreateMap<SellerQLPayload, SellerQLInput>().ReverseMap();
        //}
    }
}
