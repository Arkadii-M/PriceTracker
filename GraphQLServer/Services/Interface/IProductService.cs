using DTO.GraphQL;

namespace GraphQLServer.Services
{
    public interface IProductService
    {
        ProductQLPayload AddProduct(ProductQLInput product);
        ProductQLPayload GetProductById(long id);
        ProductQLPayload? GetProductByLink(string link);
        IQueryable<ProductQLPayload> GetAllProducts();
        void RemoveProduct(long id);
    }
}
