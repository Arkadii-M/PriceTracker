using AutoMapper;
using DTO.GraphQL;
using GraphQLServer.DbModels;
using GraphQLServer.Services;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using System.Security.Claims;
using UseFilteringAttribute = HotChocolate.Data.UseFilteringAttribute;

namespace GraphQLServer.GraphQL
{
    public class Query
    {
        //public IQueryable<GraphQLDto.User.UserPayload_QL> Users(UserService usersService) => _userService.GetAll();


        // History queries
        [GraphQLName("history")]
        public HistoryQLPayload GetHistoryById(long id, IHistoryService serivice) => serivice.GetHistoryById(id);

        [UseFiltering]
        [GraphQLName("histories")]
        public IQueryable<HistoryQLPayload> GetAllHistories(IHistoryService serivice) => serivice.GetAllHistories();


        [GraphQLName("productHistory")]
        public IQueryable<HistoryQLPayload> GetProductHistory(long productId,IHistoryService serivice) => serivice.GetAllHistoryForProductId(productId);



        // Product queries
        [GraphQLName("product")]
        public ProductQLPayload GetProductById(long id, IProductService service) => service.GetProductById(id);

        [UseFiltering]
        [GraphQLName("products")]
        public IQueryable<ProductQLPayload> GetAllProducts(IProductService service) => service.GetAllProducts();

        // Seller queries
        [GraphQLName("seller")]
        public SellerQLPayload GetSellerById(long id, ISellerService service) => service.GetSellerById(id);

        [UseFiltering]
        [GraphQLName("sellers")]
        public IQueryable<SellerQLPayload> GetAllSellers(ISellerService service) => service.GetAllSellers();


        // Subsriptions queries
        [GraphQLName("subscription")]
        public SubscriptionQLPayload GetSubscriptionById(long id, ISubscriptionService service) => service.GetSubscriptionById(id);

        [UseFiltering]
        [GraphQLName("subscriptions")]
        public IQueryable<SubscriptionQLPayload> GetAllSubscriptions(ISubscriptionService service) => service.GetAllSubscriptions();

        [Authorize]
        [GraphQLName("userSubscriptions")]
        public IQueryable<SubscriptionQLPayload> GetUserSubscriptions(ClaimsPrincipal claimsPrincipal, ISubscriptionService service)
        {
            var id = Convert.ToInt64(claimsPrincipal.FindFirstValue("id"));
            return service.GetAllSubscriptionsForUserId(id);
        }


        // Updates queries
        [GraphQLName("update")]
        public UpdateQLPayload GetUpdateById(long id, IUpdateService service) => service.GetUpdateById(id);

        [UseFiltering]
        [GraphQLName("updates")]
        public IQueryable<UpdateQLPayload> GetAllUpdates(IUpdateService service) => service.GetAllUpdates();

        [UseFiltering]
        [GraphQLName("users")]
        public IQueryable<UserQLPayload> GetAllUsers(IUserService service) => service.GetAll();

        [GraphQLName("user")]
        public UserQLPayload GetUserById(long id,IUserService service) => service.GetUserById(id);

        [GraphQLName("loginUser")]
        public LoginUserQLPayload LoginUser(LoginUserQLInput user_data, IUserService service) => service.LoginUser(user_data);

        [Authorize]
        [GraphQLName("getMe")]
        public LoginUserQLPayload GetMe(ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal.FindFirstValue("id");
            var name = claimsPrincipal.FindFirstValue(ClaimTypes.Name);

            return new LoginUserQLPayload(Convert.ToInt64(id?? "0"), name ?? "none", "none", true);
        }


    }


}
