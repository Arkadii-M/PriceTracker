using AutoMapper;
using GraphQLDto;
using GraphQLServer.DbModels;

namespace GraphQLServer.MapperProfiles
{
    public class HistoryProfile : ProfileCreator
    {
        public HistoryProfile():
            base(typeof(History),
                typeof(HistoryQL),
                typeof(HistoryQLInput),
                typeof(HistoryQLPayload))
        {
            //CreateMap<HistoryQL, History>().ReverseMap();
            //CreateMap<HistoryQLInput, HistoryQL>().ReverseMap();
            //CreateMap<HistoryQLInput, History>().ReverseMap();
            //CreateMap<HistoryQLPayload, HistoryQL>().ReverseMap();
            //CreateMap<HistoryQLPayload, History>().ReverseMap();
            //CreateMap<HistoryQLPayload, HistoryQLInput>().ReverseMap();
            //CreateMap<GraphQLDto.History.History_QL, History>().ForMember(dest => dest.HistoryId,from => from.MapFrom(f => f.HistoryId)).ReverseMap();
            //CreateMap<GraphQLDto.History.History_QL, History>().ForMember(dest => dest.HistoryId, from => from.MapFrom(f => f.HistoryId.Value));
            //CreateMap<History, GraphQLDto.History.History_QL>().ForMember(dest => (long)dest.HistoryId, from => from.MapFrom(f => (Optional<long>)f.HistoryId));
            //CreateMapForTypes(
            //    typeof(History),
            //    typeof(HistoryQL),
            //    typeof(HistoryQLInput),
            //    typeof(HistoryQLPayload));
        }
    }
}
