using AutoMapper;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class SellerProfile : Profile
    {
        public SellerProfile()
        {
            CreateMap<Seller, GraphQLDto.Seller.Seller_QL>().ReverseMap();
        }
    }
}
