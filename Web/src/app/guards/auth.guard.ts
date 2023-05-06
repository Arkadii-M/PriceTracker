import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable } from "rxjs";
import { AuthService } from "../services/auth.service";
@Injectable({ providedIn: 'root', })
export class AuthenticationGuard implements CanActivate {

  constructor(private auth_service: AuthService, public router: Router) { }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
    console.log(state.url);
    if (!this.auth_service.IsLogin()) {
      this.router.navigate(['login'], { queryParams: { returnUrl: state.url } });
      return false;
    }
    return true;
  }
}
