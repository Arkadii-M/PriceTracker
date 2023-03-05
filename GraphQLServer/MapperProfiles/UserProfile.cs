using AutoMapper;
using GraphQLDto;
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
        //public UserProfile()
        //{
        //    CreateMap<User, UserQL>().ReverseMap();
        //    CreateMap<UserQLInput, User>().ReverseMap();
        //    CreateMap<UserQLPayload, UserQL>().ReverseMap();
        //    CreateMap<UserQLPayload, UserQLInput>().ReverseMap();
        //    CreateMap<UserQLPayload, User>().ReverseMap();
        //}
    }
}
