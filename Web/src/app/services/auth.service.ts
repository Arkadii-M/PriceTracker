import { Injectable, Inject, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
/*import { JwtHelperService } from '@auth0/angular-jwt';*/
import { map } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';


export class LogInResult {
  constructor(public loggedin: boolean) { }
}

@Injectable()
export class AuthService {
  private jwtHelper: JwtHelperService = new JwtHelperService();
  @Output() fireIsLoggedIn: EventEmitter<LogInResult> = new EventEmitter<LogInResult>();

  constructor(private http: HttpClient, @Inject('BASE_API_URL') private baseUrl: string) { }

  IsLogin(): boolean {
    return !this.jwtHelper.isTokenExpired(localStorage.getItem('token'));
  }

  Login(username: string, password: string): void {
    this.http.post(this.baseUrl + '/api/User/login', { Username: username, Password: password }).subscribe(res => console.log(res), err => console.log(err));


    this.http.post(this.baseUrl + '/api/User/login', { UserName: username, Password: password }).pipe(
      map((data: any) => { localStorage.removeItem('token'); localStorage.setItem('token', data.token); },
        catchError(err => {
          return [];
        })))
      .subscribe(res => {
        this.fireIsLoggedIn.emit(new LogInResult(this.IsLogin()));
      }, err => { this.fireIsLoggedIn.emit(new LogInResult(false)); });
  }

  Logout(): void {
    localStorage.removeItem('token');
    this.fireIsLoggedIn.emit({ loggedin: false });
  }

  getLoginEmitter() { return this.fireIsLoggedIn; }
}
