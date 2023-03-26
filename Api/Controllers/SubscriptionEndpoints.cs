using DTO.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StrawberryShake;

namespace Api.Controllers;

public static class SubscriptionEndpoints
{
    public static void MapSubscriptionEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Subscriptions/user/{user_id}",async (int user_id,[FromServices] IPriceTrackerClient client) =>
        {
            var subs_payload = await client.GetUserSubscriprions.ExecuteAsync(user_id);
            subs_payload.EnsureNoErrors();
            var subs = subs_payload.Data.Subscriptions;

            //TODO: user mapper
            var res = new List<Subscription>();
            foreach(var sub in subs)
                res.Add(new Subscription() 
                { 
                    SubscriptionId = sub.SubscriptionId,
                    ProductId = sub.ProductId,
                    UserId = sub.UserId,
                    CheckMinutes = sub.CheckMinutes,
                    Product = new Product()
                    {
                        ProductId = sub.Product.ProductId,
                        Name = sub.Product.Name,
                        Link = sub.Product.Link,
                        SellerId = sub.Product.SellerId                    
                        
                    }
                });


            return res;
        })
        .WithName("GetAllSubscriptionsForUser");

        routes.MapGet("/api/Subscriptions/{id}", async (int id, [FromServices] IPriceTrackerClient client) =>
        {

            var sub_payload = await client.GetSubscriptionById.ExecuteAsync(id);
            sub_payload.EnsureNoErrors();
            var sub = sub_payload.Data.Subscription;
            var update = sub_payload.Data.Update;

            return new Subscription()
            {
                SubscriptionId = id,
                ProductId = sub.ProductId,
                UserId = sub.UserId,
                CheckMinutes = sub.CheckMinutes,
                Product = new Product()
                {
                    ProductId = sub.Product.ProductId,
                    Name = sub.Product.Name,
                    Link = sub.Product.Link,
                    SellerId = sub.Product.SellerId,
                    LastPrice = update.History.Price
                }
            };
        })
        .WithName("GetSubscriptionById");

        routes.MapPost("/api/Subscriptions", async (List<long> ids, [FromServices] IPriceTrackerClient client) =>
        {
            var subs_payload = await client.GetSubsriptionsLastUpdates.ExecuteAsync(ids);
            subs_payload.EnsureNoErrors();

            var updates = subs_payload.Data.Updates;
            var res = new List<Product>();
            foreach(var up in updates)
            {
                var hist = up.History;
                res.Add(new Product()
                {
                    ProductId = hist.Product.ProductId,
                    Name = hist.Product.Name,
                    Link = hist.Product.Link,
                    SellerId = hist.Product.SellerId,
                    LastPrice = hist.Price
                });
            }


            //var sub_payload = await client.GetSubscriptionById.ExecuteAsync(id);
            //sub_payload.EnsureNoErrors();
            //var sub = sub_payload.Data.Subscription;
            //var update = sub_payload.Data.Update;

                //return new Subscription()
                //{
                //    SubscriptionId = id,
                //    ProductId = sub.ProductId,
                //    UserId = sub.UserId,
                //    CheckMinutes = sub.CheckMinutes,
                //    Product = new Product()
                //    {
                //        ProductId = sub.Product.ProductId,
                //        Name = sub.Product.Name,
                //        Link = sub.Product.Link,
                //        SellerId = sub.Product.SellerId,
                //        LastPrice = update.History.Price
                //    }
                //};
        })
        .WithName("GetSubscriptionsByIds");


        routes.MapPost("/api/Subscription/", (Subscription model) =>
        {
            //return Results.Created($"/Subscriptions/{model.ID}", model);
        })
        .WithName("CreateSubscription");

        routes.MapDelete("/api/Subscription/{id}", (int id) =>
        {
            //return Results.Ok(new Subscription { ID = id });
        })
        .WithName("DeleteSubscription");
    }
}
