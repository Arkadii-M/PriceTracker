using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface ISellerService
    {
        Seller AddSeller(Seller seller);
        Seller GetSellerById(long id);
        IQueryable<Seller> GetAllSellers();
        void RemoveSeller(long id);
    }
}
