using DTO.GraphQL;
using GraphQLServer.Services;
using GraphQLServer.Services.Interface;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System.Security.Claims;
using UseFilteringAttribute = HotChocolate.Data.UseFilteringAttribute;

namespace GraphQLServer.GraphQL
{
    public class Mutation
    {
        // History mutations

        [GraphQLName("addHistory")]
        public async Task<HistoryQLPayload> AddHistory(
            HistoryQLInput history,
            [Service] IHistoryService service,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            var ret = service.AddHistory(history);
            await eventSender.SendAsync(nameof(AddHistory), ret.HistoryId, cancellationToken);
            return ret;
        }

        //[GraphQLName("removeHistory")]
        //public void RemoveHistory(long id, IHistoryService serivice) => serivice.RemoveHistory(id);

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

        // Subsriptions queries
        [Authorize]
        [GraphQLName("addSubscriptionByLink")]
        public SubscriptionQLPayload AddSubcriptionByLink(SubscriptionByLinkQLInput sub, ClaimsPrincipal claimsPrincipal, IRabbitMqClient rabbitMqClient,
            IProductService productService, ISubscriptionService subService)
        {
            var id = Convert.ToInt64(claimsPrincipal.FindFirstValue("id"));

            var product = productService.GetProductByLink(sub.link);
            if(product is null)
            {
                rabbitMqClient.SendUrlToParse(sub.link);
            }
            const int POLL_INTERVAL = 5 * 1000;//5 seconds
            for (int i = 0; i < 5; ++i)
            {
                Task.Delay(POLL_INTERVAL).Wait();
                product = productService.GetProductByLink(sub.link);
                if(product is not null)
                {
                    return subService.AddSubscription(new SubscriptionQLInput(id, product.ProductId, sub.CheckMinutes));
                }
            }
            throw new Exception("No subscription was added");
        }



        [UseFiltering]
        [GraphQLName("updateSubscription")]
        public SubscriptionQLPayload UpdateSubcription(SubscriptionQLUpdate subscription, ISubscriptionService service) => service.UpdateSubscription(subscription);

        [GraphQLName("removeSubscription")]
        public void RemoveSubscription(long id,ISubscriptionService service) => service.RemoveSubscription(id);


        [GraphQLName("addUser")]
        public UserQLPayload AddUser(CreateUserQLInput user,IUserService service) => service.CreateUser(user);
    }


}
