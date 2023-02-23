using AutoMapper;
using GraphQLServer.DbModels;
using GraphQLServer.Services;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQLServer.GraphQL
{
    public class Query
    {
        private readonly IUserService _userService;

        public Query(IUserService userService)
        {
            _userService = userService;
        }
        //[UseSelection]
        public IQueryable<GraphQLDto.User.UserPayload_QL> Users(UserService usersService) => _userService.GetAll();
    }
}
