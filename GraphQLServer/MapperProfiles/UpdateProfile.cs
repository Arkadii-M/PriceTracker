using AutoMapper;
using DTO.GraphQL;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class UpdateProfile : ProfileCreator
    {
        public UpdateProfile() :
            base(typeof(Update),
                typeof(UpdateQL),
                typeof(UpdateQLInput),
                typeof(UpdateQLPayload))
        {
        }
    }
}
