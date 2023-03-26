import { Component } from '@angular/core';
import { ProductService } from './services/product.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ProductService]
})
export class AppComponent {
  title = 'Web';


  constructor(private productService: ProductService) {
    productService.getAll().subscribe(res => console.log(res), err => { console.error(err) });
  }
}
