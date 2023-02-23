using GraphQLDto.History;
using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface IHistoryService
    {
        History_QL AddHistory(History_QL history);
        History_QL GetHistoryById(long id);
        IQueryable<History_QL> GetAllHistories();
        void RemoveHistory(long id);
    }
}
