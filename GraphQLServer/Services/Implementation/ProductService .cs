using AutoMapper;
using GraphQLDto.Product;
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
        public Product_QL AddProduct(Product_QL product)
        {
            _dbContext.Products.Add(_mapper.Map<GraphQLServer.DbModels.Product>(product));
            _dbContext.SaveChanges();
            return product;
        }

        public Product_QL GetProductById(long id)
        {
            return _mapper.Map<Product_QL>(_dbContext.Products
                .Include(p => p.Seller)
                .FirstOrDefault(p => p.ProductId == id));
        }

        public IQueryable<Product_QL> GetAllProducts()
        {
            return _mapper.Map<IQueryable<Product_QL>>(_dbContext.Products
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
