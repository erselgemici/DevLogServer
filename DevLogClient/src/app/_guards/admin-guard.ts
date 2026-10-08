import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../_services/auth-service';

// sadece Admin rolündeki kullanıcıların girebileceği sayfalar için
@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    if (this.authService.loggedIn() && this.authService.isAdmin()) {
      return true;
    }
    // yazar ise kendi blog sayfasına yönlendiriyorum
    this.router.navigate(['/admin/blog']);
    return false;
  }
}
