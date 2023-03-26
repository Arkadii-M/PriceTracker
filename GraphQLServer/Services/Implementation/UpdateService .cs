using AutoMapper;
using DTO.GraphQL;
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
            //return _mapper.Map<UpdateQLPayload>(_dbContext.Updates.FirstOrDefault(u => u.SubscriptionId == id));
            return _mapper.Map<UpdateQLPayload>(
                _dbContext.Updates
                .Include(h => h.History)
                .Include(s => s.Subscription)
                .FirstOrDefault(up => up.SubscriptionId == id));
        }

        public IQueryable<UpdateQLPayload> GetAllUpdates()
        {
            //var to_ret = _dbContext.Updates.Include(h => h.History).Include(s => s.Subscription);
            //return _mapper.Map<IQueryable<UpdateQLPayload>>(to_ret);
            var updates = _dbContext.Updates.Include(h => h.History).Include(s => s.Subscription).ThenInclude(p => p.Product);

            List<UpdateQLPayload> res = new List<UpdateQLPayload>();
            foreach(var up in updates)
            {
                res.Add(_mapper.Map<UpdateQLPayload>(up));
            }
            return res.AsQueryable();
            
            //return _mapper.Map<IQueryable<UpdateQLPayload>>(to_ret);
        }

        public void RemoveUpdate(long id)
        {
            //var update = _dbContext.Updates.FirstOrDefault(u => u.SubscriptionId == id);
            //if (update != null)
            //{
            //    _dbContext.Updates.Remove(update);
            //    _dbContext.SaveChanges();
            //}
            throw new NotImplementedException();
        }
    }
}
