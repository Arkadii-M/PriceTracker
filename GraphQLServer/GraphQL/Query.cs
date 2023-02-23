using AutoMapper;
using GraphQLServer.Models;
using GraphQLServer.Services;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQLServer.GraphQL
{
    public class Query
    {
        public readonly IMapper _mapper;

        //private readonly IUserService _userService;

        public Query(IMapper mapper)
        {
            _mapper = mapper;
        }

        //[UseSelection]
        public IQueryable<UserPayLoad> Users(PriceTrackerContext dbContext)
        {
            return _mapper.ProjectTo<UserPayLoad>(dbContext.Users.AsQueryable());
        }
    }
    public record UserPayLoad(long UserId ,string Username);
}
