using AutoMapper;
using DTO.GraphQL;
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
    }
}
