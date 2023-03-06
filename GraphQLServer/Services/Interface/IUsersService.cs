using DTO.GraphQL;
using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface IUserService
    {
        LoginUserQLPayload LoginUser(LoginUserQLInput user_data);
        UserQLPayload GetUserById(long id);
        UserQLPayload CreateUser(CreateUserQLInput user);
        IQueryable<UserQLPayload> GetAll();
    }
}
