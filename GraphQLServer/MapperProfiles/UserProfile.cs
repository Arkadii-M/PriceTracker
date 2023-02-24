using AutoMapper;
using GraphQLDto.User;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GraphQLDto.User.UserPayload_QL>();
            CreateMap<GraphQLDto.User.UserInput_QL, User>();
            CreateMap<User_QL, User>().ReverseMap();
        }
    }
}
