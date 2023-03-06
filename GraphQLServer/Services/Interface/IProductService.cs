using DTO.GraphQL;

namespace GraphQLServer.Services
{
    public interface IProductService
    {
        ProductQLPayload AddProduct(ProductQLInput product);
        ProductQLPayload GetProductById(long id);
        IQueryable<ProductQLPayload> GetAllProducts();
        void RemoveProduct(long id);
    }
}
