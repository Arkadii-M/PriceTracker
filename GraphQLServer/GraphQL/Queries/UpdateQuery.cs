using GraphQLServer.DbModels;
using GraphQLServer.Services;

namespace GraphQLServer.GraphQL.Queries
{

    public class UpdateQuery
    {
        private readonly IUpdateService _updateService;

        public UpdateQuery(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        public Update GetUpdateById(long id)
        {
            return _updateService.GetUpdateById(id);
        }

        public IQueryable<Update> GetAllUpdates()
        {
            return _updateService.GetAllUpdates();
        }
    }
}
