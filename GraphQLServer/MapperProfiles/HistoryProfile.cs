using AutoMapper;
using DTO.GraphQL;
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
        }
    }
}
