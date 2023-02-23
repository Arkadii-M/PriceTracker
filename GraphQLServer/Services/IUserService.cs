using GraphQLServer.Models;

namespace GraphQLServer.Services
{
    public interface IUserService
    {
        IQueryable<User> GetAll();
    }
}
