using GraphQLDto.User;
using GraphQLServer.Services;

namespace GraphQLServer.GraphQL.Queries
{
    public class UserQuery
    {
        private readonly IUserService _userService;

        public UserQuery(IUserService userService)
        {
            _userService = userService;
        }

        //public GraphQLDto.User.UserPayload GetUserById(long id)
        //{
        //    return _userService(id);
        //}
        public IQueryable<UserPayload_QL> GetAllUsers(UserService usersService) => _userService.GetAll();
    }

}
