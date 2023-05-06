import { Component, Inject, ViewChild, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { SubscriptionService } from '../../services/subscription.service';
import { NewSubscription, Subscription } from '../../dto/subscription.dto';
import { Product } from '../../dto/product.dto';
import { ProductService } from '../../services/product.service';
import { Validators } from '@angular/forms';

@Component({
  selector: 'subscriptions',
  templateUrl: './subscriptions.component.html',
  styleUrls: ['./subscriptions.component.css'],
  providers: [SubscriptionService, ProductService]
})
export class SubscriptionsComponent implements OnInit {
  public subscriptions: Subscription[] = [];
  public products: Product[] = [];

  public basicData: any;
  public basicOptions: any;

  public chart_visible: boolean = false;
  public dialog_header: string = '';


  public newSub: NewSubscription = new NewSubscription();
  public add_dialog_visible: boolean = false;
  public add_dialog_submitted: boolean = false;


  constructor(private subService: SubscriptionService,
    private productService: ProductService) {
    console.log("sub page load")
  }

  ngOnInit(): void {

    this.subService.getUserSubscriptions().subscribe(res => {
      this.subscriptions = res;
      console.log(this.subscriptions);
      this.subscriptions.forEach((val, id) =>
      {
        this.productService.getById(val.productId).subscribe(prod => { this.products.push(prod); }, err => console.log(err));
      });
    }, err => { console.error(err); })

  
  }

  openAddSubscription() {
    this.add_dialog_visible = true;
    this.add_dialog_submitted = false;
  }
  closeAddSubscription() {
    this.add_dialog_visible = false;
    this.add_dialog_submitted = false;
  }

  addSubscription() {
    console.log(this.newSub);
    this.add_dialog_visible = false;
    this.add_dialog_submitted = true;
    this.subService.addSubscription(this.newSub)
      .subscribe(res => console.log(res), err => console.error(err));
  }


  getSeverity(inStock: boolean): string {
    return (inStock ? 'success' : 'danger');
  }
  getInStockName(inStock: boolean): string {
    return (inStock ? 'In stock' : 'Out of stock');
  }
  getUpdatePeriod(product: Product): number | undefined {
    return this.subscriptions.find((val) => val.productId == product.productId)?.checkMinutes;
  }

  showChart(product: Product): void {

    this.dialog_header = product.name;
    this.chart_visible = true;
    var price_hist: [number?] = [];
    var datetime_hist: [Date?] = [];
    product.history?.forEach((value) => {
      price_hist.push(value.price);
      datetime_hist.push(value.datetime);
    });
    this.basicData = {
      labels: datetime_hist,
/*      labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],*/
      datasets: [
        {
          label: 'Price',
          data: price_hist,
          fill: true,
          borderColor: '#42A5F5',
          tension: .0
        }
      ]
    };
    this.basicOptions = {
      plugins: {
        legend: {
          labels: {
            color: '#495057'
          }
        }
      },
      scales: {
        x: {
          ticks: {
            color: '#495057'
          },
          grid: {
            color: '#ebedef'
          }
        },
        y: {
          ticks: {
            color: '#495057'
          },
          grid: {
            color: '#ebedef'
          }
        }
      }
    };
  }

}
