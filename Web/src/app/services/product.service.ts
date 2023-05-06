import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Product } from '../dto/product.dto';
import { Observable, publish } from 'rxjs';

@Injectable()
export class ProductService {

  constructor(private http: HttpClient, @Inject('BASE_API_URL') public baseUrl: string) {
  }
  private formHeader(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + (localStorage.getItem('token') ?? '')
    });
  }

  //getAll(): Observable<Product[]> {
  //  return this.http.get<Product[]>(this.baseUrl + '/api/Product');
  //}
  getById(id: number): Observable<Product> {
    return this.http.get<Product>(this.baseUrl + '/api/Product/ById/' + id, { headers: this.formHeader() });
  }
  //getByUrl(productUrl: string): Observable<Product> {
  //  return this.http.get<Product>(this.baseUrl + '/api/Product/ByUrl', { headers: new HttpHeaders({ 'url': productUrl }) });
  //}
  //getHistory(id: number): Observable<ProductHistory> {
  //  return this.http.get<ProductHistory>(this.baseUrl + '/api/Product/history/'+id,);
  //}

}
