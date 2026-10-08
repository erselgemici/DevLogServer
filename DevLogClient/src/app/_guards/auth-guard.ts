import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, GuardResult, MaybeAsync, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../_services/auth-service';

@Injectable({
  providedIn: 'root'
})
// admin sayfalarına giriş yapmadan girilmesin diye
export class AuthGuard implements CanActivate {


  constructor(private authserService: AuthService,
              private router: Router
  ){}


  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): MaybeAsync<GuardResult> {

// token var ve süresi dolmadıysa geçebilir
if(this.authserService.loggedIn()){
  return true;
}
console.log("auth guard");
// değilse ana sayfaya atıyorum
this.router.navigate([""]);
return false;

  }


}
