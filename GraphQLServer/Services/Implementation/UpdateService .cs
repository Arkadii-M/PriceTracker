using GraphQLServer.DbModels;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly PriceTrackerContext _dbContext;

        public UpdateService(IDbContextFactory<PriceTrackerContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }
        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }
        public Update AddUpdate(Update update)
        {
            _dbContext.Updates.Add(update);
            _dbContext.SaveChanges();
            return update;
        }

        public Update GetUpdateById(long id)
        {
            return _dbContext.Updates.FirstOrDefault(u => u.SubscriptionId == id);
        }

        public IQueryable<Update> GetAllUpdates()
        {
            return _dbContext.Updates.AsQueryable();
        }

        public void RemoveUpdate(long id)
        {
            var update = _dbContext.Updates.FirstOrDefault(u => u.SubscriptionId == id);
            if (update != null)
            {
                _dbContext.Updates.Remove(update);
                _dbContext.SaveChanges();
            }
        }
    }
}
