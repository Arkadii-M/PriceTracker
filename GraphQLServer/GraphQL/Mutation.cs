using GraphQLDto;
using GraphQLServer.Services;
using UseFilteringAttribute = HotChocolate.Data.UseFilteringAttribute;

namespace GraphQLServer.GraphQL
{
    public class Mutation
    {
        //private readonly IUserService _userService;

        //public Mutation(IUserService userService)
        //{
        //    _userService = userService;
        //}

        // History mutations
        [GraphQLName("addHistory")]
        public HistoryQLPayload AddHistory(HistoryQLInput history, IHistoryService serivice) => serivice.AddHistory(history);

        [GraphQLName("removeHistory")]
        public void RemoveHistory(long id, IHistoryService serivice) => serivice.RemoveHistory(id);

        // Product mutations
        [GraphQLName("addProduct")]
        public ProductQLPayload AddProdcut(ProductQLInput product, IProductService service) => service.AddProduct(product);

        [GraphQLName("removeProduct")]
        public void RemoveProduct(long id,IProductService service) => service.RemoveProduct(id);

        // Seller mutation
        [GraphQLName("addSeller")]
        public SellerQLPayload AddSeller(SellerQLInput seller, ISellerService service) => service.AddSeller(seller);

        [GraphQLName("removeSeller")]
        public IQueryable<SellerQLPayload> GetAllSellers(ISellerService service) => service.GetAllSellers();


        // Subsriptions queries
        [GraphQLName("addSubscription")]
        public SubscriptionQLPayload AddSubcription(SubscriptionQLInput subscription, ISubscriptionService service) => service.AddSubscription(subscription);

        [UseFiltering]
        [GraphQLName("updateSubscription")]
        public SubscriptionQLPayload UpdateSubcription(SubscriptionQLUpdate subscription, ISubscriptionService service) => service.UpdateSubscription(subscription);

        [GraphQLName("removeSubscription")]
        public void RemoveSubscription(long id,ISubscriptionService service) => service.RemoveSubscription(id);


        //[GraphQLName("users")]
        //public IQueryable<UserQLPayload> GetAllUsers(IUserService service) => service.GetAll();

    }


}
