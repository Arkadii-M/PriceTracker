using AutoMapper;
using DTO.GraphQL;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class UserProfile : ProfileCreator
    {
        public UserProfile() :
            base(typeof(User),
                typeof(UserQL),
                typeof(UserQLInput),
                typeof(UserQLPayload),
                typeof(LoginUserQLInput),
                typeof(LoginUserQLPayload),
                typeof(CreateUserQLInput))
        {
        }
    }
}
