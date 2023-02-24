using AutoMapper;
using GraphQLDto.History;
using GraphQLDto.Product;
using GraphQLDto.Seller;
using GraphQLDto.Subscription;
using GraphQLDto.Update;
using GraphQLDto.User;
using GraphQLServer.DbModels;
using GraphQLServer.Services;
using HotChocolate;
using HotChocolate.Types;

namespace GraphQLServer.GraphQL
{
    public class Query
    {
        //public IQueryable<GraphQLDto.User.UserPayload_QL> Users(UserService usersService) => _userService.GetAll();


        // History queries
        [GraphQLName("history")]
        public History_QL GetHistoryById(long id, IHistoryService serivice) => serivice.GetHistoryById(id);

        [GraphQLName("histories")]
        public IQueryable<History_QL> GetAllHistories(IHistoryService serivice) => serivice.GetAllHistories();


        // Product queries
        [GraphQLName("product")]
        public Product_QL GetProductById(long id,IProductService service) => service.GetProductById(id);

        [GraphQLName("products")]
        public IQueryable<Product_QL> GetAllProducts(IProductService service) => service.GetAllProducts();

        // Seller queries
        [GraphQLName("seller")]
        public Seller_QL GetSellerById(long id, ISellerService service) => service.GetSellerById(id);

        [GraphQLName("sellers")]
        public IQueryable<Seller_QL> GetAllSellers(ISellerService service) => service.GetAllSellers();


        // Subsriptions queries
        [GraphQLName("subscription")]
        public Subscription_QL GetSubscriptionById(long id, ISubscriptionService service) => service.GetSubscriptionById(id);

        [GraphQLName("subscriptions")]
        public IQueryable<Subscription_QL> GetAllSubscriptions(ISubscriptionService service) => service.GetAllSubscriptions();

        // Updates queries
        [GraphQLName("update")]
        public Update_QL GetUpdateById(long id, IUpdateService service) => service.GetUpdateById(id);

        [GraphQLName("updates")]
        public IQueryable<Update_QL> GetAllUpdates(IUpdateService service) => service.GetAllUpdates();

        [GraphQLName("users")]
        public IQueryable<UserPayload_QL> GetAllUsers(IUserService service) => service.GetAll();


    }


}
