using GraphQLDto;
using GraphQLDto.Product;

namespace GraphQLServer.Services
{
    public interface IProductService
    {
        Product_QL AddProduct(Product_QL product);
        Product_QL GetProductById(long id);
        IQueryable<Product_QL> GetAllProducts();
        void RemoveProduct(long id);
    }
}
