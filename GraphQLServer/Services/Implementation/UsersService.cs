using AutoMapper;
using GraphQLDto;
using GraphQLDto.User;
using GraphQLServer.DbModels;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.Services
{
    public class UserService : IUserService, IAsyncDisposable
    {
        private readonly IMapper _mapper;
        private readonly PriceTrackerContext _dbContext;

        public UserService(IDbContextFactory<PriceTrackerContext> dbContextFactory, IMapper mapper)
        {
            _dbContext = dbContextFactory.CreateDbContext();
            _mapper = mapper;
        }

        public UserPayload_QL CreateUser(UserInput_QL user)
        {
            throw new NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }

        public IQueryable<GraphQLDto.User.UserPayload_QL> GetAll()
        {
            return _mapper.ProjectTo<GraphQLDto.User.UserPayload_QL>(_dbContext.Users.AsQueryable());
        }

        public bool LoginUser()
        {
            throw new NotImplementedException();
        }
    }
}
