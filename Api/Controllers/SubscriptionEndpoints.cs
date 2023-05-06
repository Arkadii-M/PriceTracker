using Api.Helpers.Services;
using DTO.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using StrawberryShake;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Policy;

namespace Api.Controllers;

public static class SubscriptionEndpoints
{
    public static void MapSubscriptionEndpoints (this IEndpointRouteBuilder routes)
    {
        //routes.MapGet("/api/Subscriptions/user/{user_id}", async (int user_id, [FromServices] IPriceTrackerClient client) =>
        //{
        //    var subs_payload = await client.GetUserSubscriprions.ExecuteAsync(user_id);
        //    subs_payload.EnsureNoErrors();

        //    var dbsubs = subs_payload.Data.Subscriptions;

        //    //TODO: user mapper
        //    var res = new List<Subscription>();
        //    foreach (var sub in dbsubs)
        //        res.Add(new Subscription
        //        {
        //            SubscriptionId = sub.SubscriptionId,
        //            ProductId = sub.ProductId,
        //            UserId = sub.UserId,
        //            CheckMinutes = sub.CheckMinutes,

        //        });
        //    return res;
        //})
        //.WithName("GetAllSubscriptionsForUser");

        routes.MapGet("/api/Subscriptions", async ([FromHeader] string Authorization, [FromServices] IPriceTrackerClient client) =>
        {
            var jwtStr = Authorization.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtStr);
            var token = jsonToken as JwtSecurityToken;

            var userId = Convert.ToInt32(token.Claims.First(claim => claim.Type == "id").Value);

            var subs_payload = await client.GetUserSubscriprions.ExecuteAsync(userId);
            subs_payload.EnsureNoErrors();

            var dbsubs = subs_payload.Data.Subscriptions;

            //TODO: user mapper
            var res = new List<Subscription>();
            foreach (var sub in dbsubs)
                res.Add(new Subscription
                {
                    SubscriptionId = sub.SubscriptionId,
                    ProductId = sub.ProductId,
                    UserId = sub.UserId,
                    CheckMinutes = sub.CheckMinutes,

                });
            return res;
        }).WithName("GetAllSubscriptionsForUser").RequireAuthorization();



        //routes.MapGet("/api/Subscriptions/{id}", async (int id, [FromServices] IPriceTrackerClient client) =>
        //{

        //    var sub_payload = await client.GetSubscriptionById.ExecuteAsync(id);
        //    sub_payload.EnsureNoErrors();
        //    var sub = sub_payload.Data.Subscription;
        //    var update = sub_payload.Data.Update;

        //    return new Subscription()
        //    {
        //        SubscriptionId = id,
        //        ProductId = sub.ProductId,
        //        UserId = sub.UserId,
        //        CheckMinutes = sub.CheckMinutes,
        //        Product = new Product()
        //        {
        //            ProductId = sub.Product.ProductId,
        //            Name = sub.Product.Name,
        //            Link = sub.Product.Link,
        //            SellerId = sub.Product.SellerId,
        //            LastPrice = update.History.Price
        //        }
        //    };
        //})
        //.WithName("GetSubscriptionById");

        //routes.MapPost("/api/Subscriptions", async ([FromBody] List<long> ids,[FromServices] IPriceTrackerClient client) =>
        //{            
        //    var subs_payload = await client.GetSubsriptionsLastUpdates.ExecuteAsync(ids);
        //    subs_payload.EnsureNoErrors();

        //    var updates = subs_payload.Data.Updates;
        //    var res = new List<Product>();
        //    foreach(var up in updates)
        //    {
        //        var hist = up.History;
        //        res.Add(new Product()
        //        {
        //            ProductId = hist.Product.ProductId,
        //            Name = hist.Product.Name,
        //            Link = hist.Product.Link,
        //            SellerId = hist.Product.SellerId,
        //            LastPrice = hist.Price
        //        });
        //    }


        //    //var sub_payload = await client.GetSubscriptionById.ExecuteAsync(id);
        //    //sub_payload.EnsureNoErrors();
        //    //var sub = sub_payload.Data.Subscription;
        //    //var update = sub_payload.Data.Update;

        //        //return new Subscription()
        //        //{
        //        //    SubscriptionId = id,
        //        //    ProductId = sub.ProductId,
        //        //    UserId = sub.UserId,
        //        //    CheckMinutes = sub.CheckMinutes,
        //        //    Product = new Product()
        //        //    {
        //        //        ProductId = sub.Product.ProductId,
        //        //        Name = sub.Product.Name,
        //        //        Link = sub.Product.Link,
        //        //        SellerId = sub.Product.SellerId,
        //        //        LastPrice = update.History.Price
        //        //    }
        //        //};
        //})
        //.WithName("GetSubscriptionsByIds");


        routes.MapPost("/api/Subscription/add", async (
            [FromHeader] string Authorization,
            [FromBody] AddSubscription newSub,
            [FromServices] IPriceTrackerClient client,
            [FromServices] IRabbitMqClient queueClient) =>
        {
            //Try to get product by url. If not exists => add
            var product_payload = await client.GetProdctByUrl.ExecuteAsync(newSub.Url);
            product_payload.EnsureNoErrors();

            var dbproduct = product_payload.Data.Products.FirstOrDefault();
            if (dbproduct is null) // no product in database
            {
                const int POLL_INTERVAL = 10*1000;//10 seconds
                bool ok = false;
                queueClient.SendUrlToParse(newSub.Url);
                for(int i = 0; i < 5;++i)
                {
                    await Task.Delay(POLL_INTERVAL);
                    // check if exists
                    var new_product_payload = await client.GetProdctByUrl.ExecuteAsync(newSub.Url);
                    new_product_payload.EnsureNoErrors();
                    dbproduct = new_product_payload.Data.Products.FirstOrDefault();
                    if (dbproduct is not null)
                    {
                        ok = true;
                        break;
                    }
                }

                if(!ok)
                    return Results.NotFound(newSub.Url);
            }

            var jwtStr = Authorization.Split(" ").Last();
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtStr);
            var token = jsonToken as JwtSecurityToken;

            var userId = Convert.ToInt32(token.Claims.First(claim => claim.Type == "id").Value);
            int checkMinutes = Convert.ToInt32(newSub.CheckMinutes);

            var subscription_payload = await client.AddSubscription.ExecuteAsync(
                new SubscriptionQLInput { UserId = userId, ProductId = dbproduct.ProductId, CheckMinutes = checkMinutes });

            subscription_payload.EnsureNoErrors();

            return Results.Created("", subscription_payload.Data.AddSubscription);
        })
        .WithName("CreateSubscription").RequireAuthorization();

        routes.MapDelete("/api/Subscription/{id}", async (long id, [FromServices] IPriceTrackerClient client) =>
        {
            // TODO: remove subscription
            return Results.NotFound(id);
        })
        .WithName("DeleteSubscription").RequireAuthorization();
    }
}
