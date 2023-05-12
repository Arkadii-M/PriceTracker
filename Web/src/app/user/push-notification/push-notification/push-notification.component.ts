import { Component, OnInit } from '@angular/core';
import { Message } from 'primeng/api/message';
import { Product } from '../../../dto/product.dto';
import { Seller } from '../../../dto/seller.dto';
import { PushNotificationService } from '../../../services/push-message.service';



@Component({
  selector: 'app-push-notification',
  templateUrl: './push-notification.component.html',
  styleUrls: ['./push-notification.component.css'],
  providers: [PushNotificationService]
})
export class PushNotificationComponent implements OnInit {
  public messages: Message[] = [];

  constructor(private pushService: PushNotificationService) {
    //this.messages.push({ severity: 'info', closable: true, detail: "details", summary: "new update" });
    //this.messages.push({ severity: 'info', closable: true, detail: "details", summary: "new update" });
    //this.messages.push({ severity: 'info', closable: true, detail: "details", summary: "new update" });
    //this.pushOnNewValue(new Product(0, "", "test", new Seller(1,""), 100, true, undefined));
  }
  ngOnInit() {
    this.pushService.getUserSubscriptions().subscribe(res => {
      var msg: string = `${res.name}, price: ${res.lastPrice}, in stock: ${res.inStock}`;
      this.messages = this.messages.concat([{ severity: 'info', closable: true, detail: msg, summary: "New updates" }]);
    });
  }
  //const addData = (newData) => {
  //  return new Promise((resolve, reject) => {
  //    if (newData) {
  //      dataArray.push(newData);
  //      resolve();
  //    } else {
  //      reject(new Error('Invalid data'));
  //    }
  //  });
  //};

  pushOnNewValue(val: Product) {
    return new Promise((resolve, reject) => {
      var msg: string = `New updates for ${val.name}, price: ${val.lastPrice},in stock: ${val.inStock}`;
      this.messages.push({ severity: 'info', closable: true, detail: msg, summary: "New updates" });
      resolve(msg);

    })

    var msg: string = `New updates for ${val.name}, price: ${val.lastPrice},in stock: ${val.inStock}`;
    console.log(msg);
    this.messages.push({ severity: 'info', closable: true, detail: msg, summary: "New updates" });
  }

}
