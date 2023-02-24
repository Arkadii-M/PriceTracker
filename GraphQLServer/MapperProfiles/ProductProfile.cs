using AutoMapper;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GraphQLDto.Product.Product_QL>().ReverseMap();
        }
    }
}
