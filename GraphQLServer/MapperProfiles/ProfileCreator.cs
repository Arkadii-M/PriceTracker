using AutoMapper;

namespace GraphQLServer.MapperProfiles
{
    public class ProfileCreator : Profile
    {
        public ProfileCreator(params Type[] types)
        {
            foreach (var sourceType in types)
            {
                foreach (var destinationType in types.Except(new[] { sourceType }))
                {
                    CreateMap(sourceType, destinationType).ReverseMap();
                }
            }
        }

    }
}
