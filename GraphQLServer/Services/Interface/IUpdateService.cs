using GraphQLDto.Update;
using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface IUpdateService
    {
        Update_QL AddUpdate(Update_QL update);
        Update_QL GetUpdateById(long id);
        IQueryable<Update_QL> GetAllUpdates();
        void RemoveUpdate(long id);
    }
}
