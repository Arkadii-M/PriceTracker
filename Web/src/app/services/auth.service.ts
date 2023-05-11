import { Injectable, Inject, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
/*import { JwtHelperService } from '@auth0/angular-jwt';*/
import { map } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginUserQLPayload } from '../dto/loginPayload.dto';
import { gql } from '@apollo/client/core';
import { Apollo } from 'apollo-angular';

const LOGIN_QUERY = gql`
query Login($username : String!,$password : String!)
{
  loginUser(user_data: {username: $username,password:$password}) {
    userId
    username
    token
    is_login
  }
}
`;

export class LogInResult {
  constructor(public loggedin: boolean) { }
}

@Injectable()
export class AuthService {
  private jwtHelper: JwtHelperService = new JwtHelperService();
  @Output() fireIsLoggedIn: EventEmitter<LogInResult> = new EventEmitter<LogInResult>();

  constructor(private apollo: Apollo) { }

  IsLogin(): boolean {
    return !this.jwtHelper.isTokenExpired(localStorage.getItem('token'));
  }

  Login(username: string, password: string): void {
    this.apollo.query <LoginUserQLPayload>({
      query: LOGIN_QUERY,
      variables: {
        username: username,
        password: password,
      }
    }).pipe(
      map((data) => {
        let payload: any = (data.data as any).loginUser;
        console.log(payload);
        console.log(payload.token);
        localStorage.removeItem('token');
        localStorage.setItem('token', payload.token);
      })).subscribe(res => {
        this.fireIsLoggedIn.emit(new LogInResult(this.IsLogin()))
      });


    //this.http.post(this.baseUrl + '/api/User/login', { Username: username, Password: password }).subscribe(res => console.log(res), err => console.log(err));


    //this.http.post(this.baseUrl + '/api/User/login', { UserName: username, Password: password }).pipe(
    //  map((data: any) => { localStorage.removeItem('token'); localStorage.setItem('token', data.token); },
    //    catchError(err => {
    //      return [];
    //    })))
    //  .subscribe(res => {
    //    this.fireIsLoggedIn.emit(new LogInResult(this.IsLogin()));
    //  }, err => { this.fireIsLoggedIn.emit(new LogInResult(false)); });
  }

  Logout(): void {
    localStorage.removeItem('token');
    this.fireIsLoggedIn.emit({ loggedin: false });
  }

  getLoginEmitter() { return this.fireIsLoggedIn; }
}
