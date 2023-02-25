using AutoMapper;
using GraphQLDto;
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

        public UserQLPayload CreateUser(UserQLInput user)
        {
            throw new NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }

        public IQueryable<UserQLPayload> GetAll()
        {
            return _mapper.ProjectTo<UserQLPayload>(_dbContext.Users.AsQueryable());
        }

        public bool LoginUser()
        {
            throw new NotImplementedException();
        }
    }
}
