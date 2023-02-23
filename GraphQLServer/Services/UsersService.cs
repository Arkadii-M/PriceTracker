using GraphQLServer.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.Services
{
    public class UsersService : IUserService, IAsyncDisposable
    {
        private readonly PriceTrackerContext _dbContext;

        public UsersService(IDbContextFactory<PriceTrackerContext> databaseContext)
        {
            _dbContext = databaseContext.CreateDbContext();
        }

        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }

        public IQueryable<User> GetAll()
        {
            return _dbContext.Users.AsQueryable();
        }
    }
}
