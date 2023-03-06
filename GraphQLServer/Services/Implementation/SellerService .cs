using AutoMapper;
using DTO.GraphQL;
using GraphQLServer.DbModels;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.Services
{
    public class SellerService: ISellerService
    {
        private readonly IMapper _mapper;
        private readonly PriceTrackerContext _dbContext;

        public SellerService(IDbContextFactory<PriceTrackerContext> dbContextFactory, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContextFactory.CreateDbContext();
        }
        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }

        public SellerQLPayload AddSeller(SellerQLInput seller)
        {
            var db_seller = _mapper.Map<Seller>(seller);
            _dbContext.Sellers.Add(db_seller);
            _dbContext.SaveChanges();
            return _mapper.Map<SellerQLPayload>(db_seller);
        }

        public SellerQLPayload GetSellerById(long id)
        {
            return _mapper.Map<SellerQLPayload>(_dbContext.Sellers.FirstOrDefault(s => s.SellerId == id));
        }

        public IQueryable<SellerQLPayload> GetAllSellers()
        {
            return _mapper.ProjectTo<SellerQLPayload>(_dbContext.Sellers.AsQueryable());
        }

        public void RemoveSeller(long id)
        {
            var seller = _dbContext.Sellers.FirstOrDefault(s => s.SellerId == id);
            if (seller != null)
            {
                _dbContext.Sellers.Remove(seller);
                _dbContext.SaveChanges();
            }
        }
    }
}
