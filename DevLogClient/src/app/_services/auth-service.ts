import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

constructor(private http: HttpClient,
            private router: Router


){}

// backend adresi, api https profilinde 7000 portunda çalışıyor
baseUrl = "https://localhost:7000/api/users/login"
decodedToken:any;
// token'ı decode etmek ve süresini kontrol etmek için
jwtHelper = new JwtHelperService();


login(model){
  return this.http.post<any>(this.baseUrl,model);
}

// çıkışta token'ı silip ana sayfaya yönlendiriyorum
logout(){
  localStorage.removeItem("token");
  this.router.navigate([""]);
}


decodeToken(){
let token = localStorage.getItem("token");
this.decodedToken = this.jwtHelper.decodeToken(token);
return this.decodedToken;
}

// token'ın süresi dolmadıysa giriş yapılmış sayılıyor
loggedIn(){
  const token = localStorage.getItem("token");
  return !this.jwtHelper.isTokenExpired(token);
}

// backend'de token'a sub olarak user id'yi koymuştum
getUserId(){
  let decodedToken = this.decodeToken();
  return decodedToken.sub;
}

// token'daki rol bilgisi tek rol varsa string, birden fazlaysa dizi olarak geliyor
getRoles(): string[]{
  const decodedToken = this.decodeToken();
  const role = decodedToken?.role;
  if (!role) return [];
  return Array.isArray(role) ? role : [role];
}

isAdmin(): boolean{
  return this.getRoles().includes('Admin');
}

getUserName(){
  let decodedToken = this.decodeToken();
  return decodedToken.name;
}


}
