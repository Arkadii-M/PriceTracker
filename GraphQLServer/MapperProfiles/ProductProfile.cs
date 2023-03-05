using AutoMapper;
using GraphQLDto;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class ProductProfile : ProfileCreator
    {
        public ProductProfile():
            base(typeof(Product),
                typeof(ProductQL),
                typeof(ProductQLInput),
                typeof(ProductQLPayload))
        {
        }
        //public ProductProfile()
        //{
        //    CreateMap<Product, ProductQL>().ReverseMap();
        //    CreateMap<ProductQLInput, ProductQL>().ReverseMap();
        //    CreateMap<ProductQLInput, Product>().ReverseMap();
        //    CreateMap<ProductQLPayload, ProductQL>().ReverseMap();
        //    CreateMap<ProductQLPayload, Product>().ReverseMap();
        //}
    }
}
