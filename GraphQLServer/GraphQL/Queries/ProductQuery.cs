using GraphQLDto.Product;
using GraphQLServer.DbModels;
using GraphQLServer.Services;

namespace GraphQLServer.GraphQL.Queries
{
    public class ProductQuery
    {
        private readonly IProductService _productService;

        public ProductQuery(IProductService productService)
        {
            _productService = productService;
        }

        public Product_QL GetProductById(long id) => _productService.GetProductById(id);

        public IQueryable<Product_QL> GetAllProducts() => _productService.GetAllProducts();
    }
}
