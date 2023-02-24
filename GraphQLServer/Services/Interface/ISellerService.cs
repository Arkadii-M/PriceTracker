using GraphQLDto.Seller;
using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface ISellerService
    {
        Seller_QL AddSeller(Seller_QL seller);
        Seller_QL GetSellerById(long id);
        IQueryable<Seller_QL> GetAllSellers();
        void RemoveSeller(long id);
    }
}
