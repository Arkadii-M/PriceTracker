using GraphQLDto;
using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface IUserService
    {
        bool LoginUser();
        UserQLPayload CreateUser(UserQLInput user);
        IQueryable<UserQLPayload> GetAll();
    }
}
