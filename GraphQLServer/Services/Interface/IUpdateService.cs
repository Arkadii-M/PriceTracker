using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface IUpdateService
    {
        Update AddUpdate(Update update);
        Update GetUpdateById(long id);
        IQueryable<Update> GetAllUpdates();
        void RemoveUpdate(long id);
    }
}
