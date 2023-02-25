using AutoMapper;
using GraphQLDto;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductQL>().ReverseMap();
            CreateMap<ProductQLInput, ProductQL>().ReverseMap();
            CreateMap<ProductQLInput, Product>().ReverseMap();
            CreateMap<ProductQLPayload, ProductQL>().ReverseMap();
            CreateMap<ProductQLPayload, Product>().ReverseMap();
        }
    }
}
