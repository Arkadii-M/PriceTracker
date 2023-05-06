using DTO.Api;
using Microsoft.AspNetCore.Mvc;
using StrawberryShake;

namespace Api.Controllers;

public static class ProductEndpoints
{
    public static void MapProductEndpoints (this IEndpointRouteBuilder routes)
    {
        //routes.MapGet("/api/Product", async ([FromServices] IPriceTrackerClient client) =>
        //{
        //    // get products
        //    var products_payload = await client.GetAllProducts.ExecuteAsync();
        //    products_payload.EnsureNoErrors();


        //    // get sellers

        //    // get history for each product


        //    return products_payload.Data.Products;
        //})
        //.WithName("GetAllProducts");

        routes.MapGet("/api/Product/ById/{id}", async (int id, [FromServices] IPriceTrackerClient client) =>
        {
            var product_payload = await client.GetProductById.ExecuteAsync(id);
            product_payload.EnsureNoErrors();
            var dbproduct = product_payload.Data.Product;

            var product = new Product { 
                ProductId = dbproduct.ProductId,
                Link = dbproduct.Link,
                Name = dbproduct.Name};

            // get selller
            var seller_payload = await client.GetSellerById.ExecuteAsync(dbproduct.SellerId);
            seller_payload.EnsureNoErrors();
            var dbseller = seller_payload.Data.Seller;

            product.Seller = new Seller { SellerId = dbseller.SellerId, SellerName = dbseller.SellerName };

            // get history
            var history_payload = await client.GetProductPriceHisrory.ExecuteAsync(product.ProductId);
            history_payload.EnsureNoErrors();

            var dbhistory = history_payload.Data.Histories;

            var history = new List<ProductHistory>();
            foreach( var h in dbhistory)
            {
                history.Add(new ProductHistory(h.HistoryId, h.ProductId, h.Datetime.DateTime, h.Price, h.InStock));
            }
            product.LastPrice = history.Last().Price;
            product.InStock = history.Last().InStock;
            product.History = history;



            return product;
        })
        .WithName("GetProductById");

        //routes.MapGet("/api/Product/ByUrl", async ([FromHeader] string url, [FromServices] IPriceTrackerClient client) =>
        //{
        //    //var product_payload = await client.GetProdctByUrl.ExecuteAsync(url);
        //    //product_payload.EnsureNoErrors();
        //    //var product = product_payload.Data.Products.FirstOrDefault();

        //    //return new Product { ProductId = product.ProductId, Link = product.Link, Name = product.Name, SellerId = product.SellerId };
        //    var product_payload = await client.GetProdctByUrl.ExecuteAsync(url);
        //    product_payload.EnsureNoErrors();

        //    return product_payload.Data.Products.First();
        //}).WithName("GetProductByUrl");

        //routes.MapGet("/api/Product/history/{id}",async (long id, [FromServices] IPriceTrackerClient client) =>
        //{
        //    var history_payload = await client.GetProductPriceHisrory.ExecuteAsync(id);
        //    history_payload.EnsureNoErrors();


        //    var product = history_payload.Data.Product;
        //    var product_history = history_payload.Data.Histories;

        //    //TODO: add mapper
        //    var history = new List<ProductHistory>();
        //    foreach (var h in product_history)
        //        history.Add(new ProductHistory(h.HistoryId, h.ProductId, h.Datetime.DateTime, h.Price, h.InStock));


        //    return new ProductPriceHistory()
        //    {
        //        Product = new Product() { ProductId = product.ProductId, Link = product.Link, Name = product.Name, SellerId = product.SellerId, LastPrice = history.Last().Price },
        //        ProductHistory = history
        //    };

        //}).WithName("GetProductPriceHistory");
    }
}
