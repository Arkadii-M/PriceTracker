using DTO.GraphQL;
using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface IHistoryService
    {
        HistoryQLPayload AddHistory(HistoryQLInput history);
        HistoryQLPayload GetHistoryById(long id);
        IQueryable<HistoryQLPayload> GetAllHistories();
        void RemoveHistory(long id);
    }
}
