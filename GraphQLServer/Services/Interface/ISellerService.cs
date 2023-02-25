using GraphQLDto;
using GraphQLServer.DbModels;

namespace GraphQLServer.Services
{
    public interface ISellerService
    {
        SellerQLPayload AddSeller(SellerQLInput seller);
        SellerQLPayload GetSellerById(long id);
        IQueryable<SellerQLPayload> GetAllSellers();
        void RemoveSeller(long id);
    }
}
