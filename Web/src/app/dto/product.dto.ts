import { Seller } from "./seller.dto";

export class ProductHistory {
  public historyId: number = 0;
  public productId: number = 0;
  public datetime: Date = new Date();
  public price: number = 0;
  public inStock: boolean = false;
}


export class Product {
  public productId: number = 0;
  public link: string = '';
  public name: string = '';
  public seller?: Seller;
  public lastPrice: number = 0;
  public inStock: boolean = false;

  public history?: [ProductHistory];
}
