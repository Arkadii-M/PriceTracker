using AutoMapper;
using GraphQLDto.Seller;
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

        public Seller_QL AddSeller(Seller_QL seller)
        {
            _dbContext.Sellers.Add(_mapper.Map<Seller>(seller));
            _dbContext.SaveChanges();
            return seller;
        }

        public Seller_QL GetSellerById(long id)
        {
            return _mapper.Map<Seller_QL>(_dbContext.Sellers.FirstOrDefault(s => s.SellerId == id));
        }

        public IQueryable<Seller_QL> GetAllSellers()
        {
            return _mapper.ProjectTo<Seller_QL>(_dbContext.Sellers.AsQueryable());
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
