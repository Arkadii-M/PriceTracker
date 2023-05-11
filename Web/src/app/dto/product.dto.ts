import { Seller } from "./seller.dto";

export class ProductHistory {
  public constructor(
    public historyId: number = 0,
    public productId: number = 0,
    public datetime: Date = new Date(),
    public price: number = 0,
    public inStock: boolean = false,
  ) { }
}


export class Product {
  constructor(
    public productId: number = 0,
    public link: string = '',
    public name: string = '',
    public seller?: Seller,
    public lastPrice: number = 0,
    public inStock: boolean = false,
    public histories: ProductHistory[] =[],
  ) { }
}
