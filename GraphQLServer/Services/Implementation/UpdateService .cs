using AutoMapper;
using GraphQLDto.History;
using GraphQLDto.Update;
using GraphQLServer.DbModels;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IMapper _mapper;
        private readonly PriceTrackerContext _dbContext;

        public UpdateService(IDbContextFactory<PriceTrackerContext> dbContextFactory, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContextFactory.CreateDbContext();
        }
        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }
        public Update_QL AddUpdate(Update_QL update)
        {
            _dbContext.Updates.Add(_mapper.Map<Update>(update));
            _dbContext.SaveChanges();
            return update;
        }

        public Update_QL GetUpdateById(long id)
        {
            return _mapper.Map<Update_QL>(_dbContext.Updates.FirstOrDefault(u => u.SubscriptionId == id));
        }

        public IQueryable<Update_QL> GetAllUpdates()
        {
            return _mapper.ProjectTo<Update_QL>(_dbContext.Updates.AsQueryable());
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
