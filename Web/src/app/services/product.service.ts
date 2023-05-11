import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Product, ProductHistory } from '../dto/product.dto';
import { map, Observable, publish } from 'rxjs';
import { Apollo } from 'apollo-angular';
import { gql } from '@apollo/client/core';
import { Seller } from '../dto/seller.dto';


const PRODUCTS_BY_IDS = gql`
query GetProductsByIds($ids : [Long]!)
{
  products(where:{productId :{in: $ids}} ){
    productId
    link
    name
    sellerId

    seller {
      sellerId
      sellerName
    }
  }
  histories(where: {productId: {in: $ids}})
  {
    historyId
    productId
    datetime
    price
    inStock
  }

}
`;

@Injectable()
export class ProductService {

  constructor(private apollo: Apollo) {
  }

  getByIds(ids: number[]): Observable<Product[]>  {
    return this.apollo.query({
      query: PRODUCTS_BY_IDS,
      variables:
      {
        ids: ids
      }
    }).pipe(
     map((data) => {
       let payload: any = data.data as any;


       var products: Product[] = (payload.products as any[]).map(
         (val) => new Product(val.productId, val.link, val.name, new Seller(val.seller.sellerId, val.seller.sellerName)));
       var all_histories: ProductHistory[] = (payload.histories as any[]).map(
         (val) => new ProductHistory(val.historyId, val.productId, val.datetime, val.price, val.inStock));


      products.forEach((value) =>
      {
        console.log(value);
        value.histories = all_histories.filter(h => h.productId == value.productId);
        value.inStock = value.histories[value.histories.length - 1].inStock;
        value.lastPrice = value.histories[value.histories.length - 1].price;
      })
      return products;
    }));
  }

}
