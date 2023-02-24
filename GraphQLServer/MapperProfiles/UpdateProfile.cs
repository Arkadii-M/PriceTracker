using AutoMapper;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class UpdateProfile : Profile
    {
        public UpdateProfile()
        {
            CreateMap<Update, GraphQLDto.Update.Update_QL>().ReverseMap();
        }
    }
}
