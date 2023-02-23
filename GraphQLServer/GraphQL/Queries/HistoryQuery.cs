using GraphQLDto.History;
using GraphQLServer.DbModels;
using GraphQLServer.Services;

namespace GraphQLServer.GraphQL.Queries
{
    public class HistoryQuery
    {
        private readonly IHistoryService _historyService;

        public HistoryQuery(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        public History_QL GetHistoryById(long id) => _historyService.GetHistoryById(id);

        public IQueryable<History_QL> GetAllHistories() => _historyService.GetAllHistories();
    }
}
