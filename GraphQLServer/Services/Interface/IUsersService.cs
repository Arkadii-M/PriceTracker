using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface IUserService
    {
        bool LoginUser();
        GraphQLDto.User.UserPayload_QL CreateUser(GraphQLDto.User.UserInput_QL user);
        IQueryable<GraphQLDto.User.UserPayload_QL> GetAll();
    }
}
