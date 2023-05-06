import { Product } from "./product.dto";

export class Subscription {
  public subscriptionId: number = 0;
  public userId: number = 0;
  public productId: number = 0;
  public checkMinutes: number = 0;
}

export class NewSubscription {
  public url: string = '';
  public checkMinutes: number = 5;
}
