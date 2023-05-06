import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, publish } from 'rxjs';
import { NewSubscription, Subscription } from '../dto/subscription.dto';

@Injectable()
export class SubscriptionService {

  constructor(private http: HttpClient, @Inject('BASE_API_URL') public baseUrl: string) {
  }

  private formHeader(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + (localStorage.getItem('token') ?? '')
    });
  }

  getUserSubscriptions(): Observable<Subscription[]> {
    return this.http.get<Subscription[]>(this.baseUrl + '/api/Subscriptions', { headers: this.formHeader() });
  }
  addSubscription(newSub: NewSubscription): Observable<Subscription> {
    return this.http.post<Subscription>(this.baseUrl + '/api/Subscription/add', newSub, { headers: this.formHeader() });
  }
  //getById(subId: number): Observable<Subscription> {
  //  return this.http.get<Subscription>(this.baseUrl + '/api/Subscriptions/' + subId);
  //}
  //getByIds(subIds: number[]): Observable<Subscription[]> {
  //  return this.http.post<Subscription[]>(this.baseUrl + '/api/Subscriptions', { 'ids': subIds })
  //}

  //private formHeader(): HttpHeaders {
  //  return new HttpHeaders({
  //    'Content-Type': 'application/json',
  //    'Authorization': 'Bearer ' + (localStorage.getItem('token') ?? '')
  //  });
  //}

  //getAll(): Observable<Product[]> {
  //  return this.http.get<Product[]>(this.baseUrl + '/api/Product');
  //}
  //getById(id: number): Observable<Product> {
  //  return this.http.get<Product>(this.baseUrl + '/api/Product/ById/' + id);
  //}
  //getByUrl(productUrl: string): Observable<Product> {
  //  return this.http.get<Product>(this.baseUrl + '/api/Product/ByUrl', { headers: new HttpHeaders({ 'url': productUrl }) });
  //}
  //getHistory(id: number): Observable<ProductHistory> {
  //  return this.http.get<ProductHistory>(this.baseUrl + '/api/Product/history/' + id,);
  //}

}
