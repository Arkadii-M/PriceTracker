using AutoMapper;
using DTO.GraphQL;
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
    }
}
