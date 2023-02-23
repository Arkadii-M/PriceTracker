using AutoMapper;
using GraphQLDto.History;
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
        public History_QL AddHistory(History_QL history)
        {
            _dbContext.Histories.Add(_mapper.Map<History>(history));
            _dbContext.SaveChanges();
            return history;
        }

        public History_QL GetHistoryById(long id)
        {
            return _mapper.Map<History_QL>(_dbContext.Histories.FirstOrDefault(h => h.HistoryId == id));
        }

        public IQueryable<History_QL> GetAllHistories()
        {
            return _mapper.ProjectTo<History_QL>(_dbContext.Histories.AsQueryable());
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
    }
}
