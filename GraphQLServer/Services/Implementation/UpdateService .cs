using AutoMapper;
using GraphQLDto;
using GraphQLServer.DbModels;
using Microsoft.EntityFrameworkCore;
using static GraphQL.Instrumentation.Metrics;

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
        public UpdateQLPayload AddUpdate(UpdateQLInput update)
        {
            _dbContext.Updates.Add(_mapper.Map<Update>(update));
            _dbContext.SaveChanges();
            return _mapper.Map <UpdateQLPayload> (update);
        }

        public UpdateQLPayload GetUpdateById(long id)
        {
            return _mapper.Map<UpdateQLPayload>(_dbContext.Updates.FirstOrDefault(u => u.SubscriptionId == id));
        }

        public IQueryable<UpdateQLPayload> GetAllUpdates()
        {
            return _mapper.ProjectTo<UpdateQLPayload>(_dbContext.Updates.AsQueryable());
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
