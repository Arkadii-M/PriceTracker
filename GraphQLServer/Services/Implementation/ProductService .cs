using AutoMapper;
using GraphQLDto;
using Microsoft.EntityFrameworkCore;

namespace GraphQLServer.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly GraphQLServer.DbModels.PriceTrackerContext _dbContext;

        public ProductService(IDbContextFactory<GraphQLServer.DbModels.PriceTrackerContext> dbContextFactory,IMapper mapper)
        {
            _dbContext = dbContextFactory.CreateDbContext();
            _mapper = mapper;
        }
        public ValueTask DisposeAsync()
        {
            return ((IAsyncDisposable)_dbContext).DisposeAsync();
        }
        public ProductQLPayload AddProduct(ProductQLInput product)
        {
            var db_product = _mapper.Map<GraphQLServer.DbModels.Product>(product);
            _dbContext.Products.Add(db_product);
            _dbContext.SaveChanges();
            return _mapper.Map<ProductQLPayload>(db_product);
        }

        public ProductQLPayload GetProductById(long id)
        {
            return _mapper.Map<ProductQLPayload>(_dbContext.Products
                .Include(p => p.Seller)
                .FirstOrDefault(p => p.ProductId == id));
        }

        public IQueryable<ProductQLPayload> GetAllProducts()
        {
            return _mapper.ProjectTo<ProductQLPayload>(
                _dbContext.Products
                .Include(p => p.Seller)
                .AsQueryable());
        }

        public void RemoveProduct(long id)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
            }
        }
    }
}
