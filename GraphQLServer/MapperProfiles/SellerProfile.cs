using AutoMapper;
using GraphQLDto;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class SellerProfile : Profile
    {
        public SellerProfile()
        {
            CreateMap<Seller, SellerQL>().ReverseMap();
            CreateMap<SellerQLInput, Seller>().ReverseMap();
            CreateMap<SellerQLInput, SellerQL>().ReverseMap();
            CreateMap<SellerQLPayload, SellerQL>().ReverseMap();
            CreateMap<SellerQLPayload, Seller>().ReverseMap();
            CreateMap<SellerQLPayload, SellerQLInput>().ReverseMap();
        }
    }
}
