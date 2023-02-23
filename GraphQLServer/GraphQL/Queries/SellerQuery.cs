using GraphQLServer.DbModels;
using GraphQLServer.Services;

namespace GraphQLServer.GraphQL.Queries
{
    public class SellerQuery
    {
        private readonly ISellerService _sellerService;

        public SellerQuery(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        public Seller GetSellerById(long id)
        {
            return _sellerService.GetSellerById(id);
        }

        public IQueryable<Seller> GetAllSellers()
        {
            return _sellerService.GetAllSellers();
        }
    }
}
