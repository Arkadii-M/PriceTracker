using AutoMapper;
using GraphQLDto;
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
        //public UpdateProfile()
        //{
        //    CreateMap<Update, UpdateQL>().ReverseMap();
        //    CreateMap<UpdateQLInput, UpdateQL>().ReverseMap();
        //    CreateMap<UpdateQLPayload, UpdateQL>().ReverseMap();
        //    CreateMap<UpdateQLPayload, Update>().ReverseMap();
        //    CreateMap<UpdateQLPayload, UpdateQLInput>().ReverseMap();
        //}
    }
}
