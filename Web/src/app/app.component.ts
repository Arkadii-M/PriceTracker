import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';
import { ProductService } from './services/product.service';
import { SubscriptionService } from './services/subscription.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ProductService, AuthService, SubscriptionService]
})
export class AppComponent {
  title = 'Web';


  constructor(
    private productService: ProductService,
    private authService: AuthService,
    private subService: SubscriptionService) {
    //productService.getAll().subscribe(res => console.log(res), err => { console.error(err) });
    //productService.getHistory(14).subscribe(res => console.log(res), err => { console.error(err) });
/*    productService.getById(14).subscribe(res => console.log(res), err => { console.error(err) });*/
/*    productService.getByUrl('https://rozetka.com.ua/ua/348297003/p348297003/').subscribe(res => console.log(res), err => { console.error(err) });*/

/*    subService.getUserSubscriptions().subscribe(res => console.log(res), err => console.log(err))*/
    
/*    authService.Login("test_user", "12345");*/
  }
}
