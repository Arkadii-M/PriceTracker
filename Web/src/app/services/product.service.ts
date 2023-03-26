import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Product } from '../dto/product.dto';
import { Observable } from 'rxjs';

@Injectable()
export class ProductService {

  constructor(private http: HttpClient) {
  }
  //private formHeader(): HttpHeaders {
  //  return new HttpHeaders({
  //    'Content-Type': 'application/json',
  //    'Authorization': 'Bearer ' + (localStorage.getItem('token') ?? '')
  //  });
  //}

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>('https://localhost:60136' + '/api/Product');
  }
}
