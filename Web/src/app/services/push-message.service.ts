import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map, Observable, publish } from 'rxjs';
import { NewSubscription, Subscription } from '../dto/subscription.dto';
import { Product } from '../dto/product.dto'
import { Apollo, gql } from 'apollo-angular';
import { Seller } from '../dto/seller.dto';


const ON_HISTORY_UPDATE = gql`
subscription OnNewHistory
{
  onHistoryAdded {
    historyId
    productId
    datetime
    price
    inStock
    product {
      productId
      link
      name
      sellerId
      seller {
        sellerId
        sellerName
      }
    }
  }
}`

@Injectable()
export class PushNotificationService {

  constructor(private apollo: Apollo) {
  }

  getUserSubscriptions(): Observable<Product>{
    //return this.apollo.watchQuery({
    //  query: ON_HISTORY_UPDATE
    //}).pipe(map((data) => {
    //  let payload = data.data as any;

    //  var product: Product = (payload.onHistoryAdded.products as Product);
    //  product.inStock = payload.onHistoryAdded.price;
    //  product.inStock = payload.onHistoryAdded.inStock;
    //  return product;
    //}));

    //return this.apollo.subscribe({
    //  query: ON_HISTORY_UPDATE,
    //}).subscribe(res => {
    //  let payload = res.data as any;
    //  let payload_product = payload.onHistoryAdded.product;
    //  var product: Product = new Product(
    //    payload_product.productId,
    //    payload_product.link,
    //    payload_product.name,
    //    new Seller(payload_product.seller.sellerId, payload_product.seller.sellerName),
    //    payload.onHistoryAdded.price,
    //    payload.onHistoryAdded.inStock,
    //  );
    //  console.log(product);
    //});


    return this.apollo.subscribe({
      query: ON_HISTORY_UPDATE,
    }).pipe(map(data => {
      let payload = data.data as any;
      let payload_product = payload.onHistoryAdded.product;
      var product: Product = new Product(
        payload_product.productId,
        payload_product.link,
        payload_product.name,
        new Seller(payload_product.seller.sellerId, payload_product.seller.sellerName),
        payload.onHistoryAdded.price,
        payload.onHistoryAdded.inStock,
      );
      return product;
    }));
  }
}
