using AutoMapper;
using DTO.GraphQL;
using GraphQLServer.DbModels;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IMapper _mapper;
        private readonly PriceTrackerContext _dbContext;

        public HistoryService(IDbContextFactory<PriceTrackerContext> dbContextFactory, IMapper mapper)
        {
            _dbContext = dbContextFactory.CreateDbContext();
            _mapper = mapper;
        }
        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }
        public HistoryQLPayload AddHistory(HistoryQLInput history)
        {
            //var history_ = _mapper.Map<History>(history);
            //_dbContext.Histories.Add(_mapper.Map<History>(history));
            //_dbContext.SaveChanges();
            //return history;
            var history_ = _mapper.Map<History>(history);
            _dbContext.Histories.Add(history_);
            _dbContext.SaveChanges();
            return _mapper.Map<HistoryQLPayload>(history_);
        }

        public HistoryQLPayload GetHistoryById(long id)
        {
            return _mapper.Map<HistoryQLPayload>(_dbContext.Histories.FirstOrDefault(h => h.HistoryId == id));
        }

        public IQueryable<HistoryQLPayload> GetAllHistories()
        {
            return _mapper.ProjectTo<HistoryQLPayload>(_dbContext.Histories.AsQueryable());
        }

        public void RemoveHistory(long id)
        {
            var history = _dbContext.Histories.FirstOrDefault(h => h.HistoryId == id);
            if (history != null)
            {
                _dbContext.Histories.Remove(history);
                _dbContext.SaveChanges();
            }
        }

        public IQueryable<HistoryQLPayload> GetAllHistoryForProductId(long id)
        {
            return _mapper.ProjectTo<HistoryQLPayload>(_dbContext.Histories.Where(h => h.ProductId == id).AsQueryable());
        }
    }
}
