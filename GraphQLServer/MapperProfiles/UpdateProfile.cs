using AutoMapper;
using GraphQLDto;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class UpdateProfile : Profile
    {
        public UpdateProfile()
        {
            CreateMap<Update, UpdateQL>().ReverseMap();
            CreateMap<UpdateQLInput, UpdateQL>().ReverseMap();
            CreateMap<UpdateQLPayload, UpdateQL>().ReverseMap();
            CreateMap<UpdateQLPayload, Update>().ReverseMap();
            CreateMap<UpdateQLPayload, UpdateQLInput>().ReverseMap();
        }
    }
}
