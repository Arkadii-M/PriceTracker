using AutoMapper;
using DTO.GraphQL;
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

        public UserQLPayload CreateUser(CreateUserQLInput user)
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

        public UserQLPayload GetUserById(long id)
        {
            return _mapper.Map<UserQLPayload>(_dbContext.Users
                .Include(s => s.Subscriptions)
                .FirstOrDefault(cond => cond.UserId == id));
        }

        public LoginUserQLPayload LoginUser(LoginUserQLInput user_data)
        {
            return new LoginUserQLPayload(user_data.Username, true, "jwt_token");
        }
    }
}
