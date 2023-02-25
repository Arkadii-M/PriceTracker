using AutoMapper;
using GraphQLDto;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserQL>().ReverseMap();
            CreateMap<UserQLInput, User>().ReverseMap();
            CreateMap<UserQLPayload, UserQL>().ReverseMap();
            CreateMap<UserQLPayload, UserQLInput>().ReverseMap();
            CreateMap<UserQLPayload, User>().ReverseMap();
        }
    }
}
