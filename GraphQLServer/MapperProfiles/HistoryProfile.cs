using AutoMapper;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class HistoryProfile :Profile
    {
        public HistoryProfile()
        {
            CreateMap<History, GraphQLDto.History.History_QL>().ReverseMap();
        }
    }
}
