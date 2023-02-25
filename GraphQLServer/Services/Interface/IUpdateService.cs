using GraphQLDto;
using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface IUpdateService
    {
        UpdateQLPayload AddUpdate(UpdateQLInput update);
        UpdateQLPayload GetUpdateById(long id);
        IQueryable<UpdateQLPayload> GetAllUpdates();
        void RemoveUpdate(long id);
    }
}
