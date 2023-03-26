using DTO.Api;
using Microsoft.AspNetCore.Mvc;
using StrawberryShake;

namespace Api.Controllers;

public static class ProductEndpoints
{
    public static void MapProductEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Product", async ([FromServices] IPriceTrackerClient client) =>
        {
            var products_payload = await client.GetAllProducts.ExecuteAsync();
            products_payload.EnsureNoErrors();

            //TODO: use mapper
            var res = new List<Product>();
            foreach (var product in products_payload.Data.Products)
                res.Add(new Product { ProductId = product.ProductId, Link = product.Link, Name = product.Name, SellerId = product.SellerId });

            return res;
        })
        .WithName("GetAllProducts");

        routes.MapGet("/api/Product/{id}", async (int id, [FromServices] IPriceTrackerClient client) =>
        {
            var product_payload =  await client.GetProductById.ExecuteAsync(id);
            product_payload.EnsureNoErrors();
            var product = product_payload.Data.Product;

            return new Product { ProductId = product.ProductId, Link = product.Link, Name = product.Name, SellerId = product.SellerId };
        })
        .WithName("GetProductById");

        routes.MapGet("/api/Product/{url}", async (string url, [FromServices] IPriceTrackerClient client) =>
        {
            var product_payload = await client.GetProdctByUrl.ExecuteAsync(url);
            product_payload.EnsureNoErrors();
            var product = product_payload.Data.Products.FirstOrDefault();

            return new Product { ProductId = product.ProductId, Link = product.Link, Name = product.Name, SellerId = product.SellerId };
        }).WithName("GetProductByUrl");

        routes.MapGet("/api/Product/history/{id}",async (long id, [FromServices] IPriceTrackerClient client) =>
        {
            var history_payload = await client.GetProductPriceHisrory.ExecuteAsync(id);
            history_payload.EnsureNoErrors();


            var product = history_payload.Data.Product;
            var product_history = history_payload.Data.Histories;

            //TODO: add mapper
            var history = new List<ProductHistory>();
            foreach (var h in product_history)
                history.Add(new ProductHistory(h.HistoryId, h.ProductId, h.Datetime.DateTime, h.Price, h.InStock));


            return new ProductPriceHistory()
            {
                Product = new Product() { ProductId = product.ProductId,Link = product.Link,Name = product.Name,SellerId = product.SellerId,LastPrice = history.Last().Price },
                ProductHistory = history
            };
        }).WithName("GetProductPriceHistory");

        //routes.MapPut("/api/Product/{id}", (int id, Product input) =>
        //{
        //    return Results.NoContent();
        //})
        //.WithName("UpdateProduct");

        //routes.MapPost("/api/Product/", (Product model) =>
        //{
        //    //return Results.Created($"/Products/{model.ID}", model);
        //})
        //.WithName("CreateProduct");

        //routes.MapDelete("/api/Product/{id}", (int id) =>
        //{
        //    //return Results.Ok(new Product { ID = id });
        //})
        //.WithName("DeleteProduct");
    }
}
